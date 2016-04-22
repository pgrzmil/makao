using Makao.Common.Extensions;
using Makao.Game.Models;
using Makao.Game.Services;
using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Makao.Game.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private HubProxyService proxy;

        private bool showSuitChoosePanel;
        public bool ShowSuitChoosePanel
        {
            get { return showSuitChoosePanel; }
            set
            {
                showSuitChoosePanel = value;
                RaisePropertyChanged("ShowSuitChoosePanel");
            }
        }

        private bool showRankChoosePanel;
        public bool ShowRankChoosePanel
        {
            get { return showRankChoosePanel; }
            set
            {
                showRankChoosePanel = value;
                RaisePropertyChanged("ShowRankChoosePanel");
            }
        }

        public GameViewModel() : base()
        {
            SetReadyCommand = new DelegateCommand(SetReady);
            SitToTableCommand = new DelegateCommand(SitToTable);
            TakeCardCommand = new DelegateCommand(TakeCard);
            PlayCardCommand = new DelegateCommand<Card>(PlayCard);
            PlayerChosenRankCommand = new DelegateCommand<string>(RankChosen);
            PlayerChosenSuitCommand = new DelegateCommand<string>(SuitChosen);

            ConnectOpponentsCommand = new DelegateCommand(ConnectOpponents);
            //  SetReadyOpponentsCommand = new DelegateCommand(SetReadyOpponents);
            PlayOpponentRoundCommand = new DelegateCommand(PlayOpponentRound);

            SendMessageCommand = new DelegateCommand(SendMessage);

            ShowRankChoosePanel = false;
            ShowSuitChoosePanel = false;
        }

        private async void SuitChosen(string suit)
        {
            try
            {
                ShowSuitChoosePanel = false;
                CardSuits cardSuit = (CardSuits)Enum.Parse(typeof(CardSuits), suit);
                var status = await proxy.InvokeHubMethod<bool>("RequestSuit", Player.SessionId, GameRoom.GameRoomId, cardSuit);
                StatusText = String.Format("Chosen suit: " + suit);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private async void RankChosen(string rank)
        {
            try
            {
                ShowRankChoosePanel = false;
                CardRanks cardRank = (CardRanks)Enum.Parse(typeof(CardRanks), rank);
                var status = await proxy.InvokeHubMethod<bool>("RequestRank", Player.SessionId, GameRoom.GameRoomId, cardRank);
                StatusText = String.Format("Chosen rank: " + rank);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private GameRoom gameRoom;
        public GameRoom GameRoom
        {
            get { return gameRoom; }
            set
            {
                gameRoom = value;
                RaisePropertiesChanged();
            }
        }

        private void RaisePropertiesChanged()
        {
            RaisePropertyChanged("GameRoom");
            RaisePropertyChanged("TopCard");
            RaisePropertyChanged("Player");
            RaisePropertyChanged("Opponent1");
            RaisePropertyChanged("Opponent1Status");
            RaisePropertyChanged("Opponent2");
            RaisePropertyChanged("Opponent2Status");
            RaisePropertyChanged("Opponent3");
            RaisePropertyChanged("Opponent3Status");
        }

        public bool IsGameOver { get; set; }
        public string GameOverText { get; set; }

        public Card TopCard { get { return GameRoom == null || GameRoom.Stack == null ? null : GameRoom.Stack.Last(); } }
        public Player Player { get { return GameRoom != null && CacheService.Player != null ? GameRoom.Players.FirstOrDefault(x => x.SessionId == CacheService.Player.SessionId) : null; } }

        public Player Opponent1
        {
            get
            {
                if (GameRoom == null || (GameRoom.Players.Count == 1 && GameRoom.Players[0] == Player))
                    return null;
                var opponentIndex = 0;
                if (Player != null)
                {
                    var offset = GameRoom.Players.IndexOf(Player);
                    opponentIndex = offset + 1 >= GameRoom.NumberOfPlayers ? 0 : offset + 1;
                }
                return GameRoom.Players.ElementAtOrDefault(opponentIndex);
            }
        }

        public string Opponent1Status
        {
            get
            {
                var status = "";
                if (GameRoom != null && Opponent1 != null)
                {
                    if (GameRoom.IsRunning)
                    {
                        status = string.Format("{0} cards", Opponent1.Hand.Count);
                        if (Opponent1.Hand.Count == 1)
                            status += " MAKAO!";
                    }
                    else if (Opponent1.IsReady)
                    {
                        status = "Ready";
                    }
                }
                return status;
            }
        }

        public Player Opponent2
        {
            get
            {
                if (GameRoom == null || (GameRoom.Players.Count == 1 && GameRoom.Players[0] == Player))
                    return null;
                var opponentIndex = 1;
                if (Player != null)
                {
                    var offset = GameRoom.Players.IndexOf(Player);
                    opponentIndex = offset + 2 >= GameRoom.NumberOfPlayers ? 0 : offset + 2;
                }
                return GameRoom.Players.ElementAtOrDefault(opponentIndex);
            }
        }

        public string Opponent2Status
        {
            get
            {
                var status = "";
                if (GameRoom != null && Opponent2 != null)
                {
                    if (GameRoom.IsRunning)
                    {
                        status = string.Format("{0} cards", Opponent2.Hand.Count);
                        if (Opponent2.Hand.Count == 1)
                            status += " MAKAO!";
                    }
                    else if (Opponent2.IsReady)
                    {
                        status = "Ready";
                    }
                }
                return status;
            }
        }

        public Player Opponent3
        {
            get
            {
                if (GameRoom == null || (GameRoom.Players.Count == 1 && GameRoom.Players[0] == Player))
                    return null;
                var opponentIndex = 2;
                if (Player != null)
                {
                    var offset = GameRoom.Players.IndexOf(Player);
                    opponentIndex = offset + 3 >= GameRoom.NumberOfPlayers ? 0 : offset + 3;
                }
                return GameRoom.Players.ElementAtOrDefault(opponentIndex);
            }
        }

        public string Opponent3Status
        {
            get
            {
                var status = "";
                if (GameRoom != null && Opponent3 != null)
                {
                    if (GameRoom.IsRunning)
                    {
                        status = string.Format("{0} cards", Opponent3.Hand.Count);
                        if (Opponent3.Hand.Count == 1)
                            status += " MAKAO!";
                    }
                    else if (Opponent3.IsReady)
                    {
                        status = "Ready";
                    }
                }
                return status;
            }
        }

        #region opponents helper

        public List<OpponentModel> Opponents { get; set; }
        public DelegateCommand ConnectOpponentsCommand { get; set; }
        public DelegateCommand PlayOpponentRoundCommand { get; set; }

        private async void ConnectOpponents()
        {
            Opponents = new List<OpponentModel>();
            for (int i = 0; i < 3; i++)
            {
                var opponent = new OpponentModel();
                var proxy = new HubProxyService("SessionHub");
                opponent.Player = await proxy.InvokeHubMethod<Player>("Connect");

                opponent.Proxy = new HubProxyService("GameRoomHub", SubscribeCallbacks);

                await opponent.Proxy.InvokeHubMethod<bool>("EnterGameRoom", opponent.Player.SessionId, GameRoom.GameRoomId);
                Opponents.Add(opponent);
            }

            SetReadyOpponents();
        }

        private async void SetReadyOpponents()
        {
            foreach (var opponent in Opponents)
            {
                opponent.Player.IsReady = !opponent.Player.IsReady;
                var status = await opponent.Proxy.InvokeHubMethod<bool>("SetPlayerReady", opponent.Player.SessionId, GameRoom.GameRoomId, opponent.Player.IsReady);
            }
        }

        private void PlayOpponentRound()
        {
            if (GameRoom.IsRunning && GameRoom.CurrentPlayer() != Player)
            {
                try
                {
                    var opponent = Opponents.FirstOrDefault(o => o.Player.SessionId == GameRoom.CurrentPlayer().SessionId);
                    if (opponent != null)
                    {
                        var topCard = GameRoom.Stack.LastOrDefault();
                        var opponentAllowedCards = GameRoom.AllowedCards().Select(ac => ac.CardId).Intersect(GameRoom.CurrentPlayer().Hand.Select(c => c.CardId)).ToList();

                        if (opponentAllowedCards.Count > 0)
                        {
                            var card = GameRoom.CurrentPlayer().Hand.FirstOrDefault(c => c.CardId == opponentAllowedCards.First());
                            if (card != null)
                            {
                                PlayCard(card, opponent.Player.SessionId);
                            }
                            else
                            {
                                OpponentTakeCard(opponent);
                            }
                        }
                        else
                        {
                            OpponentTakeCard(opponent);
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        private async void OpponentTakeCard(OpponentModel opponent)
        {
            try
            {
                if (opponent != null)
                {
                    var status = await proxy.InvokeHubMethod<bool>("TakeCard", opponent.Player.SessionId, GameRoom.GameRoomId);
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        #endregion opponents helper

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            GameRoom = CacheService.GameRooms.FirstOrDefault(x => x.GameRoomId == parameter.ToString());
            HeaderText = string.Format("MAKAO - {0}", GameRoom.Name);

            proxy = new HubProxyService("GameRoomHub", SubscribeCallbacks);

            RefreshGameRoomData().Forget();

            await Task.CompletedTask;
        }

        private void SubscribeCallbacks(IHubProxy proxy)
        {
            proxy.On<GameRoom>("PlayerEnteredRoom", PlayerEnteredRoom);
            proxy.On<Player>("GameOver", GameOver);
            proxy.On<GameRoom>("PlayerTookCard", PlayerTookCard);
            proxy.On<GameRoom>("PlayerPlayedCard", PlayerPlayedCard);
            proxy.On<GameRoom>("NotifyGameStart", NotifyGameStart);
            proxy.On<GameRoom>("SetPlayerReadyResponse", SetPlayerReadyResponse);
            proxy.On<GameRoom>("PlayerLeftRoom", PlayerLeftRoom);
            proxy.On<GameRoom>("IncomingMessage", IncomingMessage);
            proxy.On<GameRoom>("PlayerRequestedRank", PlayerRequestedRank);
            proxy.On<GameRoom>("PlayerRequestedSuit", PlayerRequestedSuit);
        }

        private async Task RefreshGameRoomData()
        {
            GameRoom = await proxy.InvokeHubMethod<GameRoom>("GetGameRoom", GameRoom.GameRoomId);
        }

        #region Actions

        public DelegateCommand SetReadyCommand { get; set; }
        public DelegateCommand SitToTableCommand { get; set; }
        public DelegateCommand TakeCardCommand { get; set; }
        public DelegateCommand<Card> PlayCardCommand { get; set; }
        public DelegateCommand<string> PlayerChosenRankCommand { get; set; }
        public DelegateCommand<string> PlayerChosenSuitCommand { get; set; }

        private async void SitToTable()
        {
            bool status;

            if (Player == null)
            {
                status = await proxy.InvokeHubMethod<bool>("EnterGameRoom", CacheService.Player.SessionId, GameRoom.GameRoomId);
            }
        }

        private async void SetReady()
        {
            try
            {
                Player.IsReady = !Player.IsReady;
                var status = await proxy.InvokeHubMethod<bool>("SetPlayerReady", Player.SessionId, GameRoom.GameRoomId, Player.IsReady);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void TakeCard()
        {
            TakeCard(Player.SessionId);
        }
        private async void TakeCard(string sessionId)
        {
            try
            {
                var status = await proxy.InvokeHubMethod<bool>("TakeCard", sessionId, GameRoom.GameRoomId);
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void PlayCard(Card card)
        {
            PlayCard(card, Player.SessionId);
        }

        private async void PlayCard(Card card, string sessionId)
        {
            try
            {
                StatusText = "";
                var status = await proxy.InvokeHubMethod<PlayCardAction>("PlayCard", sessionId, GameRoom.GameRoomId, card);

                switch (status.Status)
                {
                    case PlayCardStatus.WrongCard:
                        StatusText = "Wrong card";
                        break;
                    case PlayCardStatus.TakeCards:
                        StatusText = "Player took card";
                        break;
                    case PlayCardStatus.PassRound:
                        StatusText = "Player passed round";
                        break;
                    case PlayCardStatus.ChooseRank:
                        ShowRankChoosePanel = true;
                        break;
                    case PlayCardStatus.ChooseSuit:
                        ShowSuitChoosePanel = true;
                        break;
                    case PlayCardStatus.NoCardsToPlay:
                        StatusText = "No cards to play.";
                        TakeCard(sessionId);
                        break;
                    case PlayCardStatus.Error:
                        StatusText = "Something went wrong. Don't panic!";
                        break;
                    default:
                        break;
                }
            }
            catch (InvalidOperationException)
            {
            }
        }

        #endregion Actions

        #region Chat

        public DelegateCommand SendMessageCommand { get; set; }

        public string ChatMessage { get; set; }

        private async void SendMessage()
        {
            try
            {
                var message = new ChatMessage() { Message = this.ChatMessage, Sender = this.Player, TimeStamp = DateTime.Now };
                var status = await proxy.InvokeHubMethod<bool>("SendMessage", GameRoom.GameRoomId, message);
            }
            catch (InvalidOperationException)
            {
            }
        }

        #endregion Chat

        #region Callbacks

        private void PlayerEnteredRoom(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void GameOver(Player winner)
        {
            GameRoom.IsRunning = false;
            IsGameOver = true;
            GameOverText = "Game over. Winner " + winner.Name;
            RaisePropertyChanged("IsGameOver");
            RaisePropertyChanged("GameOverText");
            RaisePropertyChanged("GameRoom");
        }

        private void PlayerTookCard(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void PlayerPlayedCard(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void NotifyGameStart(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void SetPlayerReadyResponse(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void PlayerLeftRoom(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void IncomingMessage(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void PlayerRequestedRank(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void PlayerRequestedSuit(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        #endregion Callbacks
    }
}