﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public delegate void WinnerEventHandler(Player player);

    public class GameRoom
    {
        protected Random rand = new Random();

        public event WinnerEventHandler GameOver;

        public List<Card> Stack { get; set; }
        public Deck Deck { get; set; }
        public List<Player> Players { get; set; }
        public string GameRoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MoveTime { get; set; }
        public int CurrentPlayerIndex { get; set; }
        public bool IsRunning { get; set; }

        public List<ChatMessage> ChatMessages { get; set; }

        public GameRoom()
        {
            Reset();
        }

        public bool HasGameOverListeners()
        {
            return GameOver != null;
        }

        public Player CurrentPlayer()
        {
            return Players.ElementAtOrDefault(CurrentPlayerIndex);
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

        public virtual void Start()
        {
            if (Players.Count > 1)
            {
                IsRunning = true;
                CurrentPlayerIndex = rand.Next(Players.Count);
                CurrentPlayer().IsTurn = true;

                Stack = new List<Card>();
                Deck = new Deck();

                DealCards();
            }
        }

        protected void DealCards()
        {
            var playersInDealingOrder = Players.Rotate(CurrentPlayerIndex);
            for (int i = 0; i < 5; i++)
            {
                foreach (var player in playersInDealingOrder)
                {
                    player.Hand.AddRange(Deck.TakeCards());
                }
            }
            PlayFirstCard();
        }

        protected void PlayFirstCard()
        {
            Stack.AddRange(Deck.TakeCards());
        }

        public virtual bool PlayCard(string sessionId, Card card)
        {
            var status = false;
            var player = Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (player != null)
            {
                //TODO: Implement rules of putting card on stack
                var cardFromHand = player.Hand.FirstOrDefault(c => c.CardId == card.CardId);
                if (cardFromHand != null)
                {
                    Stack.Add(cardFromHand);
                    player.Hand.Remove(cardFromHand);
                    CheckIfWinner(player);

                    UpdateCurrentPlayerIndex();

                    status = true;
                }
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
            CurrentPlayer().IsTurn = false;

            CurrentPlayerIndex++;
            if (CurrentPlayerIndex >= Players.Count)
                CurrentPlayerIndex = 0;

            CurrentPlayer().IsTurn = true;
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
                    cards = Deck.TakeCards(count);
                }
                catch (NotEnoughCardsException)
                {
                    RepopulateDeck();
                    cards = Deck.TakeCards(count);
                }

                player.Hand.AddRange(cards);

                status = true;
            }

            return status;
        }

        protected void RepopulateDeck()
        {
            var topCard = Stack.Last();
            Deck = new Deck(Stack.Take(Stack.Count - 1).ToList());

            Stack.Clear();
            Stack.Add(topCard);
        }

        public void Reset()
        {
            Players = new List<Player>();
            NumberOfPlayers = 4;
            MoveTime = 10;
            IsRunning = false;
            Deck = null;
            Stack = null;
            GameOver = null;
            ChatMessages = new List<ChatMessage>();
        }

        protected void OnGameOver(Player player)
        {
            GameOver?.Invoke(player);
        }
    }
}