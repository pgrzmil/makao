using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public class Player
    {
        public String Name { get; set; }
        public List<Card> Hand { get; set; }
        public string SessionId { get; set; }
        public string ConnectionId { get; set; }
        public string PlayerId { get; set; }
        public bool IsReady { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
            IsReady = false;
        }

        public void AddCard(Card card)
        {
            Hand.Add(card);
        }

        public void AddCards(IEnumerable<Card> cards)
        {
            this.Hand.AddRange(cards);
        }
    }
}