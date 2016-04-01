using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Makao.Tests
{
    public class HubProxyManager
    {
        private string endpoint = "http://localhost:49642/"; // "http://makao.azurewebsites.net"
        private HubConnection connection;
        private IHubProxy proxy;

        protected TimeSpan awaitTimeout = TimeSpan.FromSeconds(10);

        public HubProxyManager()
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
            if (subscribeCallBacks != null)
                subscribeCallBacks(proxy);

            connection.Start().Wait();

            proxy.Invoke(method, args).Wait(awaitTimeout);
        }

        public T InvokeHubMethod<T>(string hub, string method, params object[] args)
        {
            return InvokeHubMethod<T>(hub, method, null, args);
        }

        public T InvokeHubMethod<T>(string hub, string method, Action<IHubProxy> subscribeCallBacks, params object[] args)
        {
            proxy = connection.CreateHubProxy(hub);
            if (subscribeCallBacks != null)
                subscribeCallBacks(proxy);

            connection.Start().Wait();

            var task = proxy.Invoke<T>(method, args);
            task.Wait(awaitTimeout);

            return task.Result;
        }
    }
}