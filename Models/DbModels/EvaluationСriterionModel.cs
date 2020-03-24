using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class EvaluationСriterionModel
    {

        [Key]
        public string CriterionName { get; set; }

        public float Weight { get; set; }

    }
}
