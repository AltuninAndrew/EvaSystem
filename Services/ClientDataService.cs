using EvaSystem.Data;
using EvaSystem.Models;
using EvaSystem.Services.AuxiliaryHandlers;
using EvaSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Services
{
    public class ClientDataService : IClientDataService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly DataContext _dataContext;
        private PositionManager _positionManager;

        private const float MaxAvatarImageWeightKB = 1024;

        public ClientDataService(UserManager<UserModel> userManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _positionManager = new PositionManager(dataContext);
        }

        public async Task<ChangedInformationResultModel> ChangeEmailAsync(string username, string newEmail)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null)
            {
                if(newEmail == foundUser.Email)
                {
                    return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "New address is no different from the old one" } };
                }

                foundUser.Email = newEmail;
                foundUser.UserName = newEmail.Substring(0, newEmail.LastIndexOf('@'));
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found or password incorrect" } };
            }
        }

        public async Task<ChangedInformationResultModel> ChangePositionAsync(string username, string newPosition)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null)
            {
                foundUser.Position = await _positionManager.AddNewPositionAsync(newPosition);
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
        }

        public async Task<ChangedInformationResultModel> ChangeFirstNameAsync(string username, string newName)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null)
            {
                foundUser.FirstName = newName;
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }

        }

        public async Task<ChangedInformationResultModel> ChangeLastNameAsync(string username, string newLastName)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null)
            {
                foundUser.LastName = newLastName;
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
        }

        public async Task<ChangedInformationResultModel> ChangeMiddleNameAsync(string username, string newMiddleName)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null)
            {
                foundUser.MiddleName = newMiddleName;
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
        }

        public async Task<ChangedInformationResultModel> AddAvatarToUserAsync(string username, byte[] image)
        {
            if((image.Length/1024)>MaxAvatarImageWeightKB)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "Image is too large" } };
            }

            var foundUser = await _userManager.FindByNameAsync(username);

            if(foundUser == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }

            foundUser.AvatarImage = image;

            IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
            return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
        }

        public async Task<ChangedInformationResultModel> RemoveUserAvatarAsync(string username)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }

            if(foundUser.AvatarImage !=null)
            {
                foundUser.AvatarImage = null;
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "The user's avatar was not found" } };
            }

        }

        public async Task<ChangedInformationResultModel> DeleteUserAsync(string username)
        {
            var foundUser = await _userManager.FindByNameAsync(username);


            if (foundUser != null)
            {
                await DeleteUserFromInterectedUsersTableAsync(username);
                IdentityResult identityResult = await _userManager.DeleteAsync(foundUser);
                var deletRes = await _positionManager.DeletePositionAsync(foundUser.PositionId);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
        }

        public async Task<IEnumerable<ResponseUserModel>> GetUsersForInteractAsync(string username)
        {
            if (await _userManager.FindByNameAsync(username) == null)
            {
                return null;
            }


            var interactUsers = await GetInteractedUsersAsync(username);


            var allUsers = await _userManager.Users.Where(x=>x.UserName!=username).Select(x=> new ResponseUserModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Position = x.Position.PositionName,
                MiddleName = x.MiddleName,
                Email = x.Email,
                AvatarImage = x.AvatarImage,
                UserRole = x.Role,
                Username = x.UserName,

            }).ToListAsync();


            if (interactUsers != null)
            {
                var result = allUsers.Except(interactUsers, new UsersComparer());
                return result;
            }
            else
            {
                return allUsers;
            }
           
        }

        public async Task<ChangedInformationResultModel> AddСommunicationsBtwUsersAsync(string username, string[] interectedUsersName)
        {
            if (await _userManager.FindByNameAsync(username) == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { $"User '{username}' does not exist" } };
            }

            List<InterectedUserModel> models = new List<InterectedUserModel>();
            List<string> errors = new List<string>();

            foreach (var element in interectedUsersName)
            {
                var existUser = await _userManager.FindByNameAsync(element);

                //To do: add cheking element !=null

                if (existUser != null && element != username)
                {


                    var pk1 = element + username; //костыль!
                    var pk2 = username + element; //костыль!

                    var isContain = await _dataContext.interectedUsers.SingleOrDefaultAsync(x => x.EntryHash == pk1);

                    if (isContain == null)
                    {
                        var newCommunication1 = new InterectedUserModel { UserName = username, InterectedUserName = element, EntryHash = pk1 };
                        var newCommunication2 = new InterectedUserModel { UserName = element, InterectedUserName = username, EntryHash = pk2 };
                        models.Add(newCommunication1);
                        models.Add(newCommunication2);
                    }
                    else
                    {
                        errors.Add($"Pair {username}:{element} / {element}:{username} already exist");
                    }

                }

                else
                {
                    if (existUser == null)
                    {
                        errors.Add($"User '{element}' does not exist");
                    }
                    else
                    {
                        errors.Add($"The specified user names {username}:{element} match");
                    }
                }

            }

            if (models.Count > 0)
            {
                await _dataContext.interectedUsers.AddRangeAsync(models);
                await _dataContext.SaveChangesAsync();
            }

            if (models.Count == 0)
            {
                return new ChangedInformationResultModel { ErrorsMessages = errors, Success = false };
            }

            if (models.Count > 0 && models.Count < interectedUsersName.Length)
            {
                return new ChangedInformationResultModel { ErrorsMessages = errors, Success = true };
            }

            return new ChangedInformationResultModel { Success = true };

        }

        public async Task<List<ResponseUserModel>> GetInteractedUsersAsync(string username)
        {
            if (await _userManager.FindByNameAsync(username) == null)
            {
                return null;
            }

            List<ResponseUserModel> resultUsers = new List<ResponseUserModel>();

            var interectedUsers = _dataContext.interectedUsers.Where(x => x.UserName == username).ToList();

            foreach (var elemet in interectedUsers)
            {
                var userModel = await _userManager.FindByNameAsync(elemet.InterectedUserName);

                if (userModel != null)
                {
                    resultUsers.Add(new ResponseUserModel
                    {
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        MiddleName = userModel.MiddleName,
                        Position = _positionManager.GetPositionByIDAsync(userModel.PositionId).Result.PositionName,
                        Email = userModel.Email,
                        AvatarImage = userModel.AvatarImage,
                        UserRole = userModel.Role,
                        Username = userModel.UserName,
                    });
                }
            }

            if (resultUsers.Count > 0)
            {
                return resultUsers;
            }
            else
            {
                return null;
            }
        }

        public async Task<ChangedInformationResultModel> DeleteUserFromInterectedUsersTableAsync(string username)
        {
            if (await _userManager.FindByNameAsync(username) == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User does not exist" } };
            }

            List<InterectedUserModel> interectedUserModelsForDelete = new List<InterectedUserModel>();

            interectedUserModelsForDelete.AddRange(_dataContext.interectedUsers.Where(x => x.UserName == username));
            interectedUserModelsForDelete.AddRange(_dataContext.interectedUsers.Where(x => x.InterectedUserName == username));

            if (interectedUserModelsForDelete.Count == 0)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "Communications does not exist" } };
            }

            _dataContext.interectedUsers.RemoveRange(interectedUserModelsForDelete);
            await _dataContext.SaveChangesAsync();

            return new ChangedInformationResultModel { Success = true };

        }

        public async Task<ChangedInformationResultModel> DeleteСommunicationAsync(string username, string interectedUserName)
        {
            if ((await _userManager.FindByNameAsync(username) == null) || (await _userManager.FindByNameAsync(interectedUserName) == null))
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User does not exist" } };
            }

            var pk1 = username + interectedUserName; //костыль!
            var pk2 = interectedUserName + username; //костыль!


            var isContain = await _dataContext.interectedUsers.AsNoTracking().SingleOrDefaultAsync(x => x.EntryHash == pk1);

            if (isContain != null)
            {
                var model1 = new InterectedUserModel { UserName = username, InterectedUserName = interectedUserName, EntryHash = pk1 };
                var model2 = new InterectedUserModel { UserName = interectedUserName, InterectedUserName = username, EntryHash = pk2 };
                _dataContext.interectedUsers.Remove(model1);
                _dataContext.interectedUsers.Remove(model2);

            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "Сommunication does not exist" } };
            }

            await _dataContext.SaveChangesAsync();
            return new ChangedInformationResultModel { Success = true };

        }




    }
}
