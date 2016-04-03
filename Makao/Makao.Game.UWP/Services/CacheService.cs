using Makao.Models;
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
        internal static ObservableCollection<GameRoom> GameRooms { get; set; }
        internal static string SessionId { get; set; }

        static CacheService()
        {
            GameRooms = new ObservableCollection<GameRoom>();
        }
    }
}