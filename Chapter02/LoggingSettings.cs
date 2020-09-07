using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace chapter02
{
    public class LoggingSettings
    {
        public Dictionary<string, LogLevel> LogLevel { get; set; } = new Dictionary<string, LogLevel>();
    }
}