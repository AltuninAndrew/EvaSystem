using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangePositionRequest
    {
        [MinLength(2, ErrorMessage = "Position length should be more then 1 chars")]
        public string NewPosition { get; set; }
    }
}
