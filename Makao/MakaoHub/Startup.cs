﻿using Makao.Data;
using Makao.Hub.Models;
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
            SharedData.GameRooms.Add(new GameRoomModel("1") { Name = "Alaska" });
            SharedData.GameRooms.Add(new GameRoomModel("2") { Name = "Alabama" });
            SharedData.GameRooms.Add(new GameRoomModel("3") { Name = "Arkansas" });
            SharedData.GameRooms.Add(new GameRoomModel("4") { Name = "California" });
            SharedData.GameRooms.Add(new GameRoomModel("5") { Name = "Florida" });
            SharedData.GameRooms.Add(new GameRoomModel("6") { Name = "Nevada" });
            SharedData.GameRooms.Add(new GameRoomModel("7") { Name = "Massachusetts" });
            SharedData.GameRooms.Add(new GameRoomModel("8") { Name = "Texas" });
            SharedData.GameRooms.Add(new GameRoomModel("9") { Name = "Utah" });
            SharedData.GameRooms.Add(new GameRoomModel("10") { Name = "Washington" });
        }
    }
}