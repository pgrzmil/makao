using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.Models
{
    class Game
    {
        private IList<Player> players;
        private Card topCard;
        
        public Game()
        {
            players = new List<Player>();
        }
    }

}
