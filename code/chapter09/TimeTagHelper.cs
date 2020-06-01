using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace chapter09
{
    public class TimeTagHelper : TagHelper
    {
        public string Format { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();
            var stringContent = content.GetContent();
            var time = DateTime.Now.ToString(this.Format);

            output.TagName = "span";
            output.Content.Append(string.Format(CultureInfo.InvariantCulture, stringContent, time));

        }
    }
}
