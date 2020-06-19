using Microsoft.Extensions.Logging;
using System;

namespace chapter12
{
    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, Func<string, LogLevel, bool> func)
        {
            loggerFactory.AddProvider(new FileLoggerProvider(func));
            return loggerFactory;
        }

        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, LogLevel minimumLogLevel)
        {
            return AddFile(loggerFactory, (category, logLevel) => logLevel >= minimumLogLevel);
        }

        public static ILoggingBuilder AddFile(this ILoggingBuilder loggingBuilder, Func<string, LogLevel, bool> func)
        {
            return loggingBuilder.AddProvider(new FileLoggerProvider(func));
        }

        public static ILoggingBuilder AddFile(this ILoggingBuilder loggingBuilder, LogLevel minimumLogLevel)
        {
            return AddFile(loggingBuilder, (category, logLevel) => logLevel >= minimumLogLevel);
        }
    }
}
