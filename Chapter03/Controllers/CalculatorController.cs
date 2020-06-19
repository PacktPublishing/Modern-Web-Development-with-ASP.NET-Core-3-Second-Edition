using Microsoft.AspNetCore.Mvc;
using System;

namespace chapter03.Controllers
{
    public class CalculatorController : Controller
    {
        //Calculator/CalculateDirectly
        [HttpGet(Name = "CalculateDirectly")]
        public IActionResult Calculate(int a, int b)
        {
            return Json(new { Result = a + b });
        }

        //Calculator/CalculateByKey
        [HttpGet(Name = "CalculateById")]
        public IActionResult Calculate(Guid calculationId)
        {
            return Json(new { Result = calculationId.GetHashCode() });
        }

        //POST Calculator/Calculate
        [HttpPost]
        public IActionResult Calculate([FromBody] Calculation calculation)
        {
            return Json(new { Result = calculation.A + calculation.B });
        }

        [HttpGet("")]
        public IActionResult Default()
        {
            return Json(new { Ok = true });
        }
    }
}
