using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace chapter03.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var endpoint = this.HttpContext.GetEndpoint();
            var displayName = endpoint.DisplayName;
            var metadata = endpoint.Metadata.ToArray();
            var metadata1 = endpoint.Metadata.GetMetadata<MyMetadata1>();

            return View();
        }

        public IActionResult Local()
        {
            return Json(new { Ok = true });
        }

        [NonAction]
        public IActionResult Process()
        {
            return View();
        }

        public IActionResult Error()
        {
            return this.StatusCode((int)HttpStatusCode.Unauthorized);
        }

        [Route("Calculate({a:int},{b:int})")]
        public IActionResult Calculate(int a, int b)
        {
            return new ObjectResult(a + b);
        }

        [HttpGet("error/401")]
        public IActionResult Error401()
        {
            return this.View();
        }
    }
}