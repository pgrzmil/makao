using Makao.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Hub
{
    public class GameRoomHub : Microsoft.AspNet.SignalR.Hub
    {
        public GameRoomHub()
        {
        }

        public void GetGameRooms(string sessionId)
        {
            var status = true;
            Clients.Caller.GetGameRoomsResponse(SharedData.GameRooms, status);
        }

        public void SetPlayerReady(string sessionId, string gameRoomId)
        {
        }
    }
}