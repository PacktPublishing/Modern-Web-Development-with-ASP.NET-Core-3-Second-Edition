using chapter06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

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

            if (!this.ModelState.IsValid)
            {
                if (this.ModelState["Email"].Errors.Any())
                {
                    var emailErrors = string.Join(Environment.NewLine, this.ModelState["Email"].Errors.Select(e => e.ErrorMessage));

                    return View();
                }
            }


            return RedirectToAction(nameof(Index));
        }

        public IActionResult SetLocation([BindRequired][FromQuery] int x, [BindRequired][FromQuery] int y)
        {
            return Json(new { X = x, Y = y });
        }

        [HttpPost]
        public IActionResult Process([FromBody] dynamic payload)
        {
            return Json(new { Ok = true });
        }
    }
}
