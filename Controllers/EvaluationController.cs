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

    }
}
