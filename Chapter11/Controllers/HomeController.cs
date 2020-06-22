using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using chapter07.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chapter07.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthorizationService _authSvc;

        public HomeController(ILogger<HomeController> logger, IAuthorizationService  authSvc)
        {
            this._logger = logger;
            this._authSvc = authSvc;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Private()
        {
            var result = await this._authSvc.AuthorizeAsync(this.User, new DayOfWeekRequirement(DayOfWeek.Friday), DayOfWeekRequirement.Name);

            if (result.Succeeded)
            {
                //it's a friday
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
