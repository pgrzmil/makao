using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.GameItems
{
    public class Player: Models.Player
    {
        private List<Card> hand;
        public string ConnectionId { get; set; }

        public Player(string name): base(name)
    {
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
