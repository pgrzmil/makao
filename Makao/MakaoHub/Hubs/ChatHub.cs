using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Hub.Hubs
{
    public class ChatHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Send(ChatMessage message)
        {
            Clients.All.broadcastMessage(message);
        }
    }
}