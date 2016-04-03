using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makao.Hub.Models
{
    public delegate void WinnerEventHandler(Player player);

    public class GameRoomModel : GameRoom
    {
        protected GameRoom gameRoom;

        protected Random rand = new Random();

        public event WinnerEventHandler GameOver;

        public Player CurrentPlayer
        {
            get { return Players[CurrentPlayerIndex]; }
        }

        public GameRoomModel(GameRoom gameRoom)
        {
            this.gameRoom = gameRoom;
        }

        public virtual void Start()
        {
            if (Players.Count > 1)
            {
                IsRunning = true;
                CurrentPlayerIndex = rand.Next(Players.Count);

                Stack = new List<Card>();

                PopulateDeck();
                Shuffle();
                DealCards();
            }
        }

        protected void RepopulateDeck()
        {
            var topCard = Stack.Last();
            Deck = new Deck(Stack.Take(Stack.Count - 1).ToList());
            Shuffle();

            Stack.Clear();
            Stack.Add(topCard);
        }

        protected void DealCards()
        {
            var playersInDealingOrder = Players.Rotate(CurrentPlayerIndex);
            for (int i = 0; i < 5; i++)
            {
                foreach (var player in playersInDealingOrder)
                {
                    player.Hand.AddRange(TakeCards());
                }
            }
            PlayFirstCard();
        }

        protected void PlayFirstCard()
        {
            Stack.AddRange(TakeCards());
        }

        public virtual bool PlayCard(string sessionId, Card card)
        {
            var status = false;
            var player = Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (player != null)
            {
                //TODO: Implement rules of putting card on stack
                Stack.Add(card);
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
                    cards = TakeCards(count);
                }
                catch (NotEnoughCardsException)
                {
                    RepopulateDeck();
                    cards = TakeCards(count);
                }

                player.Hand.AddRange(cards);

                status = true;
            }

            return status;
        }

        //Temporary deck methods
        protected void PopulateDeck()
        {
            Deck = new Deck();

            var suits = Enum.GetValues(typeof(CardSuits)).Cast<CardSuits>();
            var ranks = Enum.GetValues(typeof(CardRanks)).Cast<CardRanks>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    Deck.Cards.Add(new Card { Suit = suit, Rank = rank });
                }
            }
        }

        protected void Shuffle()
        {
            Deck.Cards.Shuffle();
        }

        public List<Card> TakeCards(int count = 1)
        {
            if (Deck.Cards.Count < count)
                throw new NotEnoughCardsException();

            var cardsToTake = Deck.Cards.Take(count).ToList();
            Deck.Cards.RemoveRange(0, count);

            return cardsToTake;
        }

        protected void OnGameOver(Player player)
        {
            GameOver?.Invoke(player);
        }

        #region Decorator properties

        public new List<Player> Players
        {
            get { return gameRoom.Players; }
            set { gameRoom.Players = value; }
        }

        public new string GameRoomId
        {
            get { return gameRoom.GameRoomId; }
            set { gameRoom.GameRoomId = value; }
        }

        public new string Name
        {
            get { return gameRoom.Name; }
            set { gameRoom.Name = value; }
        }

        public new int NumberOfPlayers
        {
            get { return gameRoom.NumberOfPlayers; }
            set { gameRoom.NumberOfPlayers = value; }
        }

        public new int MoveTime
        {
            get { return gameRoom.MoveTime; }
            set { gameRoom.MoveTime = value; }
        }

        public new int CurrentPlayerIndex
        {
            get { return gameRoom.CurrentPlayerIndex; }
            set { gameRoom.CurrentPlayerIndex = value; }
        }

        public new bool IsRunning
        {
            get { return gameRoom.IsRunning; }
            set { gameRoom.IsRunning = value; }
        }

        public new List<Card> Stack
        {
            get { return gameRoom.Stack; }
            set { gameRoom.Stack = value; }
        }

        public new Deck Deck
        {
            get { return gameRoom.Deck; }
            set { gameRoom.Deck = value; }
        }

        public new Card TopCard { get { return Stack == null ? null : Stack.Last(); } }

        #endregion Decorator properties
    }

    public class NotEnoughCardsException : Exception
    {
        public NotEnoughCardsException()
        {
        }

        public NotEnoughCardsException(string message) : base(message)
        {
        }

        public NotEnoughCardsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}