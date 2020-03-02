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

        public Task<AuthResultModel> LoginAsync(string email, string password);

        public Task<ChangedPasswordResultModel> ChangePasswordAsync(string username, string oldPassword, string newPassword);
    }
}
