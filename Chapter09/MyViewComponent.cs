using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace chapter09
{
    [ViewComponent(Name = "MyView")]
    public class MyViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int parameter, bool otherParameter)
        {
            return this.Content("This is a view component");
        }
    }
}
