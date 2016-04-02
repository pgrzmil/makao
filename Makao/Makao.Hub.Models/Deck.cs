using Makao.Extensions;
using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Hub.Models
{
    public class Deck : DeckModel
    {
        public Deck() : base()
        {
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

        public Deck(List<Card> cards) : base(cards)
        {
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