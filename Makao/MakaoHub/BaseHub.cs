using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Hub
{
    public class BaseHub : Microsoft.AspNet.SignalR.Hub
    {
        protected void UpdateConnectionId(Player player)
        {
            if (player.ConnectionId != Context.ConnectionId)
            {
                player.ConnectionId = Context.ConnectionId;
            }
        }
    }
}