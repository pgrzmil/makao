using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public class GameRoom
    {
        public List<Card> Stack { get; set; }
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public string GameRoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MoveTime { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public bool IsRunning { get; set; }

        public GameRoom()
        {
            Reset();
        }

        public void Reset()
        {
            Players = new List<Player>();
            NumberOfPlayers = 4;
            MoveTime = 10;
            IsRunning = false;
            Deck = null;
            Stack = null;
        }
    }
}