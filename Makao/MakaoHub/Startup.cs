using Makao.Data;
using Makao.Models;
using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Makao.Hub.Startup))]

namespace Makao.Hub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ResetData();
        }

        internal static void ResetData()
        {
            SharedData.GameRooms.Add(new GameRoom { Name = "Alaska", GameRoomId = "1" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Alabama", GameRoomId = "2" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Arkansas", GameRoomId = "3" });
            SharedData.GameRooms.Add(new GameRoom { Name = "California", GameRoomId = "4" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Florida", GameRoomId = "5" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Nevada", GameRoomId = "6" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Massachusetts", GameRoomId = "7" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Texas", GameRoomId = "8" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Utah", GameRoomId = "9" });
            SharedData.GameRooms.Add(new GameRoom { Name = "Washington", GameRoomId = "10" });
        }
    }
}