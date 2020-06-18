using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chapter03.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            this.Response.StatusCode = StatusCodes.Status404NotFound;
            return this.View();
        }


        [Route("error/{statusCode:int}")]
        public IActionResult Error(int statusCode)
        {
            this.Response.StatusCode = statusCode;
            this.ViewBag.StatusCode = statusCode;
            return this.View();
        }

        [HttpGet("{*url}", Order = int.MaxValue)]
        public IActionResult CatchAll()
        {
            this.Response.StatusCode = StatusCodes.Status404NotFound;
            return this.View();
        }
    }
}
