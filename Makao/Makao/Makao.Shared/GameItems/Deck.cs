using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Makao.Extensions;

namespace Makao.Models
{
    public delegate void EmptyEventHandler();

    class Deck
    {
        private IList<Card> cards;

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

        private void Shuffle()
        {
            cards.Shuffle();
        }

        internal Card TakeCard()
        {
            var card = cards.FirstOrDefault();
            cards.RemoveAt(0);

            if (cards.Count == 0)
                OnDeckEmpty();

            return card;
        }

        internal IList<Card> TakeCards(int count)
        {
            count = cards.Count >= count ? count : cards.Count;
            var cardsToTake = cards.Take(count).ToList();
            cardsToTake.RemoveRange(0, count);

            if (cards.Count == 0)
                OnDeckEmpty();

            return cardsToTake;
        }

        private void OnDeckEmpty()
        {
            if (DeckEmpty != null)
            {
                DeckEmpty();
            }
        }
    }

}
