using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace chapter06
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent Button(this IHtmlHelper html, string text)
        {
            return html.Button(text, null);
        }

        public static IHtmlContent Button(this IHtmlHelper html, string text, object htmlAttributes)
        {
            return html.Button(text, null, null, htmlAttributes);
        }

        public static IHtmlContent Button(
            this IHtmlHelper html,
            string text,
            string action,
            object htmlAttributes)
        {
            return html.Button(text, action, null, htmlAttributes);
        }

        public static IHtmlContent Button(this IHtmlHelper html, string text, string action)
        {
            return html.Button(text, action, null, null);
        }

        public static IHtmlContent Button(
            this IHtmlHelper html,
            string text,
            string action,
            string controller)
        {
            return html.Button(text, action, controller, null);
        }

        public static IHtmlContent Button(
            this IHtmlHelper html,
            string text,
            string action,
            string controller,
            object htmlAttributes)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            var builder = new TagBuilder("button");
            builder.InnerHtml.Append(text);

            if (htmlAttributes != null)
            {
                foreach (var prop in htmlAttributes.GetType().GetTypeInfo().GetProperties())
                {
                    builder.MergeAttribute(prop.Name,
                        prop.GetValue(htmlAttributes)?.ToString() ?? string.Empty);
                }
            }

            var url = new UrlHelper(new ActionContext(
                html.ViewContext.HttpContext,
                html.ViewContext.RouteData,
                html.ViewContext.ActionDescriptor));

            if (!string.IsNullOrWhiteSpace(action))
            {
                if (!string.IsNullOrEmpty(controller))
                {
                    builder.Attributes["formaction"] = url.Action(action, controller);
                }
                else
                {
                    builder.Attributes["formaction"] = url.Action(action);
                }
            }

            return builder;
        }

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
