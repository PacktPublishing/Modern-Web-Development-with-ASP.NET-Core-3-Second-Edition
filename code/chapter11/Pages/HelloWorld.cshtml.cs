using Microsoft.AspNetCore.Mvc.RazorPages;

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
            this.Message = "Hello, World!";
        }
    }
}