using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace chapter02
{
    public class LoggingSettings
    {
        public Dictionary<string, LogLevel> LogLevel { get; } = new Dictionary<string, LogLevel>();
    }
}