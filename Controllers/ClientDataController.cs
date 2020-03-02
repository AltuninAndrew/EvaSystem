using EvaSystem.Contracts;
using EvaSystem.Contracts.Requests;
using EvaSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Controllers
{
    public class ClientDataController:Controller
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

            if(changeResponse.Success)
            {
                return Ok("Password change is successful");
            }
            else
            {
                return BadRequest(changeResponse.ErrorsMessages);
            }
        }


    }
}
