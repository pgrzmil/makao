using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makao.Models
{
    public delegate void WinnerEventHandler(Player player);

    public enum PlayCardStatus
    {
        WrongCard,
        Success,
        TakeCards,
        PassRound,
        ChooseRank,
        ChooseSuit,
        Error,
        RoundsToWaitInc,
        CardsToTakeInc
    }

    public class PlayCardAction
    {
        public PlayCardStatus Status { get; set; }
        public int Count { get; set; }
        public Card RequestedCard { get; set; }

        public PlayCardAction()
        {
            Status = PlayCardStatus.Error;
        }
    }

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
        public int CardsToTake { get; set; }
        public int RoundsToWait { get; set; }
        public CardSuits? RequestedSuit { get; set; }
        public CardRanks? RequestedRank { get; set; }

        public List<ChatMessage> ChatMessages { get; set; }
        public bool PlayersNextMove { get; private set; }

        //public List<Card> AllowedCards { get; set; }

        public List<Card> AllowedCards()
        {
            var cards = new List<Card>();
            if (Stack == null)
            {
                return null;
            }
            var currentCard = Stack.Last();
            if (!PlayersNextMove)
            {
                switch (currentCard.Rank)
                {
                    case CardRanks.Four:
                        cards.AddRange(Card.GetAllCardsOfRank(CardRanks.Four));
                        return cards;

                    case CardRanks.Three:
                    case CardRanks.Two:
                        cards.AddRange(Card.GetAllCardsOfRank(CardRanks.Two));
                        cards.AddRange(Card.GetAllCardsOfRank(CardRanks.Three));
                        return cards;

                    case CardRanks.Ace:
                        cards.AddRange(Card.GetAllCardsOfRank(CardRanks.Ace));
                        if (this.RequestedSuit != null)
                            cards.AddRange(Card.GetAllCardOfSuit(this.RequestedSuit.Value));
                        else
                            cards.AddRange(Card.GetAllCardOfSuit(currentCard.Suit));

                        return cards;

                    case CardRanks.King: //to be improved - active / inactive state should be taken into account
                        if (currentCard.Suit == CardSuits.Spades || currentCard.Suit == CardSuits.Hearts)
                        {
                            cards.AddRange(Card.GetAllCardsOfRank(CardRanks.King));
                        }
                        else
                        {
                            cards.AddRange(Card.GetAllCardsOfRank(CardRanks.King));
                            cards.AddRange(Card.GetAllCardOfSuit(currentCard.Suit));
                        }
                        return cards;

                    case CardRanks.Jack:
                        cards.AddRange(Card.GetAllCardsOfRank(CardRanks.Jack));
                        if (this.RequestedRank != null)
                            cards.AddRange(Card.GetAllCardsOfRank(this.RequestedRank.Value));
                        else
                            cards.AddRange(Card.GetAllCardOfSuit(currentCard.Suit));

                        return cards;

                    case CardRanks.Queen: //assumption: we don't play as Q for all, all for Q
                    default:
                        cards.AddRange(Card.GetAllCardsOfRank(currentCard.Rank));
                        cards.AddRange(Card.GetAllCardOfSuit(currentCard.Suit));
                        return cards;
                } 
            }
            else
            {
                cards.AddRange(Card.GetAllCardsOfRank(currentCard.Rank));
            }

            return cards;
        }

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

        public virtual PlayCardAction PlayCard(string sessionId, Card playedCard, PlayCardAction previousState = null)
        {
            var status = previousState == null ? new PlayCardAction() : previousState;
            var player = Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (player != null)
            {
                var card = player.Hand.FirstOrDefault(c => c.CardId == playedCard.CardId);
                var topCard = Stack.LastOrDefault();
                if (card != null)
                {
                    if (AllowedCards().FirstOrDefault(c => c.CardId == card.CardId) != null)
                    {
                        //TODO: Implement rules of putting card on stack
                        Stack.Add(card);
                        player.Hand.Remove(card);

                        CheckIfWinner(player);

                        switch (card.Rank)
                        {
                            case CardRanks.Ace:
                                status.Status = PlayCardStatus.ChooseSuit;
                                break;
                            case CardRanks.King:
                                if (card.Suit == CardSuits.Spades || card.Suit == CardSuits.Hearts)
                                {
                                    CardsToTake += 5;
                                    status.Status = PlayCardStatus.CardsToTakeInc;
                                }
                                else
                                {
                                    status.Status = PlayCardStatus.Success;
                                }
                                break;
                            case CardRanks.Jack:
                                status.Status = PlayCardStatus.ChooseRank;
                                break;
                            case CardRanks.Four:
                                RoundsToWait += 1;
                                status.Status = PlayCardStatus.RoundsToWaitInc;
                                break;
                            case CardRanks.Three:
                                CardsToTake += 3;
                                status.Status = PlayCardStatus.CardsToTakeInc;
                                break;
                            case CardRanks.Two:
                                CardsToTake += 2;
                                status.Status = PlayCardStatus.CardsToTakeInc;
                                break;
                            default:
                                status.Status = PlayCardStatus.Success;
                                break;
                        }

                        var cardsAllowedWithCurrent = new List<Card>();
                        cardsAllowedWithCurrent.AddRange(card.GetAllCardsOfRank());                 

                        //if player has more allowed cards
                        if (!cardsAllowedWithCurrent.Select(ac => ac.CardId).Intersect(player.Hand.Select(c => c.CardId)).Any())
                        {
                            PlayersNextMove = false;
                            UpdateCurrentPlayerIndex();
                        }
                        else
                        {
                            PlayersNextMove = true;
                        }
                    }
                    else
                    {
                        status.Status = PlayCardStatus.WrongCard;
                        return status;
                    }
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
            try
            {
                CurrentPlayer().IsTurn = false;

                CurrentPlayerIndex++;
                if (CurrentPlayerIndex >= Players.Count)
                    CurrentPlayerIndex = 0;

                CurrentPlayer().IsTurn = true;
            }
            catch (Exception)
            {
            }
        }

        public bool GiveCardToPlayer(string sessionId, int count = 1)
        {
            var status = false;
            var player = Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (player != null)
            {
                IEnumerable<Card> cards;

                try
                {
                    if (CardsToTake > 0)
                    {
                        count = CardsToTake;
                        CardsToTake = 0;
                    }
                    cards = Deck.TakeCards(count);
                }
                catch (NotEnoughCardsException)
                {
                    RepopulateDeck();
                    cards = Deck.TakeCards(count);
                }

                player.Hand.AddRange(cards);
                UpdateCurrentPlayerIndex();
                status = true;
            }

            return status;
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