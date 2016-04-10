using Makao.Common.Extensions;
using Makao.Game.Services;
using Makao.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Makao.Game.ViewModels
{
    public class GameRoomViewModel : BaseViewModel
    {
        public DelegateCommand<string> GoToDetails { get; set; }

        public ObservableCollection<GameRoom> GameRooms { get { return CacheService.GameRooms; } }
        public Player Player { get { return CacheService.Player; } }

        public GameRoomViewModel() : base()
        {
            HeaderText = "MAKAO - PICK GAME ROOM";
            GoToDetails = new DelegateCommand<string>((id) => NavigationService.Navigate(typeof(Views.GamePage), id));
        }

        public string Value { get; set; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                Value = suspensionState[nameof(Value)]?.ToString();
            }
            StatusText = "Connecting";
            Connect().Forget();
            FetchGameRooms().Forget();
            await Task.CompletedTask;
        }

        private async Task Connect()
        {
            if (CacheService.Player == null)
            {
                var proxy = new HubProxyService("SessionHub");
                var player = await proxy.InvokeHubMethod<Player>("Connect");
                if (player != null)
                {
                    CacheService.Player = player;
                    RaisePropertyChanged("Player");
                    StatusText = "Connected";
                }
            }
        }

        private async Task FetchGameRooms()
        {
            var proxy = new HubProxyService("GameRoomHub");
            var gamerooms = await proxy.InvokeHubMethod<IList<GameRoom>>("GetGameRooms");

            CacheService.GameRooms = new ObservableCollection<GameRoom>(gamerooms);
            RaisePropertyChanged("GameRooms");
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}