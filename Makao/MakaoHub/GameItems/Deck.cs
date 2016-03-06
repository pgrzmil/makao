using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Makao.Models;

namespace Makao.GameItems
{
    public delegate void EmptyEventHandler();

    class Deck: Models.Deck
    {
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
    }
}
