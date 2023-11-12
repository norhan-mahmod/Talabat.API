using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;

namespace Talabat.API.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // because we will not request the endpoint using controller
                                            // the program will make redirect to it
    public class ErrorController : ControllerBase
    {
        // this to handle NotFound Endpoint (when request endpoint that doesn't exist)
        public ActionResult Error(int code)
        {
            return NotFound(new ApiErrorResponse(code));
        }
    }
}
