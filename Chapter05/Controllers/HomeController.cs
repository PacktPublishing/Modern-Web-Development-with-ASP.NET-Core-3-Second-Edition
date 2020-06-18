using Microsoft.AspNetCore.Mvc;

namespace chapter05.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyPage()
        {
            return View();
        }
    }
}
