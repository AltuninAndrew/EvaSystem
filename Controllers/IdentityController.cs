using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using EvaSystem.Services;
using EvaSystem.Contracts;
using EvaSystem.Contracts.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace EvaSystem.Controllers
{
    public class IdentityController:Controller
    {
        private readonly IIdentityService _identityService;

        
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.RegisterAdmin)]
        public async Task<IActionResult> RegisterAdmin([FromBody]ClientRegistrationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }


            var authResponse = await _identityService.RegisterAsync(
                request.Email,
                request.FirstName,
                request.LastName,
                request.MiddleName,
                request.Password, 
                "admin",
                request.Position);

            if(!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

            return Ok(authResponse);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Identity.RegisterClient)]
        public async Task<IActionResult> RegisterClient([FromBody]ClientRegistrationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }

            var authResponse = await _identityService.RegisterAsync(
                request.Email,
                request.FirstName,
                request.LastName,
                request.MiddleName,
                request.Password, 
                "user",
                request.Position);


            if (!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

            return Ok(authResponse);
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody]ClientLoginRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

            return Ok(authResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromRoute]string username, [FromBody]ClientChangePasswordRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var changeResponse = await _identityService.ChangePasswordAsync(username, request.OldPassword, request.NewPassword);

            if (changeResponse.Success)
            {
                return Ok("Password change is successful");
            }
            else
            {
                return BadRequest(changeResponse.ErrorsMessages);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Identity.GetAllUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var response = await _identityService.GetAllUsersInSystemAsync();

            if(response == null || response.Count==0)
            {
                return Ok("Database with users is empty");
            }

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Identity.GetUserInfo)]
        public async Task<IActionResult> GetUserInfo([FromRoute]string username)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (userRole == "admin" || userNameFromJwt == username)
            {
                var response = await _identityService.GetUserInfoAsync(username);

                if(response == null)
                {
                    return BadRequest("User not found");
                }

                return Ok(response);

            }
            else
            {
                return Forbid();
            }


        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Identity.CheckJWT)]
        public IActionResult CheckJWT()
        {
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            var response = new { tokenActive = true, userName = userNameFromJwt};

            return Ok(response);
        }

    }
}
