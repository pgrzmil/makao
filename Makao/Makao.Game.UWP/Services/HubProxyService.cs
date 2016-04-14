using Makao.Common.Extensions;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Makao.Game.Services
{
    public class HubProxyService
    {
        private string endpoint = "http://localhost:49642/"; // "http://makao.azurewebsites.net"
        private HubConnection connection;
        private IHubProxy proxy;

        public HubProxyService(string hub, Action<IHubProxy> subscribeCallBacks = null)
        {
            ResetConnection();

            proxy = connection.CreateHubProxy(hub);
            subscribeCallBacks?.Invoke(proxy);
            connection.Received += Connection_Received;
            connection.StateChanged += Connection_StateChanged;
            connection.Start().Wait();
        }

        private void Connection_StateChanged(StateChange obj)
        {
            if (obj.NewState == ConnectionState.Disconnected)
            {
                connection.Start().Wait();
            }
        }

        private void Connection_Received(string obj)
        {
            Debug.WriteLine("\n\nReceived:");
            Debug.WriteLine(obj);
        }

        public void ResetConnection()
        {
            proxy = null;
            connection = new HubConnection(endpoint);
        }

        public async Task<T> InvokeHubMethod<T>(string method, params object[] args)
        {
            return await proxy.Invoke<T>(method, args);
        }
    }
}