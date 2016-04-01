using Makao.Game.UWP.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;

namespace Makao.Game.UWP.Views
{
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }
    }
}

