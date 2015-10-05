using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.Models
{
    class Game
    {
        private IList<string> playersNames;
        private Card topCard;
        
        public Game()
        {
            playersNames = new List<string>();
        }
    }

}
