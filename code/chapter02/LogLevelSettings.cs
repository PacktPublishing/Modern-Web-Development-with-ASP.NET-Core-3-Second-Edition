using Microsoft.Extensions.Logging;

namespace chapter02
{
    public class LogLevelSettings
    {
        public LogLevel Default { get; set; }
        public LogLevel System { get; set; }
        public LogLevel Microsoft { get; set; }
    }
}