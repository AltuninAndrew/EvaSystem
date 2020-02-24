using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class UserRegistrationRequest
    {
        public string Loging { get; set; }
        public string Password { get; set; }

    }
}
