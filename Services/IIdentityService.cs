using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaSystem.Models;

namespace EvaSystem.Services
{
    public interface IIdentityService
    {
        public Task<AuthResultModel> RegisterAsync(string email, string password, string role);
    }
}
