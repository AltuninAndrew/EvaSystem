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
    }
}
