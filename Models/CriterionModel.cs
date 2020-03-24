using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class CriterionModel
    {
        [Required]
        public string Name { get; set; }
        public float Weight { get; set; }
    }
}
