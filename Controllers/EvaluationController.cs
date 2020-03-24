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

        [HttpPost(ApiRoutes.Evaluation.AddCriterions)]
        public async Task<IActionResult> AddCriterions(string positionName, [FromBody]CriterionModel[] request)
        {
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
    }
}
