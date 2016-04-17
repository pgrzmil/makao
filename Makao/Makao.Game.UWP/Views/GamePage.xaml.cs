using Makao.Game.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Makao.Game.Views
{
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void sendButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            messageBox.Text = "";
        }
    }
}