using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace chapter03.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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