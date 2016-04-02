using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class GameRoom
    {
        protected List<Card> stack;
        protected Deck deck;

        public List<Player> Players { get; set; }
        public string GameRoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MoveTime { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public bool IsRunning { get; set; }

        public GameRoom(string id)
        {
            GameRoomId = id;
            NumberOfPlayers = 4;
            MoveTime = 10;
        }
    }
}