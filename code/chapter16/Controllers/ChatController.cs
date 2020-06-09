using chapter16.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter16.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _context;

        public ChatController(IHubContext<ChatHub> context)
        {
            this._context = context;
        }

        [HttpGet("Send/{message}")]
        public async Task<IActionResult> Send(string message)
        {
            await this._context.Clients.All.SendAsync("message", this.User.Identity.Name, message);
            return Json(new { Message = message });
        }
    }
}
