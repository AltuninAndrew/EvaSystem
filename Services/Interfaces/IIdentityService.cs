using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaSystem.Models;

namespace EvaSystem.Services
{
    public interface IIdentityService
    {
        public Task<AuthResultModel> RegisterAsync(string email, string firstName, string lastName, string password, string role, string position);

        public Task<AuthResultModel> LoginAsync(string email, string password);

        public Task<ChangedInformationResultModel> ChangePasswordAsync(string username, string oldPassword, string newPassword);

        public Task<ChangedInformationResultModel> ChangeEmailAsync(string username, string newEmail,string password);

        public Task<ChangedInformationResultModel> ChangePositionAsync(string username, string newPosition);
    }
}
