using System;
using System.Collections.Generic;
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
    }
}