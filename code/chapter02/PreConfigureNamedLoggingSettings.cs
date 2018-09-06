using Microsoft.Extensions.Options;

namespace chapter02
{
    public class PreConfigureNamedLoggingSettings : IConfigureNamedOptions<LoggingSettings>
    {
        public void Configure(string name, LoggingSettings options)
        {
            //act upon the configured instance
        }

        public void Configure(LoggingSettings options)
        {
        }
    }
}