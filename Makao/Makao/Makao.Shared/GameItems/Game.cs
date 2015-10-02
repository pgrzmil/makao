using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.Models
{
    class Game
    {
        private Deck deck;
        private IList<Player> players;
        private IList<Card> stack;
        
        public Game()
        {
            players = new List<Player>();
        }

        public void Start()
        {
            deck = new Deck();
            stack = new List<Card>();
        }

        public void RegisterPlayer(string name)
        {
            players.Add(new Player(name));
        }

    }

}
