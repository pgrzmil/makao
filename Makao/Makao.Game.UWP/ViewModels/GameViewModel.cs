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
using Windows.UI.Xaml.Navigation;

namespace Makao.Game.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private HubProxyService proxy;

        public GameViewModel() : base()
        {
            SetReadyCommand = new DelegateCommand(SetReady);
            SitToTableCommand = new DelegateCommand(SitToTable);
            TakeCardCommand = new DelegateCommand(TakeCard);
            PlayCardCommand = new DelegateCommand<Card>(PlayCard);

            ConnectOpponentsCommand = new DelegateCommand(ConnectOpponents);
            SetReadyOpponentsCommand = new DelegateCommand(SetReadyOpponents);
            PlayOpponentRoundCommand = new DelegateCommand(PlayOpponentRound);
        }

        private GameRoom gameRoom;
        public GameRoom GameRoom
        {
            get { return gameRoom; }
            set
            {
                gameRoom = value;
                RaisePropertyChanged("GameRoom");
                RaisePropertyChanged("TopCard");
                RaisePropertyChanged("Hand");
                RaisePropertyChanged("Opponent1");
                RaisePropertyChanged("Opponent2");
                RaisePropertyChanged("Opponent3");
            }
        }

        public Card TopCard { get { return GameRoom == null || GameRoom.Stack == null ? null : GameRoom.Stack.Last(); } }
        public List<Card> Hand { get { return Player == null || Player.Hand == null ? null : Player.Hand; } }
        public Player Player { get { return GameRoom == null || CacheService.Player == null ? null : GameRoom.Players.FirstOrDefault(x => x.SessionId == CacheService.Player.SessionId); } }

        public DelegateCommand SetReadyCommand { get; set; }
        public DelegateCommand SitToTableCommand { get; set; }
        public DelegateCommand TakeCardCommand { get; set; }
        public DelegateCommand<Card> PlayCardCommand { get; set; }

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

        public Player Opponent2
        {
            get
            {
                if (GameRoom == null || (GameRoom.Players.Count == 1 && GameRoom.Players[0].SessionId == Player.SessionId))
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

        public Player Opponent3
        {
            get
            {
                if (GameRoom == null || (GameRoom.Players.Count == 1 && GameRoom.Players[0].SessionId == Player.SessionId))
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

        #region opponents helper

        public List<OpponentModel> Opponents { get; set; }
        public DelegateCommand ConnectOpponentsCommand { get; set; }
        public DelegateCommand SetReadyOpponentsCommand { get; set; }
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

            RaisePropertyChanged("Opponent1");
            RaisePropertyChanged("Opponent2");
            RaisePropertyChanged("Opponent3");
        }

        private async void SetReadyOpponents()
        { }

        private async void PlayOpponentRound()
        { }

        #endregion opponents helper

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            GameRoom = CacheService.GameRooms.FirstOrDefault(x => x.GameRoomId == parameter.ToString());
            HeaderText = string.Format("MAKAO - {0}", GameRoom.Name);

            //For ui tests purposes
            //GameRoom.Stack = new List<Card>() { new Card { Rank = CardRanks.Ace, Suit = CardSuits.Clubs } };
            //Player.Hand = new List<Card>() { new Card { Rank = CardRanks.Ace, Suit = CardSuits.Clubs },
            //    new Card { Rank = CardRanks.Ace, Suit = CardSuits.Hearts },
            //    new Card { Rank = CardRanks.Ace, Suit = CardSuits.Diamonds } };
            //RaisePropertyChanged("TopCard");
            //RaisePropertyChanged("Hand");

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
        }

        private async Task RefreshGameRoomData()
        {
            GameRoom = await proxy.InvokeHubMethod<GameRoom>("GetGameRoom", GameRoom.GameRoomId);
        }

        #region Actions

        private async void SitToTable()
        {
            var status = await proxy.InvokeHubMethod<bool>("EnterGameRoom", Player.SessionId, GameRoom.GameRoomId);
            StatusText = status ? "EnterGameRoom successful" : "EnterGameRoom failed";
        }

        private async void SetReady()
        {
            Player.IsReady = !Player.IsReady;
            var status = await proxy.InvokeHubMethod<bool>("SetPlayerReady", Player.SessionId, GameRoom.GameRoomId, Player.IsReady);
            StatusText = status ? "SetPlayerReady successful" : "SetPlayerReady failed";
        }

        private async void TakeCard()
        {
            var status = await proxy.InvokeHubMethod<bool>("TakeCard", Player.SessionId, GameRoom.GameRoomId);
            StatusText = status ? "TakeCard successful" : "TakeCard failed";
        }

        private async void PlayCard(Card card)
        {
            var status = await proxy.InvokeHubMethod<bool>("PlayCard", Player.SessionId, GameRoom.GameRoomId, card);
            StatusText = status ? "PlayCard successful" : "PlayCard failed";
        }

        #endregion Actions

        #region Callbacks

        private void PlayerEnteredRoom(GameRoom gameRoom)
        {
            GameRoom = gameRoom;
        }

        private void GameOver(Player winner)
        {
            StatusText = "Game over. Winner " + winner.Name;
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

        #endregion Callbacks
    }
}