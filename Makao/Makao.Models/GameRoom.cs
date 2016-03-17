using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.Models
{
    public class GameRoom
    {
        protected IList<Card> stack;
        protected Deck deck;

        public IList<Player> Players { get; set; }
        public string GameRoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MoveTime { get; set; }
        public int CurrentPlayerIndex { get; set; }

        public GameRoom(string id)
        {
            Players = new List<Player>();
            GameRoomId = id;
            NumberOfPlayers = 4;
            MoveTime = 10;
            CurrentPlayerIndex = 0;
        }

        public virtual void Start()
        {
            deck = new Deck();
            stack = new List<Card>();
        }

        public void AddPlayer(Player player)
        {
            if (Players.Count < NumberOfPlayers)
                Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }
    }
}