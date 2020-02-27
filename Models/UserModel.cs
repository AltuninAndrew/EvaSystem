using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EvaSystem.Models
{
    public class UserModel:IdentityUser
    {
        public string Role { get; set; }

    }
}
