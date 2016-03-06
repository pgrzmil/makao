using Makao.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.GameItems
{
    class Game
    {
        protected Deck deck;
        protected IList<Player> players;
        protected IList<Card> stack;

        public Game()
        {
            players = new List<Player>();
        }

        public virtual void Start()
    {
            deck = new Deck();
            stack = new List<Card>();
        }

        public void AddPlayer(string name)
        {
            players.Add(new Player(name));
        }

    }

}
