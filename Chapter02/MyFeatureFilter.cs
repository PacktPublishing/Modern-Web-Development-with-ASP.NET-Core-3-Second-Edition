using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;

namespace chapter02
{
    [FilterAlias("MyFeature")]
    public class MyFeatureFilter : IFeatureFilter
    {
        private readonly HttpContext _httpContext;

        public MyFeatureFilter(IHttpContextAccessor httpContextAccessor) => _httpContext = httpContextAccessor.HttpContext;

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context) => Task.FromResult(true);
    }
}
