using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace chapter12
{
    public sealed class FileLogger : ILogger
    {
        internal sealed class EmptyDisposable : IDisposable
        {
            public void Dispose() { }
        }

        private readonly string _categoryName;
        private readonly Func<string, LogLevel, bool> _func;

        public FileLogger(string categoryName, Func<string, LogLevel, bool> func)
        {
            this._categoryName = categoryName;
            this._func = func;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new EmptyDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this._func(this._categoryName, logLevel);
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (this.IsEnabled(logLevel))
            {
                var now = DateTime.UtcNow;
                var today = now.ToString("yyyy-MM-dd");
                var fileName = $"{this._categoryName}_{today}.log";
                var message = formatter(state, exception);

                File.AppendAllText(fileName, $"{message}\n");
            }
        }
    }
}
