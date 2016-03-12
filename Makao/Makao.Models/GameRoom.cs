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
        public int MaxPlayersNumber { get; set; }

        public GameRoom(string id, int playersNumber = 4)
        {
            Players = new List<Player>();
            GameRoomId = id;
            MaxPlayersNumber = playersNumber;
        }

        public virtual void Start()
        {
            deck = new Deck();
            stack = new List<Card>();
        }

        public void AddPlayer(Player player)
        {
            if (Players.Count < MaxPlayersNumber)
                Players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }
    }
}