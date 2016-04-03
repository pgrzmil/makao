using Makao.Data;
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
    public class SessionHub : BaseHub
    {
        public Player Connect()
        {
            var name = PlayerNameCreator.GetRandomName();
            var sessionId = getSessionId(Context.ConnectionId);

            var player = new Player(name) { SessionId = sessionId, ConnectionId = Context.ConnectionId, Name = name };
            SharedData.Players.Add(player);

            return player;
        }

        public bool Disconnect(string sessionId)
        {
            var status = false;

            var playerToRemove = SharedData.Players.First(x => x.SessionId == sessionId);
            if (playerToRemove != null)
            {
                var gameRoomHub = new GameRoomHub();
                gameRoomHub.LeaveGameRoom(sessionId);
                SharedData.Players.Remove(playerToRemove);
                status = true;
            }

            return status;
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