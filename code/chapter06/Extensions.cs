using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter06
{
    public static class Extensions
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            var anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (var item in anonymousDictionary)
            {
                expando.Add(item);
            }

            return expando as ExpandoObject;
        }
    }
}
