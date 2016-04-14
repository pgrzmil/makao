using Makao.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class Deck
    {
        protected List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();

            var suits = Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>();
            var ranks = Enum.GetValues(typeof(CardRanks)).Cast<CardRanks>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    Cards.Add(new Card(suit, rank));
                }
            }

            Shuffle();
        }

        public Deck(List<Card> cards)
        {
            this.Cards = new List<Card>(cards);
            Shuffle();
        }

        protected void Shuffle()
        {
            Cards.Shuffle();
        }

        public List<Card> TakeCards(int count = 1)
        {
            if (Cards.Count < count)
                throw new NotEnoughCardsException();

            var cardsToTake = Cards.Take(count).ToList();
            Cards.RemoveRange(0, count);

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