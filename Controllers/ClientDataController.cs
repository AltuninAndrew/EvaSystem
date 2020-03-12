using EvaSystem.Contracts;
using EvaSystem.Contracts.Requests;
using EvaSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Controllers
{
    public class ClientDataController : Controller
    {

        private readonly IIdentityService _identityService;

        public ClientDataController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromRoute]string username, [FromBody]ClientChangePasswordRequest request)
        {
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
        [HttpPut(ApiRoutes.ClientData.ChangeEmail)]
        public async Task<IActionResult> ChangeEmail([FromRoute]string username, [FromBody]ClientChangeEmailRequest request)
        {
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (username != userNameFromJwt)
            {
                return Forbid();
            }

            var changeResponse = await _identityService.ChangeEmailAsync(username, request.NewEmail, request.Password);

            if (changeResponse.Success)
            {
                return Ok("Email chage is successful");
            }
            else
            {
                return BadRequest(changeResponse.ErrorsMessages);
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangePosition)]
        public async Task<IActionResult> ChangePosition([FromRoute]string username, string newPosition)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (newPosition.Length < 2)
            {
                return BadRequest("Length should be more then 1 chars");
            }

            if (userRole != "admin")
            {
                return Forbid();
            }

            var changeResponse = await _identityService.ChangePositionAsync(username, newPosition);

            if (changeResponse.Success)
            {
                return Ok("Role chage is successful");
            }
            else
            {
                return BadRequest(changeResponse.ErrorsMessages);
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.ClientData.DeleteUser)]
        public async Task<IActionResult> DeleteUser([FromRoute]string username)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var changeResponse = await _identityService.DeleteUserAsync(username);

            if(!changeResponse.Success)
            {
                return BadRequest(changeResponse.ErrorsMessages);
            }

            return Ok("User was successfully deleted");


        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.ClientData.AddCommunicationBtwUsers)]
        public async Task<IActionResult> AddCommunicationsBtwUsers([FromRoute] string username, string[] interectedUsersName)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            if(interectedUsersName.Length==0)
            {
                return BadRequest("Interected users name cannot be null");
            }

            var response = await _identityService.AddСommunicationsBtwUsersAsync(username, interectedUsersName);

            if (response.Success == false)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok(response);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.ClientData.GetInterectedUsers)]
        public async Task<IActionResult> GetInterectedUsers([FromRoute] string username)
        {
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (username == userNameFromJwt || userRole == "admin")
            {
                var response = await _identityService.GetInterectedUsersAsync(username);

                if (response == null)
                {
                    return BadRequest("User not found or user have not interected users");
                }

                return Ok(response);

            }
            else
            {
                return Forbid();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.ClientData.DeleteСommunicationBtwUsers)]
        public async Task<IActionResult> DeleteСommunicationBtwUsers([FromRoute] string username, string interectedUserName)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var response = await _identityService.DeleteСommunicationAsync(username, interectedUserName);

            if(!response.Success)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok("User communication was successfully deleted");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.ClientData.DeleteUserFromInterectedUsersTable)]
        public async Task<IActionResult> DeleteUserFromInterectedUsersTable([FromRoute] string username)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var response = await _identityService.DeleteUserFromInterectedUsersTableAsync(username);

            if (!response.Success)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok("User was successfully deleted from interected users table");

        }
    }
}
