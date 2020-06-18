using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace chapter10.Controllers
{
    //[LogAction]
    [ServiceFilter(typeof(LogFilter), Order = 1)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Throw()
        {
            throw new Exception("Just to test the error filter!");
        }
    }
}
