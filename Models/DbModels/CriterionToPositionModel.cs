using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class CriterionToPositionModel
    {
        public int PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public PositionModel Position { get; set; }

        public string CriterionName { get; set; }

        [ForeignKey(nameof(CriterionName))]
        public EvaluationСriterionModel Criterion { get; set; }

        [Key]
        public int Id { get; set; }

    }
}
