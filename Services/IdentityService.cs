using EvaSystem.Data;
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
using System.Text;
using System.Threading.Tasks;

namespace EvaSystem.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly DataContext _dataContext;
        private PositionManager _positionManager;

        public IdentityService(UserManager<UserModel> userManager, JwtSettings jwtSettings,PasswordOptions passOptions,DataContext dataContext)
        { 
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _userManager.Options.Password = passOptions;
            _dataContext = dataContext;
            _positionManager = new PositionManager(dataContext);
        }

        public async Task<AuthResultModel> RegisterAsync(string email, string firstName, string lastName, string middleName, string password, string role,string position)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser!=null)
            {
                 return new AuthResultModel { Success = false, ErrorsMessages = new[] { "User with this email addres already exist" } };
            }

            var userName = email.Substring(0, email.LastIndexOf('@'));

            var newUser = new UserModel
            {
                Email = email,
                UserName = userName,
                Role = role,
                FirstName = firstName,
                LastName = lastName,
                Position = await _positionManager.AddNewPositionAsync(position),
                MiddleName = middleName
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthResultModel { Success = false, ErrorsMessages = createdUser.Errors.Select(x => x.Description) };
            }

            return GenerateAuthResultForUser(newUser);

        }

        public async Task<AuthResultModel> LoginAsync(string email, string password)
        {
            //var existingUser = await _userManager.FindByEmailAsync(email);
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(x => x.Email == email);

            if (existingUser !=null && await _userManager.CheckPasswordAsync(existingUser,password))
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

        public async Task<List<ResponseUserModel>> GetAllUsersInSystemAsync()
        {
            var result = _userManager.Users.Select(x => new ResponseUserModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Position = x.Position.PositionName,
                MiddleName = x.MiddleName,
                Email = x.Email
            });

            return await result.ToListAsync();
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

            return new AuthResultModel
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                UserFirstName = userModel.FirstName,
                UserLastName = userModel.LastName,
                UserMiddleName = userModel.MiddleName,
                UserPosition = _positionManager.GetPositionByIDAsync(userModel.PositionId).Result.PositionName
            };
        }

    }
}
