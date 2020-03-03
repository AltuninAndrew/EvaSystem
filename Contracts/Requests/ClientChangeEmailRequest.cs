using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangeEmailRequest
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
