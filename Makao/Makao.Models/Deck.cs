using Makao.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public delegate void DeckEmptyEventHandler();

    public class Deck
    {
        protected List<Card> cards;

        public event DeckEmptyEventHandler DeckEmpty;

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

        public Deck(List<Card> cards)
        {
            this.cards = new List<Card>(cards);
            Shuffle();
        }

        protected void Shuffle()
        {
            cards.Shuffle();
        }

        public Card TakeCard()
        {
            var card = cards.FirstOrDefault();
            cards.RemoveAt(0);

            if (cards.Count == 0)
                OnDeckEmpty();

            return card;
        }

        public List<Card> TakeCards(int count)
        {
            count = cards.Count >= count ? count : cards.Count;
            var cardsToTake = cards.Take(count).ToList();
            cards.RemoveRange(0, count);

            if (cards.Count == 0)
                OnDeckEmpty();

            return cardsToTake;
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