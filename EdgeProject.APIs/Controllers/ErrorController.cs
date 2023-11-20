using EdgeProject.APIs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeProject.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {

        public ActionResult Error(int code) 
        {
            return NotFound(new ApiErrorResponse(code));
        }

    }
}
