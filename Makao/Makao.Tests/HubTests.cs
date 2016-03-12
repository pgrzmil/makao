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
    public class HubTests
    {
        private string endpoint = "http://localhost:49642/"; // "http://makao.azurewebsites.net"
        private HubConnection connection;
        private IHubProxy proxy;
        private AutoResetEvent waitHandle;

        protected int RequestAwaitTime { get; set; }
        protected int ResponseAwaitTime { get; set; }

        public virtual void PrepareForTests()
        {
            RequestAwaitTime = 5000;
            ResponseAwaitTime = 10000;

            waitHandle = new AutoResetEvent(false);
            connection = new HubConnection(endpoint);
        }

        protected void ResetConnection()
        {
            connection.Stop();
            proxy = null;
        }

        protected void InvokeHubMethod(string hub, string method, params object[] args)
        {
            proxy = connection.CreateHubProxy(hub);
            connection.Start().Wait();

            proxy.Invoke(method, args).Wait(RequestAwaitTime);
        }

        protected void InvokeHubMethod<T1>(string hub, string method, string responseMethod, Action<T1> responseAction, params object[] args)
        {
            proxy = connection.CreateHubProxy(hub);
            proxy.On<T1>(responseMethod, (arg1) =>
            {
                responseAction(arg1);
                NotifyResponseArrived();
            });

            connection.Start().Wait();
            proxy.Invoke(method, args).Wait(RequestAwaitTime);

            Assert.IsTrue(DidResponseArrived());
        }

        protected void InvokeHubMethod<T1, T2>(string hub, string method, string responseMethod, Action<T1, T2> responseAction, params object[] args)
        {
            proxy = connection.CreateHubProxy(hub);
            proxy.On<T1, T2>(responseMethod, (arg1, arg2) =>
            {
                responseAction(arg1, arg2);
                NotifyResponseArrived();
            });

            connection.Start().Wait();
            proxy.Invoke(method, args).Wait(RequestAwaitTime);

            Assert.IsTrue(DidResponseArrived());
        }

        private void NotifyResponseArrived()
        {
            waitHandle.Set();
        }

        private bool DidResponseArrived()
        {
            return waitHandle.WaitOne(ResponseAwaitTime);
        }
    }
}