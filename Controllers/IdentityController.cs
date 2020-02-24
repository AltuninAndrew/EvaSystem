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
        
        IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;

        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest request)
        {

            return Ok();
        }

        
    }
}
