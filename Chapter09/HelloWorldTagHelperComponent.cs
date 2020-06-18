using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace chapter09
{
    public class HelloWorldTagHelperComponent : TagHelperComponent
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.TagName.ToLowerInvariant() == "head")
            {
                output.Content.AppendHtml("<script> window.alert('Hello, World!') </script>");
            }
            return Task.CompletedTask;
        }
    }
}