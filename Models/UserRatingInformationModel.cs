using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class UserRatingInformationModel
    {
        public string UserName { get; set; }

        public string PositionName { get; set; }

        public float CurrentRating { get; set; }

        public List<ScorePerCriterionModel> ScorePerCriterion { get; set; }



    }
}
