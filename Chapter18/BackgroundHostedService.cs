using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chapter18
{
    public class BackgroundHostedService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //do something

                Task.Delay(1000, cancellationToken);
            }

            return Task.CompletedTask;
        }
    }
}
