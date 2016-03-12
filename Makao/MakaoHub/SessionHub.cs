using Makao.Extensions;
using Makao.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Makao.Hub
{
    public class SessionHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Connect()
        {
            var name = PlayerNameCreator.GetRandomName();
            var sessionId = getSessionId(Context.ConnectionId);

            var player = new Player(name) { SessionId = sessionId, ConnectionId = Context.ConnectionId, Name = name };
            SharedData.Players.Add(player);

            Clients.Caller.ConnectResponse(player, true);
        }

        public void Disconnect(string sessionId)
        {
            var playerToRemove = SharedData.Players.First(x => x.SessionId == sessionId);
            if (playerToRemove != null)
                SharedData.Players.Remove(playerToRemove);

            Clients.Caller.DisconnectResponse(true);
        }

        private string getSessionId(string connectionId = "")
        {
            var data = DateTime.UtcNow.GetTimestamp().ToString();
            data += connectionId;
            var sessionId = data.GetSHA1();

            return SharedData.Players.Count(p => p.SessionId == sessionId) != 0 ? getSessionId(connectionId) : sessionId;
        }
    }
}