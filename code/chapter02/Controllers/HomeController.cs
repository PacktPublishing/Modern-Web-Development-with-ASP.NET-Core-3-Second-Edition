using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace chapter02.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoggingSettings _settings;

        public HomeController(IOptionsSnapshot<LoggingSettings> settings)
        {
            _settings = settings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}