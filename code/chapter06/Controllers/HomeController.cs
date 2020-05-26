using chapter06.Models;
using Microsoft.AspNetCore.Mvc;

namespace chapter06.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            //create the model

            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactModel model)
        {
            //save the model here

            return RedirectToAction(nameof(Index));
        }
    }
}
