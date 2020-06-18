using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter16.Hubs
{
    public class TimerHub : Hub
    {
        public async Task Notify()
        {
            await this.Clients.All.SendAsync("notify");
        }
    }
}
