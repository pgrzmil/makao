using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Hub
{
    public static class SharedData
    {
        internal static IList<Player> Players { get; set; }
        internal static IList<GameRoom> GameRooms { get; set; }

        static SharedData()
        {
            SharedData.Players = new List<Player>();
            SharedData.GameRooms = new List<GameRoom>();
            SharedData.GameRooms.Add(new GameRoom("1") { Name = "Alaska" });
            SharedData.GameRooms.Add(new GameRoom("2") { Name = "Alabama" });
            SharedData.GameRooms.Add(new GameRoom("3") { Name = "Arkansas" });
            SharedData.GameRooms.Add(new GameRoom("4") { Name = "California" });
            SharedData.GameRooms.Add(new GameRoom("5") { Name = "Florida" });
            SharedData.GameRooms.Add(new GameRoom("6") { Name = "Nevada" });
            SharedData.GameRooms.Add(new GameRoom("7") { Name = "Massachusetts" });
            SharedData.GameRooms.Add(new GameRoom("8") { Name = "Texas" });
            SharedData.GameRooms.Add(new GameRoom("9") { Name = "Utah" });
            SharedData.GameRooms.Add(new GameRoom("10") { Name = "Washington" });
        }
    }
}