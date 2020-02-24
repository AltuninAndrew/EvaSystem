using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvaSystem.Controllers
{
    public class TestController:Controller
    {
        [HttpGet(Contracts.ApiRoutes.Test.Value)]
        public IActionResult GetTestValue()
        {
            var result = new
            {
                message = "ok",
                status = 1,
            };
            return Ok(result); 
        }

    }
}
