using Makao.Game.Services;
using Makao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Makao.Game.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        public string HeaderText { get; set; }

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        private string statusText;

        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                RaisePropertyChanged("StatusText");
            }
        }

        public BaseViewModel()
        {
            HeaderText = String.Empty;
        }
    }
}