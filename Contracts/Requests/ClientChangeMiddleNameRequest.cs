using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangeMiddleNameRequest
    {
        [MinLength(2, ErrorMessage = "Middle name lenght should be more then 1 chars")]
        [MaxLength(50, ErrorMessage = "Middle name lenght should be less then 50 char")]
        public string NewMiddleName { get; set; }
    }
}
