using Microsoft.Extensions.Options;

namespace chapter02
{
    public class PreConfigureLoggingSettings : IConfigureOptions<LoggingSettings>
    {
        public void Configure(LoggingSettings options)
        {
            //act upon the configured instance
        }
    }
}