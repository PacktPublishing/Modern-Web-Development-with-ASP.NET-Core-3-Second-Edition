using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter18.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IOpenWeatherMap _openWeatherMap;

        public WeatherController(IOpenWeatherMap openWeatherMap)
        {
            this._openWeatherMap = openWeatherMap;
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> GetByCity(int id)
        {
            var data = await this._openWeatherMap.GetByCity(id);

            return Json(data);
        }
    }
}
