using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;

namespace chapter12
{
    [EventSource(Name = SourceName)]
    public sealed class LogElapsedUrlEventSource : EventSource
    {
        private readonly EventCounter _counter;

        public static readonly LogElapsedUrlEventSource Instance = new LogElapsedUrlEventSource();

        private const int SourceId = 1;
        private const string SourceName = "LogElapsedUrl";

        private LogElapsedUrlEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat)
        {
            this._counter = new EventCounter(SourceName, this);
        }

        [Event(SourceId, Message = "Elapsed Time for URL {0}: {1}", Level = EventLevel.Informational)]
        public void LogElapsed(string url, float time)
        {
            this.WriteEvent(SourceId, url, time);
            this._counter.WriteMetric(time);
        }
    }
}
