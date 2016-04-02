using Makao.Hub.Models;
using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Tests.ModelsTests
{
    internal class GameRoomMock : GameRoom
    {
        internal GameRoomMock(string id) : base(id)
        {
        }

        public override void Start()
        {
            if (Players.Count > 1)
            {
                IsRunning = true;
                CurrentPlayerIndex = rand.Next(Players.Count);

                deck = new DeckMock();
                stack = new List<Card>();

                DealCardsMock();
            }
        }

        internal IList<Card> GetStack()
        {
            return stack;
        }

        internal DeckMock GetDeck()
        {
            return deck as DeckMock;
        }

        internal void DealCardsMock()
        {
            DealCards();
        }

        internal void UpdateCurrentPlayerIndexMock()
        {
            UpdateCurrentPlayerIndex();
        }

        internal void CheckIfWinnerMock(Player player)
        {
            CheckIfWinner(player);
        }
    }
}