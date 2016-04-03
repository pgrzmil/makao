using Makao.Game.Models;
using Makao.Game.Services;
using Makao.Models;
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
        public GameViewModel() : base()
        {
            TakeCardCommand = new DelegateCommand(() => { return; });
        }

        public GameRoomModel GameRoom { get; set; }
        public DelegateCommand TakeCardCommand { get; set; }

        public Player Opponent1 { get { return new Player("Opponent 1"); } }
        public Player Opponent2 { get { return new Player("Opponent 2"); } }
        public Player Opponent3 { get { return new Player("Opponent 3"); } }
        public Player Player { get { return new Player("Player"); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            GameRoom = CacheService.GameRooms.FirstOrDefault(x => x.GameRoomId == parameter.ToString());
            HeaderText = string.Format("MAKAO - {0}", GameRoom.Name);
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }
    }
}