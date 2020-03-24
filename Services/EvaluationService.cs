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

    }
}
