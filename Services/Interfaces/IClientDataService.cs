using EvaSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvaSystem.Services.Interfaces
{
    public interface IClientDataService
    {
        public Task<ChangedInformationResultModel> ChangeEmailAsync(string username, string newEmail);

        public Task<ChangedInformationResultModel> ChangePositionAsync(string username, string newPosition);

        public Task<ChangedInformationResultModel> ChangeFirstNameAsync(string username, string newFirstName);

        public Task<ChangedInformationResultModel> ChangeLastNameAsync(string username, string newFirstName);

        public Task<ChangedInformationResultModel> ChangeMiddleNameAsync(string username, string newFirstName);

        public Task<ChangedInformationResultModel> DeleteUserAsync(string username);

        public Task<ChangedInformationResultModel> AddСommunicationsBtwUsersAsync(string username, string[] interectedUsersName);

        public Task<List<ResponseUserModel>> GetInterectedUsersAsync(string username);

        public Task<ChangedInformationResultModel> DeleteUserFromInterectedUsersTableAsync(string username);

        public Task<ChangedInformationResultModel> DeleteСommunicationAsync(string username, string interectedUserName);
    }
}
