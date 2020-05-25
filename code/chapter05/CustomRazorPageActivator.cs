using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace chapter05
{
    public class CustomRazorPageActivator : IRazorPageActivator
    {
        private readonly IRazorPageActivator _activator;

        public CustomRazorPageActivator(
            IModelMetadataProvider metadataProvider,
            IUrlHelperFactory urlHelperFactory,
            IJsonHelper jsonHelper,
            DiagnosticSource diagnosticSource,
            HtmlEncoder htmlEncoder,
            IModelExpressionProvider modelExpressionProvider)
        {
            this._activator = new RazorPageActivator(metadataProvider, urlHelperFactory,
                jsonHelper, diagnosticSource, htmlEncoder, modelExpressionProvider);
        }

        public void Activate(IRazorPage page, ViewContext context)
        {
            if (page is ICustomInitializable)
            {
                (page as ICustomInitializable).Init(context);
            }

            this._activator.Activate(page, context);
        }
    }
}
