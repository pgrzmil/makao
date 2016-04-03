using Makao.Hub.Models;
using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Tests.ModelsTests
{
    internal class GameRoomMock : GameRoomModel
    {
        public GameRoomMock(GameRoom gameRoom) : base(gameRoom)
        {
        }

        internal void UpdateCurrentPlayerIndexMock()
        {
            UpdateCurrentPlayerIndex();
        }

        internal void CheckIfWinnerMock(Player player)
        {
            CheckIfWinner(player);
        }

        internal void PopulateDeckMock()
        {
            PopulateDeck();
        }
    }
}