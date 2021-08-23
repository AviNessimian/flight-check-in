using Britannica.Host.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Britannica.Host.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(AppValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(AppNotFoundProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BusinessRuleProblemDetails), StatusCodes.Status400BadRequest)]
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult Json(object response) => new JsonResult(response);
    }
}
