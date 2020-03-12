﻿using EvaSystem.Data;
using EvaSystem.Models;
using EvaSystem.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EvaSystem.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly DataContext _dataContext;

        public IdentityService(UserManager<UserModel> userManager, JwtSettings jwtSettings,PasswordOptions passOptions,DataContext dataContext)
        { 
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _userManager.Options.Password = passOptions;
            _dataContext = dataContext;
        }

        public async Task<AuthResultModel> RegisterAsync(string email, string firstName, string lastName, string password, string role,string position)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser == null)
            {
                var userName = email.Substring(0,email.LastIndexOf('@'));

                var newUser = new UserModel
                {
                    Email = email,
                    UserName = userName,
                    Role = role,
                    FirstName = firstName,
                    LastName = lastName,
                    Position = position
                    
                };
           
                var createdUser = await _userManager.CreateAsync(newUser,password);

                if(!createdUser.Succeeded)
                {
                    return new AuthResultModel { Success = false, ErrorsMessages = createdUser.Errors.Select(x => x.Description) };
                }

                return GenerateAuthResultForUser(newUser);
              
            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "User with this email addres already exist" } };
            }
        }

        public async Task<AuthResultModel> LoginAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser !=null && await _userManager.CheckPasswordAsync(existingUser,password))
            {
                return GenerateAuthResultForUser(existingUser);
            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "Email/password incorrect" } };
            }
  
        }

        public async Task<List<ResponseUserModel>> GetAllUsersInSystemAsync()
        {
            var result = _userManager.Users.Select(x => new ResponseUserModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Position = x.Position,
                Email = x.Email
            });

            return await result.ToListAsync();
        }

        public async Task<ChangedInformationResultModel> ChangePasswordAsync(string username,string oldPassword, string newPassword)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if(foundUser!=null)
            {
                IdentityResult identityResult = await _userManager.ChangePasswordAsync(foundUser, oldPassword, newPassword);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description)};
            }
            else
            {
                return new ChangedInformationResultModel{Success=false,ErrorsMessages = new[] {"User not found"}};
            }

        }

        public async Task<ChangedInformationResultModel> ChangeEmailAsync(string username, string newEmail,string password)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null && await _userManager.CheckPasswordAsync(foundUser,password))
            {
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
                foundUser.Position = newPosition;
                IdentityResult identityResult = await _userManager.UpdateAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
        }

        public async Task<ChangedInformationResultModel> DeleteUserAsync(string username)
        {
            var foundUser = await _userManager.FindByNameAsync(username);
   

            if (foundUser != null)
            {
                await DeleteUserFromInterectedUsersTableAsync(username);
                IdentityResult identityResult = await _userManager.DeleteAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
        }

        public async Task<ChangedInformationResultModel> AddСommunicationsBtwUsersAsync(string username, string[] interectedUsersName)
        {
            if(await _userManager.FindByNameAsync(username) == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { $"User '{username}' does not exist" } };
            }

            List<InterectedUserModel> models = new List<InterectedUserModel>();
            List<string> errors = new List<string>();

            foreach (var element in interectedUsersName)
            {
                var existUser = await _userManager.FindByNameAsync(element);

                //To do: add cheking element !=null

                if(existUser!=null && element!=username)
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
                    if(existUser == null)
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

        public async Task<List<ResponseUserModel>> GetInterectedUsersAsync(string username)
        {
            if(await _userManager.FindByNameAsync(username) == null)
            {
                return null;
            }

            List<ResponseUserModel> resultUsers = new List<ResponseUserModel>();

            var interectedUsers = _dataContext.interectedUsers.Where(x => x.UserName == username).ToList();

            foreach (var elemet in interectedUsers)
            {
                var userModel = await _userManager.FindByNameAsync(elemet.InterectedUserName);

                if(userModel!=null)
                {
                    resultUsers.Add(new ResponseUserModel { 
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName, 
                        Position = userModel.Position, 
                        Email = userModel.Email });
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
            if(await _userManager.FindByNameAsync(username) == null)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User does not exist" } };
            }

            List<InterectedUserModel> interectedUserModelsForDelete = new List<InterectedUserModel>();

            interectedUserModelsForDelete.AddRange(_dataContext.interectedUsers.Where(x => x.UserName == username));
            interectedUserModelsForDelete.AddRange(_dataContext.interectedUsers.Where(x => x.InterectedUserName == username));

            if (interectedUserModelsForDelete.Count==0)
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "Communications does not exist" } };
            }

            _dataContext.interectedUsers.RemoveRange(interectedUserModelsForDelete);
            await _dataContext.SaveChangesAsync();

            return new ChangedInformationResultModel { Success = true };

        }

        public async Task<ChangedInformationResultModel> DeleteСommunicationAsync(string username,string interectedUserName)
        {
            if((await _userManager.FindByNameAsync(username) == null) || (await _userManager.FindByNameAsync(interectedUserName)==null))
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

        private AuthResultModel GenerateAuthResultForUser(UserModel userModel)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("UserName", userModel.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
                        new Claim("Role", userModel.Role),
                        new Claim("id", userModel.Id),
                    }),
                Expires = DateTime.UtcNow.AddHours(2),

                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature) 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResultModel { Token = tokenHandler.WriteToken(token), Success = true, UserFirstName = userModel.FirstName,
            UserLastName = userModel.LastName, UserPosition = userModel.Position};
        }

       

    }
}
