using EvaSystem.Contracts;
using EvaSystem.Contracts.Requests;
using EvaSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Controllers
{
    public class ClientDataController : Controller
    {

        private readonly IClientDataService _clientDataService;
        private const float MaxImageWeghtKB = 1024;

        public ClientDataController(IClientDataService clientDataService)
        {
            _clientDataService = clientDataService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangeEmail)]
        public async Task<IActionResult> ChangeEmail([FromRoute]string username, [FromBody]ClientChangeEmailRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (userNameFromJwt == username || userRole == "admin")
            {
                var changeResponse = await _clientDataService.ChangeEmailAsync(username, request.NewEmail);

                if (!changeResponse.Success)
                {
                    return BadRequest(changeResponse.ErrorsMessages);
                }

                return Ok("Email chage is successful");


            }
            else
            {
                return Forbid();
            }


        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangePosition)]
        public async Task<IActionResult> ChangePosition([FromRoute]string username, string newPosition)
        {
            if (string.IsNullOrEmpty(newPosition))
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (newPosition.Length < 2)
            {
                return BadRequest("Length should be more then 1 chars");
            }

            if (userRole != "admin")
            {
                return Forbid();
            }

            var changeResponse = await _clientDataService.ChangePositionAsync(username, newPosition);

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
        [HttpPut(ApiRoutes.ClientData.ChangeFirstName)]
        public async Task<IActionResult> ChangeFirstName([FromRoute]string username, string newFirstName)
        {
            if (string.IsNullOrEmpty(newFirstName))
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (newFirstName.Length < 2)
            {
                return BadRequest("Length should be more then 1 chars");
            }

            if (userRole == "admin" || userNameFromJwt == username)
            {
                var changeResponse = await _clientDataService.ChangeFirstNameAsync(username, newFirstName);

                if (!changeResponse.Success)
                {
                    return BadRequest(changeResponse.ErrorsMessages);
                }

                return Ok("First name was successfully change");

            }
            else
            {
                return Forbid();
            }

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangeMiddleName)]
        public async Task<IActionResult> ChangeMiddleName([FromRoute]string username, string newMiddleName)
        {
            if (string.IsNullOrEmpty(newMiddleName))
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (newMiddleName.Length < 2)
            {
                return BadRequest("Length should be more then 1 chars");
            }

            if (userRole == "admin" || userNameFromJwt == username)
            {
                var changeResponse = await _clientDataService.ChangeMiddleNameAsync(username, newMiddleName);

                if (!changeResponse.Success)
                {
                    return BadRequest(changeResponse.ErrorsMessages);
                }

                return Ok("Middle name was successfully change");

            }
            else
            {
                return Forbid();
            }

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(ApiRoutes.ClientData.ChangeLastName)]
        public async Task<IActionResult> ChangeLastName([FromRoute]string username, string newLastName)
        {
            if (string.IsNullOrEmpty(newLastName))
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (newLastName.Length < 2)
            {
                return BadRequest("Length should be more then 1 chars");
            }

            if (userRole == "admin" || userNameFromJwt == username)
            {
                var changeResponse = await _clientDataService.ChangeLastNameAsync(username, newLastName);

                if (!changeResponse.Success)
                {
                    return BadRequest(changeResponse.ErrorsMessages);
                }

                return Ok("Last name was successfully change");

            }
            else
            {
                return Forbid();
            }

        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.ClientData.AddUserAvatar)]
        public async Task<IActionResult> AddUserAvatar([FromRoute]string username, IFormFile userAvatarImage)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if(userRole == "admin" || userNameFromJwt==username)
            {
                if (userAvatarImage == null)
                {
                    return BadRequest("File is null");
                }

                if (CheckImage(userAvatarImage) == false)
                {
                    return BadRequest("Image does not meet the conditions");
                }


                byte[] avatar = null;
                using (var fs = userAvatarImage.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    avatar = ms.ToArray();
                }

                var response = await _clientDataService.AddAvatarToUserAsync(username, avatar);

                if(response.Success == false)
                {
                    return BadRequest(response.ErrorsMessages);
                }

                return Ok(response);

            }
            else
            {
                return Forbid();
            }


        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.ClientData.RemoveUserAvatar)]
        public async Task<IActionResult> RemoveUserAvatar([FromRoute]string username)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();
            var userNameFromJwt = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserName").Value.ToString();

            if (userRole == "admin" || userNameFromJwt == username)
            {
                var response = await _clientDataService.RemoveUserAvatarAsync(username);

                if (response.Success == false)
                {
                    return BadRequest(response.ErrorsMessages);
                }

                return Ok(response);
            }
            else
            {
                return Forbid();
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

            var changeResponse = await _clientDataService.DeleteUserAsync(username);

            if(!changeResponse.Success)
            {
                return BadRequest(changeResponse.ErrorsMessages);
            }

            return Ok("User was successfully deleted");


        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.ClientData.GetUsersForInteract)]
        public async Task<IActionResult> GetUsersForInteract([FromRoute] string username)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var response = await _clientDataService.GetUsersForInteractAsync(username);

            if(response == null)
            {
                return BadRequest("User not found");
            }

            if (response.Count() > 0)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Users for interact not found");
            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.ClientData.AddCommunicationBtwUsers)]
        public async Task<IActionResult> AddCommunicationsBtwUsers([FromRoute] string username, string[] interectedUsersName)
        {
            if (interectedUsersName == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            if(interectedUsersName.Length==0)
            {
                return BadRequest("Interected users name cannot be null");
            }

            var response = await _clientDataService.AddСommunicationsBtwUsersAsync(username, interectedUsersName);

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
                var response = await _clientDataService.GetInteractedUsersAsync(username);

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
            if (interectedUserName == null)
            {
                return BadRequest("Request model is not correct");
            }

            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var response = await _clientDataService.DeleteСommunicationAsync(username, interectedUserName);

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

            var response = await _clientDataService.DeleteUserFromInterectedUsersTableAsync(username);

            if (!response.Success)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok("User was successfully deleted from interected users table");

        }


        private bool CheckImage(IFormFile image)
        {
            if (((image.Length / 1024) <= MaxImageWeghtKB) && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
