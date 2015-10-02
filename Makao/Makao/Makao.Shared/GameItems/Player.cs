using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    class Player
    {
        public String Name { get; set; }
        private List<Card> hand;

        public Player(string name)
        {
            Name = name;
            hand = new List<Card>();
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public void AddCards(IEnumerable<Card> cards)
        {
            this.hand.AddRange(cards);
        }

    }
}
