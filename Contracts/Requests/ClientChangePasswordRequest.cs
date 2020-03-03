﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Contracts.Requests
{
    public class ClientChangePasswordRequest
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
