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
            if (cards.Count == 0)
                throw new NotEnoughCardsException();

            var card = cards.FirstOrDefault();
            cards.RemoveAt(0);

            return card;
        }

        public List<Card> TakeCards(int count)
        {
            if (cards.Count < count)
                throw new NotEnoughCardsException();

            var cardsToTake = cards.Take(count).ToList();
            cards.RemoveRange(0, count);

            return cardsToTake;
        }
    }

    public class NotEnoughCardsException : Exception
    {
        public NotEnoughCardsException()
        {
        }

        public NotEnoughCardsException(string message) : base(message)
        {
        }

        public NotEnoughCardsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}