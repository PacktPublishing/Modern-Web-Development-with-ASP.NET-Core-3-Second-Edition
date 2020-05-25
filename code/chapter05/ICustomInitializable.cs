using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace chapter05
{
    public interface ICustomInitializable : IRazorPage
    {
        void Init(ViewContext context);
    }
}