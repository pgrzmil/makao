using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Makao.Models;

namespace MakaoHub
{
    public class GameHub : Hub
    {
        private IList<Player> AwaitingPlayers;

        public void RegisterPlayer(Makao.Models.Player player)
        {
            AwaitingPlayers.Add(player);
        }

        public void Send(string message)
        {
            Clients.All.AddMessage(message);
        }
    }
}