using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace chapter12.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            this._logger = logger;
            this._httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = this._httpClientFactory.CreateClient("chapter12");

           await client.GetAsync("/Home/Telemetry");

            LogElapsedUrlEventSource.Instance.LogElapsed("http://google.com", 0.1F);

            return View();
        }

        public IActionResult Telemetry()
        {
            var id = this.HttpContext.TraceIdentifier;
            return Ok();
        }

        [Logger("Method called with {email}", LogLevel = LogLevel.Information)]
        public IActionResult AddToMailingList(string email)
        {
            return Ok();
        }
    }
}
