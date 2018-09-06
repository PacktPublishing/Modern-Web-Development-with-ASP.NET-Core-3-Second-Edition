using Microsoft.Extensions.Options;

namespace chapter02
{
    public class PostConfigureLoggingSettings : IPostConfigureOptions<LoggingSettings>
    {
        public void PostConfigure(string name, LoggingSettings options)
        {
        }
    }
}
