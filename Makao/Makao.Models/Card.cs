using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class Card
    {
        public CardSuits Suit { get; set; }
        public CardRanks Rank { get; set; }
        public string CardId { get; set; }

        public Card(CardSuits suit, CardRanks rank)
        {
            Suit = suit;
            Rank = rank;
            CardId = string.Format("{0}_of_{1}", rank, suit);
        }

        public override string ToString()
        {
            return CardId;
        }

        public List<Card> GetAllCardsOfSuit()
        {
            var ranks = Enum.GetValues(typeof(CardRanks)).Cast<CardRanks>();
            var cards = new List<Card>();
            foreach (var rank in ranks)
            {
                cards.Add(new Card(this.Suit, rank));
            }

            return cards;
        }

        public List<Card> GetAllCardsOfRank()
        {
            var suits = Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>();
            var cards = new List<Card>();
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, this.Rank));
            }

            return cards;
        }

        //IDK if this should be here. Probably not and it should be in some separate class? in gameroom? To discuss
        public static List<Card> GetAllCardOfSuit(CardSuits suit)
        {
            var ranks = Enum.GetValues(typeof(CardRanks)).Cast<CardRanks>();
            var cards = new List<Card>();
            foreach (var rank in ranks)
            {
                cards.Add(new Card(suit, rank));
            }

            return cards;
        }

        public static List<Card> GetAllCardsOfRank(CardRanks rank)
        {
            var suits = Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>();
            var cards = new List<Card>();
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, rank));
            }

            return cards;
        }
    }
}