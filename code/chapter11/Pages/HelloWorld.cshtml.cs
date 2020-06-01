using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using System.Threading.Tasks;

namespace chapter11.Pages
{
    public class HelloWorldModel : PageModel
    {
        public HelloWorldModel(IMyService svc)
        {
        }

        public string Message { get; set; }

        public void OnGet()
        {
            if (this.HttpContext.Request.Headers["HTTP-Referer"].SingleOrDefault()?.Contains("google.com") == true)
            {
                //hey, someone found us through Google!
            }
            
            this.Message = "Hello, World!";
        }

        public async Task<IActionResult> OnPostOneAsync()
        {
            return RedirectToPage("One");
        }

        public IActionResult OnPostTwo()
        {
            return RedirectToPage("Two");
        }

        public void OnPostThree(string foo)
        {
            //
        }

        [NonHandler]
        public void OnGetFoo()
        {
            //nothing
        }
    }
}