using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using EvaSystem.Services;
using EvaSystem.Contracts;
using EvaSystem.Contracts.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using EvaSystem.Services.Interfaces;
using EvaSystem.Models;

namespace EvaSystem.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Evaluation.AddCriterions)]
        public async Task<IActionResult> AddCriterions(string positionName, [FromBody]CriterionModel[] request)
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }


            if (request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _evaluationService.AddCriterionsAsync(positionName, request);

            if(string.IsNullOrEmpty(positionName))
            {
                return BadRequest("Name of position is empty");
            }

            if (response.Success == false)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok(response);
        }

        [HttpGet(ApiRoutes.Evaluation.GetCriterions)]
        public async Task<IActionResult> GetCriterions([FromRoute]string positionName)
        {
            var response = await _evaluationService.GetCriterionsAsync(positionName);

            if(response == null)
            {
                return BadRequest("Position not found");
            }

            if (response.Count == 0)
            {
                return BadRequest("Position has not criterions");
            }

            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Evaluation.GetAllPositions)]
        public async Task<IActionResult> GetPositions()
        {
            var userRole = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role").Value.ToString();

            if (userRole != "admin")
            {
                return Forbid();
            }

            var response = await _evaluationService.GetAllPositions();

            if (response == null || response.Count() == 0)
            {
                return Ok("Database with positions is empty");
            }

            return Ok(response);

        }

        [HttpGet(ApiRoutes.Evaluation.GetCriterionsForUser)]
        public async Task<IActionResult> GetCriterionsForUser([FromRoute]string username)
        {
            var response = await _evaluationService.GetCriterionsForUserAsync(username);

            if (response == null)
            {
                return BadRequest("User not found");
            }

            if(response.Count == 0)
            {
                return BadRequest("User has not criterions");
            }

            return Ok(response);
        }

        [HttpDelete(ApiRoutes.Evaluation.DeleteCriterionsForPos)]
        public async Task<IActionResult> DeleteCriterionsForPos([FromRoute]string positionName, string[] criterionNames)
        {
            if (criterionNames == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _evaluationService.DeleteCriterionsToPosition(positionName, criterionNames);

            if(response.Success == false)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok(response);

        }

        [HttpPost(ApiRoutes.Evaluation.RateUser)]
        public async Task<IActionResult> RateUser([FromRoute]string username, [FromBody]ScorePerCriterionModel[] request)
        {
            if(request == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _evaluationService.RateUserAsync(username, request);

            if(response.Success == false)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok(response);
        }

        [HttpGet(ApiRoutes.Evaluation.GetUserRating)]
        public async Task<IActionResult> GetUserRating([FromRoute]string username)
        {
            var response = await _evaluationService.GetUserRatingAsync(username);

            if(response == null)
            {
                return BadRequest("Rating is empty or user not found");
            }

            return Ok(response);  
        }

        [HttpDelete(ApiRoutes.Evaluation.RemoveUserRating)]
        public async Task<IActionResult> RemoveUserRating([FromRoute]string username, string[] criterionNames)
        {
            if (criterionNames == null)
            {
                return BadRequest("Request model is not correct");
            }

            var response = await _evaluationService.RemoveUserRatingAsync(username, criterionNames);

            if(response.Success == false)
            {
                return BadRequest(response.ErrorsMessages);
            }

            return Ok(response);

        }
    }
}
