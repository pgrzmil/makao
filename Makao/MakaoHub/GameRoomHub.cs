using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Makao.Models;

namespace Makao.Hub
{
    public class GameRoomHub : Microsoft.AspNet.SignalR.Hub
    {
        public GameRoomHub()
        {

        }

        public void GetGameRooms(string sessionId)
        {
            Clients.Caller.GetGameRoomsResponse(SharedData.GameRooms);
        }

        public void SetPlayerReady(string sessionId, string gameRoomId)
        {

        }
    }
}