using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;

namespace chapter02
{
    public sealed class RedirectDisabledFeatureHandler : IDisabledFeaturesHandler
    {
        public RedirectDisabledFeatureHandler(string url) => Url = url;

        public string Url { get; }

        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            context.Result = new RedirectResult(Url);
            return Task.CompletedTask;
        }
    }
}
