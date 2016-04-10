using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Tests.ModelsTests
{
    public class DeckMock : Deck
    {
        public DeckMock() : base()
        {
        }

        public DeckMock(List<Card> cards) : base(cards)
        {
        }

        public List<Card> GetCards()
        {
            return Cards;
        }
    }
}