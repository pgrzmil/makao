using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Models
{
    public delegate void WinnerEventHandler(Player player);

    public class GameRoom
    {
        protected Random rand = new Random();
        protected List<Card> stack;
        protected Deck deck;

        public event WinnerEventHandler GameOver;

        public List<Player> Players { get; set; }
        public string GameRoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MoveTime { get; set; } //TODO: Implement move time
        public int CurrentPlayerIndex { get; set; }
        public bool IsRunning { get; set; }

        public Player CurrentPlayer
        {
            get { return Players[CurrentPlayerIndex]; }
        }

        public GameRoom(string id)
        {
            GameRoomId = id;
            Reset();
        }

        public virtual void Start()
        {
            if (Players.Count > 1)
            {
                IsRunning = true;
                CurrentPlayerIndex = rand.Next(Players.Count);

                deck = new Deck();
                stack = new List<Card>();

                DealCards();
            }
        }

        protected void RepopulateDeck()
        {
            var topCard = stack.Last();
            deck = new Deck(stack.Take(stack.Count - 1).ToList());

            stack.Clear();
            stack.Add(topCard);
        }

        protected void DealCards()
        {
            var playersInDealingOrder = Players.Rotate(CurrentPlayerIndex);
            for (int i = 0; i < 5; i++)
            {
                foreach (var player in playersInDealingOrder)
                {
                    player.Hand.Add(deck.TakeCard());
                }
            }
            PlayFirstCard();
        }

        protected void PlayFirstCard()
        {
            stack.Add(deck.TakeCard());
        }

        public virtual bool PlayCard(string sessionId, Card card)
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

        protected void CheckIfWinner(Player player)
        {
            if (player.Hand.Count == 0)
            {
                IsRunning = false;
                OnGameOver(player);
            }
        }

        protected void UpdateCurrentPlayerIndex()
        {
            CurrentPlayerIndex++;
            if (CurrentPlayerIndex >= Players.Count)
                CurrentPlayerIndex = 0;
        }

        public bool AddPlayer(Player player)
        {
            var status = false;
            if (Players.Count < NumberOfPlayers)
            {
                Players.Add(player);
                status = true;
            }
            return status;
        }

        public void RemovePlayer(Player player)
        {
            Players.Remove(player);
        }

        public bool GiveCardsToPlayer(string sessionId, int count = 1)
        {
            var status = false;
            var player = Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (player != null)
            {
                IEnumerable<Card> cards;

                try
                {
                    cards = deck.TakeCards(count);
                }
                catch (NotEnoughCardsException)
                {
                    RepopulateDeck();
                    cards = deck.TakeCards(count);
                }

                player.Hand.AddRange(cards);

                status = true;
            }

            return status;
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
            IsRunning = false;
            deck = null;
            stack = null;
        }
    }
}