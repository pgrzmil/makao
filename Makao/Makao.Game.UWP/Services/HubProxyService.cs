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

        protected TimeSpan awaitTimeout = TimeSpan.FromSeconds(10);

        public HubProxyService()
        {
            ResetConnection();
        }

        public void ResetConnection()
        {
            proxy = null;
            connection = new HubConnection(endpoint);
        }

        public void InvokeHubMethod(string hub, string method, params object[] args)
        {
            InvokeHubMethod(hub, method, null, args);
        }

        public void InvokeHubMethod(string hub, string method, Action<IHubProxy> subscribeCallBacks, params object[] args)
        {
            proxy = connection.CreateHubProxy(hub);
            subscribeCallBacks?.Invoke(proxy);

            connection.Start().Wait();

            proxy.Invoke(method, args).Wait(awaitTimeout);
        }

        public Task<T> InvokeHubMethod<T>(string hub, string method, params object[] args)
        {
            return InvokeHubMethod<T>(hub, method, null, args);
        }

        public async Task<T> InvokeHubMethod<T>(string hub, string method, Action<IHubProxy> subscribeCallBacks, params object[] args)
        {
            proxy = connection.CreateHubProxy(hub);
            if (subscribeCallBacks != null)
                subscribeCallBacks(proxy);

            await connection.Start();
            return await proxy.Invoke<T>(method, args);
        }
    }
}