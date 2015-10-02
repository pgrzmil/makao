using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Makao.Extensions;

namespace Makao.Models
{
    public delegate void EmptyEventHandler();

    public class Deck
    {
        protected IList<Card> cards;

        public event EmptyEventHandler DeckEmpty;

        public Deck()
        {
            cards = new List<Card>();
            var suits = Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>();
            var ranks = Enum.GetValues(typeof(CardRanks)).Cast<CardRanks>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    cards.Add(new Card { Suit = suit, Rank = rank });
                }
            }
            Shuffle();
        }

        public Deck(IList<Card> cards)
        {
            this.cards = new List<Card>(cards);
            Shuffle();
        }

        protected void Shuffle()
        {
            cards.Shuffle();
        }

        protected void OnDeckEmpty()
        {
            if (DeckEmpty != null)
            {
                DeckEmpty();
            }
        }
    }

}
