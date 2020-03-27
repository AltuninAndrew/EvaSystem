using EvaSystem.Data;
using EvaSystem.Models;
using EvaSystem.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly DataContext _dataContext;
        private PositionManager _positionManager;


        public EvaluationService(UserManager<UserModel> userManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _positionManager = new PositionManager(dataContext);
        }

        public async Task<ChangedInformationResultModel> AddCriterionsAsync(string positionName, CriterionModel[] criterions)
        {
            var foundPos = await _positionManager.GetPositionByNameAsync(positionName);

            if (foundPos == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "Position not found" } };
            }

            List<string> errors = new List<string>();

            List<EvaluationСriterionModel> critModels = new List<EvaluationСriterionModel>();
            List<CriterionToPositionModel> critToPosModels = new List<CriterionToPositionModel>();

            // adding new elements in models collections
            foreach (var element in criterions)
            {
                if(!string.IsNullOrEmpty(element.Name))
                {

                    var existCrit = await _dataContext.Criterions.FirstOrDefaultAsync(x => x.CriterionName == element.Name);

                    var existPair = await _dataContext.CriterionsToPosition.FirstOrDefaultAsync(x =>
                                                                            (x.CriterionName == element.Name
                                                                             && x.Position.PositionName == positionName));
                    if (existPair == null)
                    {
                        if(existCrit == null)
                        {
                            existCrit = new EvaluationСriterionModel
                            {
                                CriterionName = element.Name,
                                Weight = element.Weight
                            };

                            critModels.Add(existCrit);
                        }

                        var newCritToPosModel = new CriterionToPositionModel
                        {
                            Criterion = existCrit,
                            CriterionName = element.Name,
                            Position = foundPos,
                            PositionId = foundPos.PositionId
                        };

                        critToPosModels.Add(newCritToPosModel);
                    }
                    else
                    {
                        errors.Add($"{element.Name} already exist");
                    }
                } 
                else
                {
                    errors.Add($"{element.Name} is null");
                }
                      
            }

            if(critToPosModels.Count>0)
            {
                await _dataContext.CriterionsToPosition.AddRangeAsync(critToPosModels);

                if(critModels.Count>0)
                {
                    await _dataContext.Criterions.AddRangeAsync(critModels);
                }

                await _dataContext.SaveChangesAsync();
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = errors };
            }

            return new ChangedInformationResultModel { Success = true, ErrorsMessages = errors };
        }

        public async Task<List<CriterionModel>> GetCriterionsAsync(string positionName)
        {
            var foundPosition = await _positionManager.GetPositionByNameAsync(positionName);

            if(foundPosition == null)
            {
                return null;
            }

            var critsToPos =  await _dataContext.CriterionsToPosition.Include(x => x.Criterion)
                .Where(x => x.Position.PositionName == positionName).ToListAsync();

            return critsToPos.Select(x => new CriterionModel 
            { 
                Name = x.CriterionName, 
                Weight = x.Criterion.Weight
            }).ToList();

        }

        public async Task<List<CriterionModel>> GetCriterionsForUserAsync(string username)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if(foundUser == null)
            {
                return null;
            }

            var critsToPos = await _dataContext.CriterionsToPosition.Include(x => x.Criterion)
                .Where(x => x.PositionId == foundUser.PositionId).ToListAsync();


            return critsToPos.Select(x => new CriterionModel
            {
                Name = x.CriterionName,
                Weight = x.Criterion.Weight
            }).ToList();

        }

        public async Task<ChangedInformationResultModel> RateUserAsync(string username, ScorePerCriterionModel[] scores)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if(foundUser == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }

            List<string> errors = new List<string>();
            List<ScoreModel> scoreModels = new List<ScoreModel>();

            int countExistRatig = 0;


            foreach (var element in scores)
            {
                var foundCritToPos = await _dataContext.CriterionsToPosition.FirstOrDefaultAsync
                    (x =>

                        (x.CriterionName == element.CriterionName) &&
                        (x.PositionId == foundUser.PositionId)
                    );


                if (foundCritToPos != null)
                {
                    var foundRating = await _dataContext.Scores.FirstOrDefaultAsync
                        (x =>
                             (x.UserName == username) &&
                             (x.CriterionName == element.CriterionName)
                        );

                    if (foundRating == null)
                    {
                        scoreModels.Add(new ScoreModel
                        {
                            Criterion = await _dataContext.Criterions.FirstOrDefaultAsync(x => x.CriterionName == element.CriterionName),
                            CriterionName = element.CriterionName,
                            Score = element.Score,
                            UserName = username
                        });
                    }
                    else
                    {
                        foundRating.Score = element.Score;
                        _dataContext.Scores.Update(foundRating);
                        countExistRatig++;
                    }

                }
                else
                {
                    errors.Add($"{element.CriterionName} criterion to pos not found");
                }

            }

            if(scoreModels.Count == 0 && countExistRatig == 0)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = errors };
            }

            if(scoreModels.Count>0)
            {
                await _dataContext.AddRangeAsync(scoreModels);
                await _dataContext.SaveChangesAsync();
            }
            else if(countExistRatig>0)
            {
                await _dataContext.SaveChangesAsync();
            }

            return new ChangedInformationResultModel { Success = true, ErrorsMessages = errors };

        }

        public async Task<UserRatingInformationModel> GetRatingAsync(string username)
        {
            var foundUser = await _dataContext.Users.Include(x => x.Position).FirstOrDefaultAsync(x => x.UserName == username);

            if(foundUser == null)
            {
                return null;
            }

            if(await _dataContext.Scores.FirstOrDefaultAsync(x=>x.UserName == username) == null)
            {
                return null;
            }

            UserRatingInformationModel result = new UserRatingInformationModel
            {
                UserName = username,
                PositionName = foundUser.Position.PositionName,
                ScorePerCriterion = await _dataContext.Scores.Where(x => x.UserName == username).Select(x => new ScorePerCriterionModel
                {
                    CriterionName = x.CriterionName,
                    Score = x.Score
                }).ToListAsync(),
            };

            return result;

        }
    }
}
