using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }

            
            var authResponse = await _identityService.RegisterAsync(request.Email,request.FirstName,request.LastName, request.Password, "admin",
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
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }

            var authResponse = await _identityService.RegisterAsync(request.Email,request.FirstName,request.LastName,request.Password, "user",
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
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

            return Ok(authResponse);
        }

        [HttpGet(ApiRoutes.Identity.GettAllUsers)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _identityService.GetAllUsersInSystemAsync();

            if(response == null || response.Count==0)
            {
                return Ok("Database with users is empty");
            }

            return Ok(response);
        }

    }
}
