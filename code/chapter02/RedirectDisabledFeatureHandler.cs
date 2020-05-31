using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter02
{
    public sealed class RedirectDisabledFeatureHandler : IDisabledFeaturesHandler
    {
        public RedirectDisabledFeatureHandler(string url)
        {
            this.Url = url;
        }

        public string Url { get; }

        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            context.Result = new RedirectResult(this.Url);
            return Task.CompletedTask;
        }
    }
}
