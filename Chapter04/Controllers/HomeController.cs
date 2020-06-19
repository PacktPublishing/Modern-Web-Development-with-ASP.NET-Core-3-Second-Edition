using chapter04.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace chapter04.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Invalid()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Process([FromForm] Model model)
        {
            return Ok();
        }
    }
}
