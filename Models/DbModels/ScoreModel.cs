using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
    
namespace EvaSystem.Models
{
    public class ScoreModel
    {
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserModel User { get; set; }

        public string CriterionName { get; set; }

        public int NumOfTimesRate { get; set; }

        [ForeignKey(nameof(CriterionName))]
        public EvaluationСriterionModel Criterion { get; set; }

        
        public float Score { get; set; }

        [Key]
        public int Id { get; set; }
    }
}
