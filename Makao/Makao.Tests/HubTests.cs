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
        protected IList<HubProxyManager> hubProxies;
        protected AutoResetEvent waitHandle;
        protected TimeSpan awaitTimeout = TimeSpan.FromSeconds(15);

        public HubProxyManager DefaultProxy { get { return hubProxies.FirstOrDefault(); } }

        public virtual void PrepareForTests()
        {
            PrepareForTests(1);
        }

        public virtual void PrepareForTests(int numberOfElements)
        {
            hubProxies = new List<HubProxyManager>();
            for (int i = 0; i < numberOfElements; i++)
            {
                hubProxies.Add(new HubProxyManager());
            }
        }

        public virtual void Cleanup()
        {
            var proxy = new HubProxyManager();
            proxy.InvokeHubMethod("GameRoomHub", "ResetData");
        }

        protected void NotifyResponseArrived()
        {
            waitHandle.Set();
        }

        protected bool DidResponseArrived()
        {
            return waitHandle.WaitOne(awaitTimeout);
        }
    }
}