using Makao.Hub.Models;
using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Tests.ModelsTests
{
    internal class DeckMock : Deck
    {
        internal DeckMock() : base()
        {
        }

        internal DeckMock(List<Card> cards) : base(cards)
        {
        }

        internal List<Card> GetCards()
        {
            return cards;
        }
    }
}