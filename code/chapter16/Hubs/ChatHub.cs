using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter16.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message)
        {
            var value = this.Context.GetHttpContext().Request.Query["key"].SingleOrDefault();
            var connectionId = this.Context.ConnectionId;

            await this.Clients.All.SendAsync("message", this.Context.User.Identity.Name, message);
        }
    }
}
