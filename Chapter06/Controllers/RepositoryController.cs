using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chapter06.Controllers
{
    public class RepositoryController : Controller
    {
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmailExists(string email)
        {
            //if (this._repository.CheckEmailExists(email))
            //{
            //    return this.Json(false);
            //}

            return Json(true);
        }

        [HttpPost("[controller]/[action]")]
        public IActionResult SaveForm(IFormFile file)
        {
            var length = file.Length;
            var name = file.Name;
            //do something with the file
            return View();
        }

        [AjaxOnly]
        public IActionResult AjaxOnly()
        {
            return Json(true);
        }

        public IActionResult Save()
        {
            return View();
        }
    }
}
