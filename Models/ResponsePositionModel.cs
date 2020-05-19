using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class ResponsePositionModel
    {
        public string PositionName { get; set; }

        public IEnumerable<CriterionModel> Criterions {get;set;}

    }
}
