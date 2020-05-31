using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter02
{
    [FilterAlias("MyFeature")]
    public class MyFeatureFilter : IFeatureFilter
    { 
        private readonly HttpContext _httpContext;

        public MyFeatureFilter(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContext = httpContextAccessor.HttpContext;
        }

    public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            return true;
        }
    }

}
