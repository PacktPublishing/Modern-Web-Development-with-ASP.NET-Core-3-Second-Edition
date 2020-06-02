using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace chapter07
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseProtectedPaths(this IApplicationBuilder app, params ProtectedPathOptions[] options)
        {
            foreach (var option in options ?? Enumerable.Empty<ProtectedPathOptions>())
            {
                app.UseMiddleware<ProtectedPathsMiddleware>(option);
            }

            return app;
        }
    }
}
