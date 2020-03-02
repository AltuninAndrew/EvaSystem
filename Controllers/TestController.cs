using EvaSystem.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EvaSystem.Controllers
{
    public class TestController:Controller
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Roles = "admin")]
        [HttpGet(Contracts.ApiRoutes.Test.Value)]
        public IActionResult GetTestValue()
        {
            //var indents = HttpContext.User.Identities;

            var clientRole = HttpContext.User.Claims.Where(x => x.Type == "Role").Select(x => x.Value);

            var result = new
            {
                message = "ok",
                status = 1,
                role = clientRole
            };
            return Ok(result); 
        }

    }
}
