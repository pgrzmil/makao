using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Game.Models
{
    public class GameRoomModel : GameRoom
    {
        public GameRoomModel(string id) : base(id)
        {
        }

        public string NumberOfPlayerText
        {
            get
            {
                return string.Format("{0}/{1} players", Players != null ? Players.Count : 0, NumberOfPlayers);
            }
        }

        public Card TopCard
        {
            get
            {
                return stack == null ? null : stack.Last();
            }
        }
    }
}