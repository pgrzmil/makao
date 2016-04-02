using Makao.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class DeckModel
    {
        protected List<Card> cards;

        public DeckModel()
        {
            cards = new List<Card>();
        }

        public DeckModel(List<Card> cards)
        {
            this.cards = new List<Card>(cards);
        }
    }
}