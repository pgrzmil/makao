using Makao.Common.Extensions;
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
            }
        }

        public Card TopCard { get { return GameRoom == null || GameRoom.Stack == null ? null : GameRoom.Stack.Last(); } }
        public List<Card> Hand { get { return Player == null || Player.Hand == null ? null : Player.Hand; } }

        public DelegateCommand SetReadyCommand { get; set; }
        public DelegateCommand SitToTableCommand { get; set; }
        public DelegateCommand TakeCardCommand { get; set; }
        public DelegateCommand<Card> PlayCardCommand { get; set; }

        public Player Opponent1 { get { return new Player("Opponent 1"); } }
        public Player Opponent2 { get { return new Player("Opponent 2"); } }
        public Player Opponent3 { get { return new Player("Opponent 3"); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            GameRoom = CacheService.GameRooms.FirstOrDefault(x => x.GameRoomId == parameter.ToString());
            HeaderText = string.Format("MAKAO - {0}", GameRoom.Name);

            //For ui tests purposes
            GameRoom.Stack = new List<Card>() { new Card { Rank = CardRanks.Ace, Suit = CardSuits.Clubs } };
            Player.Hand = new List<Card>() { new Card { Rank = CardRanks.Ace, Suit = CardSuits.Clubs },
                new Card { Rank = CardRanks.Ace, Suit = CardSuits.Hearts },
                new Card { Rank = CardRanks.Ace, Suit = CardSuits.Diamonds } };
            RaisePropertyChanged("TopCard");
            RaisePropertyChanged("Hand");

            //proxy = new HubProxyService("GameRoomHub", SubscribeCallbacks);

            //RefreshGameRoomData().Forget();

            await Task.CompletedTask;
        }

        private void SubscribeCallbacks(IHubProxy proxy)
        {
            proxy.On<GameRoom>("PlayerEnteredRoom", (gameRoom) =>
            {
                GameRoom = gameRoom;
            });

            proxy.On<Player>("GameOver", (winner) =>
            {
                StatusText = "Game over. Winner " + winner.Name;
            });

            proxy.On<GameRoom>("PlayerTookCard", (gameRoom) =>
            {
                GameRoom = gameRoom;
            });

            proxy.On<GameRoom>("PlayerPlayedCard", (gameRoom) =>
            {
                GameRoom = gameRoom;
            });

            proxy.On<GameRoom>("NotifyGameStart", (gameRoom) =>
            {
                GameRoom = gameRoom;
            });

            proxy.On<GameRoom>("SetPlayerReadyResponse", (gameRoom) =>
            {
                GameRoom = gameRoom;
            });

            proxy.On<GameRoom>("PlayerLeftRoom", (gameRoom) =>
            {
                GameRoom = gameRoom;
            });
        }

        private async Task RefreshGameRoomData()
        {
            GameRoom = await proxy.InvokeHubMethod<GameRoom>("GetGameRoom", GameRoom.GameRoomId);
        }

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
    }
}