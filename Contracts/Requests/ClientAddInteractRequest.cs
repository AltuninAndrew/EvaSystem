using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class ClientAddInteractRequest
    {
        public string[] interactedUsersName { get; set; }
    }
}
