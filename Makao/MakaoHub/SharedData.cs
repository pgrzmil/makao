using Makao.GameItems;
using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakaoHub
{
    public static class SharedData
    {
        internal static IList<Player> Players { get; set; }
        internal static IList<Game> Games { get; set; }

        static SharedData()
        {
            SharedData.Players = new List<Player>();
            SharedData.Games = new List<Game>();
        }
    }
}