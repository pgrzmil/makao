using Makao.Common.Extensions;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
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
            connection.Start().Wait();
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