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
using System.Text;
using System.Threading.Tasks;

namespace EvaSystem.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserModel> _userMaganer;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<UserModel> userMaganer, JwtSettings jwtSettings)
        {
            _userMaganer = userMaganer;
            _jwtSettings = jwtSettings;
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

                var rsa = new RSACryptoServiceProvider(2048);
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, newUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                        new Claim("id", newUser.Id),
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),

                    SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa),SecurityAlgorithms.RsaSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new AuthResultModel { Token = tokenHandler.WriteToken(token), Success = true };

            }
            else
            {
                return new AuthResultModel { Success = false, ErrorsMessages = new[] { "User with this email addres already exist" } };
            }
        }
    }
}
