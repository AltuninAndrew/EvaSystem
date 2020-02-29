using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaSystem.Services;
using EvaSystem.Contracts;
using EvaSystem.Contracts.Requests;

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
        public async Task<IActionResult> RegisterAdmin([FromBody]UserRegistrationRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }

            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password, "admin");

            if(authResponse.Success)
            {
                return Ok(authResponse.Token);
            }
            else
            {
                return BadRequest(authResponse.ErrorsMessages);
            }

           
        }

        [HttpPost(ApiRoutes.Identity.RegisterClient)]
        public async Task<IActionResult> RegisterClient([FromBody]UserRegistrationRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)));
            }

            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password, "user");

            if (authResponse.Success)
            {
                return Ok(authResponse.Token);
            }
            else
            {
                return BadRequest(authResponse.ErrorsMessages);
            }
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (authResponse.Success)
            {
                return Ok(authResponse.Token);
            }
            else
            {
                return BadRequest(authResponse.ErrorsMessages);
            }
        }

    }
}
