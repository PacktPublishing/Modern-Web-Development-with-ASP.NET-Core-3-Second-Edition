using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter05
{
    public class ThemesViewLocationExpander : IViewLocationExpander
    {
        public ThemesViewLocationExpander(string theme)
        {
            this.Theme = theme;
        }

        public string Theme { get; }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            var theme = context.Values["theme"];

            return viewLocations
              .Select(x => x.Replace("/Views/", "/Views/" + theme + "/"))
              .Concat(viewLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["theme"] = this.Theme;
        }
    }
}
