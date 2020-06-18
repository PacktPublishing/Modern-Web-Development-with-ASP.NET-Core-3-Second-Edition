using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreProfiler;
using Microsoft.AspNetCore.Mvc;

namespace chapter15.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (ProfilingSession.Current.Step("/Home"))
            {
                using (ProfilingSession.Current.Step(() => "Return View"))
                {
                    return View();
                }
            }
            
        }
    }
}
