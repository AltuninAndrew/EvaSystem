using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaSystem.Models;

namespace EvaSystem.Services
{
    public interface IIdentityService
    {
        public Task<AuthResultModel> RegisterAsync(string email, string firstName, string lastName,string middleName, string password, string role, string position);

        public Task<AuthResultModel> LoginAsync(string email, string password);

        public Task<List<ResponseUserModel>> GetAllUsersInSystemAsync();

        public Task<ChangedInformationResultModel> ChangePasswordAsync(string username, string oldPassword, string newPassword);

        public Task<ChangedInformationResultModel> ChangeEmailAsync(string username, string newEmail,string password);

        public Task<ChangedInformationResultModel> ChangePositionAsync(string username, string newPosition);

        public Task<ChangedInformationResultModel> DeleteUserAsync(string username);

        public Task<ChangedInformationResultModel> AddСommunicationsBtwUsersAsync(string username, string[] interectedUsersName);

        public Task<List<ResponseUserModel>> GetInterectedUsersAsync(string username);

        public Task<ChangedInformationResultModel> DeleteUserFromInterectedUsersTableAsync(string username);

        public Task<ChangedInformationResultModel> DeleteСommunicationAsync(string username, string interectedUserName);

    }
}
