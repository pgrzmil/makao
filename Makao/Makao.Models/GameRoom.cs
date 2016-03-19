using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public delegate void WinnerEventHandler(Player player);

    public class GameRoom
    {
        protected IList<Card> stack;

        public event WinnerEventHandler GameOver;

        public Deck Deck { get; set; }
        public IList<Player> Players { get; set; }
        public string GameRoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MoveTime { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public bool IsRunning { get; set; }

        public GameRoom(string id)
        {
            GameRoomId = id;
            Reset();
        }

        public void Start()
        {
            IsRunning = true;
            Deck = new Deck();
            Deck.DeckEmpty += Deck_DeckEmpty;
            stack = new List<Card>();

            DealCards();
        }

        private void Deck_DeckEmpty()
        {
            var topCard = stack.Last();
            Deck = new Deck(stack.Take(stack.Count - 1).ToList());
            Deck.DeckEmpty += Deck_DeckEmpty;

            stack.Clear();
            stack.Add(topCard);
        }

        private void DealCards()
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (var player in Players)
                {
                    player.Hand.Add(Deck.TakeCard());
                }
            }
            PlayFirstCard();
        }

        private void PlayFirstCard()
        {
            stack.Add(Deck.TakeCard());
        }

        public bool PlayCard(string sessionId, Card card)
        {
            var status = false;
            var player = Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (player != null)
            {
                //TODO: Implement rules of putting card on stack
                stack.Add(card);
                player.Hand.Remove(card);
                CheckIfWinner(player);

                UpdateCurrentPlayerIndex();

                status = true;
            }

            return status;
        }

        private void CheckIfWinner(Player player)
        {
            if (player.Hand.Count == 0)
            {
                IsRunning = false;
                OnGameOver(player);
            }
        }

        private void UpdateCurrentPlayerIndex()
        {
            CurrentPlayerIndex++;
            if (CurrentPlayerIndex > Players.Count)
                CurrentPlayerIndex = 0;
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

        protected void OnGameOver(Player player)
        {
            if (GameOver != null)
            {
                GameOver(player);
            }
        }

        public void Reset()
        {
            Players = new List<Player>();
            NumberOfPlayers = 4;
            MoveTime = 10;
            CurrentPlayerIndex = 0;
            IsRunning = false;
            Deck = null;
            stack = null;
        }
    }
}