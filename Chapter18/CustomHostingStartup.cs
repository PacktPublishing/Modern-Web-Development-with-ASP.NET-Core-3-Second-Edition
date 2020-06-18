using Microsoft.AspNetCore.Hosting;

[assembly:HostingStartup(typeof(chapter18.CustomHostingStartup))]

namespace chapter18
{
    public class CustomHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
        }
    }
}
