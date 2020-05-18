using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangeFirstNameRequest
    {
        [MinLength(2, ErrorMessage = "First name lenght should be more then 1 chars")]
        [MaxLength(50, ErrorMessage = "Firs name lenght should be less then 50 char")]
        public string NewFirstName { get; set; }
    }
}
