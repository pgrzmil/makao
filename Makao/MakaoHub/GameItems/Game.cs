using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.GameItems
{
    class Game: Models.Game
    {
        public void RegisterPlayer(string name)
        {
            players.Add(new Player(name));
        }

    }

}
