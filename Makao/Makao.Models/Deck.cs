using Makao.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public Deck(List<Card> cards)
        {
            this.Cards = new List<Card>(cards);
        }
    }
}