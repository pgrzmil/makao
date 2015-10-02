using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.Models
{
    public class Game
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

    }

}
