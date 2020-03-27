using EvaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Services.Interfaces
{
    public interface IEvaluationService
    {
        public Task<ChangedInformationResultModel> AddCriterionsAsync(string positionName, CriterionModel[] criterions);

        public Task<List<CriterionModel>> GetCriterionsAsync(string positionName);

        public Task<List<CriterionModel>> GetCriterionsForUserAsync(string username);

        public Task<ChangedInformationResultModel> RateUserAsync(string username, ScorePerCriterionModel[] scores);

        public Task<UserRatingInformationModel> GetRatingAsync(string username);
    }
}
