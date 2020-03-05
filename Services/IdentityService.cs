using EvaSystem.Models;
using EvaSystem.Options;
using Microsoft.AspNetCore.Identity;
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

        public IdentityService(UserManager<UserModel> userMaganer, JwtSettings jwtSettings,PasswordOptions passOptions)
        {
            _userManager = userMaganer;
            _jwtSettings = jwtSettings;
            _userManager.Options.Password = passOptions;

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
            
            if(existingUser !=null && await _userManager.CheckPasswordAsync(existingUser,password))
            {
                return GenerateAuthResultForUser(existingUser);
            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "Email/password incorrect" } };
            }
  
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

        public async Task<ChangedInformationResultModel> DeleteUser(string username)
        {
            var foundUser = await _userManager.FindByNameAsync(username);

            if (foundUser != null)
            {       
                IdentityResult identityResult = await _userManager.DeleteAsync(foundUser);
                return new ChangedInformationResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description) };
            }
            else
            {
                return new ChangedInformationResultModel { Success = false, ErrorsMessages = new[] { "User not found" } };
            }
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

            return new AuthResultModel { Token = tokenHandler.WriteToken(token), Success = true };
        }
    }
}
