using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class ChangedInformationResultModel
    {
        public bool Success { get; set; }
        public IEnumerable<string> ErrorsMessages { get; set; }
    }
}
