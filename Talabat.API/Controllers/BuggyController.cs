using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.Repository.Data;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext context)
        {
            this.context = context;
        }
        // Types of Errors that should be handled

        // 1. not found
        [HttpGet("notfound")] // api/Buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = context.Products.Find(100);
            if (product is null)
                return NotFound(new ApiErrorResponse(404));
            return Ok(product);
        }
        // 2. server error (Throw Exception)
        [HttpGet("servererror")] // api/Buggy/servererror
        public ActionResult GetServerError()
        {
            var product = context.Products.Find(100);
            var producttoreturn = product.ToString(); // (server error) throw exception because product is null
            return Ok(producttoreturn);
        }
        // 3. Bad Request
        [HttpGet("badrequest")] // api/Buggy/badrequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiErrorResponse(400));
        }
        // 4. Validation error which is part of Bad Request
        [HttpGet("badrequest/{id}")] // api/Buggy/badrequest/five
        public ActionResult GetBadRequest(int id) // validation error because endpoint supposed to recieve int
                                                  // and we send string so model is not valid
        {
            return Ok();
        }
        // 5. we ask endpoint doesn't exist
    }
}
