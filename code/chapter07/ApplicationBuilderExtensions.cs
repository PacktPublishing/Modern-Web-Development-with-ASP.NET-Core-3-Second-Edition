using Microsoft.AspNetCore.Builder;

namespace chapter07
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseProtectedPaths(this IApplicationBuilder app, params ProtectedPathOptions[] options)
        {
            app.UseMiddleware<ProtectedPathsMiddleware>(options);
            return app;
        }
    }
}
