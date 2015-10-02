﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Makao
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IHubProxy proxy;

        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var connection = new HubConnection("http://makao.azurewebsites.net");
            connection.StateChanged += Connection_StateChanged;

            proxy = connection.CreateHubProxy("GameHub");
            proxy.On<string>("AddMessage", message =>
            {
                this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => textBlock.Text = message);
            });

            connection.Start();
        }

        private void Connection_StateChanged(StateChange state)
        {
            if (state.NewState == ConnectionState.Connected)
                this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => button.IsEnabled = true);
            else
                this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => button.IsEnabled = false);

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            proxy.Invoke("Send", DateTime.Now.ToString());
        }
    }
}
