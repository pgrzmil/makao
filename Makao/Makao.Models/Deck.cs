using Makao.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class Deck
    {
        protected List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
        }

        public Deck(List<Card> cards)
        {
            this.cards = new List<Card>(cards);
        }
    }
}