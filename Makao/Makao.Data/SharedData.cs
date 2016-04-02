using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Makao.Data
{
    public static class SharedData
    {
        public static IList<Player> Players { get; set; }
        public static IList<GameRoom> GameRooms { get; set; }

        static SharedData()
        {
            Players = new List<Player>();
            GameRooms = new List<GameRoom>();
        }
    }
}