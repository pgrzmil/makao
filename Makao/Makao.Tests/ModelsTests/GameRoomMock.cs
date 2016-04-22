using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Tests.ModelsTests
{
    public class GameRoomMock : GameRoom
    {
        internal void UpdateCurrentPlayerIndexMock()
        {
            UpdateCurrentPlayerIndex(this.AllowedCards());
        }

        internal void CheckIfWinnerMock(Player player)
        {
            CheckIfWinner(player);
        }

        public override void Start()
        {
            if (Players.Count > 1)
            {
                IsRunning = true;
                CurrentPlayerIndex = rand.Next(Players.Count);

                Stack = new List<Card>();
                Deck = new DeckMock();

                DealCards();
            }
        }
    }
}