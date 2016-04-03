using Makao.Game.Models;
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

        public ObservableCollection<GameRoomModel> GameRooms { get { return CacheService.GameRooms; } }

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
            await Task.CompletedTask;
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