using Makao.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakaoHub
{
    public static class SharedData
    {
        internal static IList<Player> AwaitingPlayers { get; set; }
        internal static IList<Game> Games { get; set; }
    }
}