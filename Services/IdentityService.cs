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
        private readonly UserManager<UserModel> _userMaganer;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<UserModel> userMaganer, JwtSettings jwtSettings,PasswordOptions passOptions)
        {
            _userMaganer = userMaganer;
            _jwtSettings = jwtSettings;

            _userMaganer.Options.Password = passOptions;

        }

        public async Task<AuthResultModel> RegisterAsync(string email, string password, string role)
        {
            var existingUser = await _userMaganer.FindByEmailAsync(email);

            if(existingUser == null)
            {
                var userName = email.Substring(0,email.LastIndexOf('@'));

                var newUser = new UserModel
                {
                    Email = email,
                    UserName = userName,
                    Role = role
                };

                var createdUser = await _userMaganer.CreateAsync(newUser,password);

                

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
            var existingUser = await _userMaganer.FindByEmailAsync(email);
            
            if(existingUser !=null && await _userMaganer.CheckPasswordAsync(existingUser,password))
            {
                return GenerateAuthResultForUser(existingUser);
            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "Email/password incorrect" } };
            }
  
        }

        public async Task<ChangedPasswordResultModel> ChangePasswordAsync(string username,string oldPassword, string newPassword)
        {
            var foundUser = await _userMaganer.FindByNameAsync(username);

            if(foundUser!=null)
            {
                IdentityResult identityResult = await _userMaganer.ChangePasswordAsync(foundUser, oldPassword, newPassword);
                return new ChangedPasswordResultModel { Success = identityResult.Succeeded, ErrorsMessages = identityResult.Errors.Select(x => x.Description)};
            }
            else
            {
                return new ChangedPasswordResultModel{Success=false,ErrorsMessages = new[] {"User not found"}};
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
                        new Claim(JwtRegisteredClaimNames.Sub, userModel.UserName),
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
