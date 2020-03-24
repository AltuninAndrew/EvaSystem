using System.Collections.Generic;
using System.Threading.Tasks;
using EvaSystem.Models;

namespace EvaSystem.Services
{
    public interface IIdentityService
    {
        public Task<AuthResultModel> RegisterAsync(string email, string firstName, string lastName,string middleName, string password, string role, string position);

        public Task<AuthResultModel> LoginAsync(string email, string password);

        public Task<ChangedInformationResultModel> ChangePasswordAsync(string username, string oldPassword, string newPassword);

        public Task<List<ResponseUserModel>> GetAllUsersInSystemAsync();


    }
}
