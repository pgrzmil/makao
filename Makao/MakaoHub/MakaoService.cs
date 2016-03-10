using System;
using System.Collections.Generic;
using System.Text;

namespace Makao.Connection
{
    class MakaoService
    {
        //IHubProxy proxy;
        //String endpoint = "http://localhost:49642/"; // "http://makao.azurewebsites.net"

        //public MainPage()
        //{
        //    this.InitializeComponent();
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //}

        //private void Connection_StateChanged(StateChange state)
        //{
        //    if (state.NewState == ConnectionState.Connected)
        //        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => button.IsEnabled = true);
        //    else
        //        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => button.IsEnabled = false);
        //}

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    var connection = new HubConnection(endpoint);
        //    connection.StateChanged += Connection_StateChanged;
        //    setProxy(proxy, connection);

        //    var connection2 = new HubConnection(endpoint);
        //    connection.StateChanged += Connection_StateChanged;

        //    setProxy(proxy, connection);


        //    connection.Start();
        //}

        //void setProxy(IHubProxy proxy, HubConnection connection)
        //{
        //    proxy = connection.CreateHubProxy("GameHub");
        //    //proxy.On<string>("AddMessage", message =>
        //    //{
        //    //    this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => textBlock.Text = message);
        //    //});
        //    proxy.On<Guid>("SetPlayerId", (x) => SetPlayerId(x));
        //}

        //private void SetPlayerId(Guid id)
        //{
        //    var a = id;
        //}

        //private void registerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    proxy.Invoke("Connect");

        //}
    }
}
