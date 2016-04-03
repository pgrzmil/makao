using Makao.Game.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Game.Services
{
    internal static class CacheService
    {
        internal static ObservableCollection<GameRoomModel> GameRooms { get; set; }
        internal static string SessionId { get; set; }

        static CacheService()
        {
            GameRooms = new ObservableCollection<GameRoomModel>();
            GameRooms.Add(new GameRoomModel("1") { Name = "Alaska" });
            GameRooms.Add(new GameRoomModel("2") { Name = "Alabama" });
            GameRooms.Add(new GameRoomModel("3") { Name = "Arkansas" });
            GameRooms.Add(new GameRoomModel("4") { Name = "California" });
            GameRooms.Add(new GameRoomModel("5") { Name = "Florida" });
            GameRooms.Add(new GameRoomModel("6") { Name = "Nevada" });
            GameRooms.Add(new GameRoomModel("7") { Name = "Massachusetts" });
            GameRooms.Add(new GameRoomModel("8") { Name = "Texas" });
            GameRooms.Add(new GameRoomModel("9") { Name = "Utah" });
            GameRooms.Add(new GameRoomModel("10") { Name = "Washington" });
        }
    }
}