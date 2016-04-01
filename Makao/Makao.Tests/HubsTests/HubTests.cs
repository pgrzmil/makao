using Makao.Models;
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

        public HubProxyManager DefaultProxy { get { return hubProxies.FirstOrDefault(); } }

        public virtual void PrepareForTests()
        {
            PrepareForTests(1);
        }

        public virtual void PrepareForTests(int numberOfElements)
        {
            waitHandle = new AutoResetEvent(false);
            hubProxies = new List<HubProxyManager>();
            for (int i = 0; i < numberOfElements; i++)
            {
                hubProxies.Add(new HubProxyManager());
            }
        }

        public virtual void Cleanup()
        {
            var proxy = new HubProxyManager();
            proxy.InvokeHubMethod("MaintenanceHub", "ResetData");
        }

        protected List<Player> ConnectMultiplePlayers(int numberOfPlayers)
        {
            var players = new List<Player>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                var player = hubProxies[i].InvokeHubMethod<Player>("SessionHub", "Connect");
                players.Add(player);
                hubProxies[i].ResetConnection();
            }
            return players;
        }

        protected void NotifyResponseArrived()
        {
            waitHandle.Set();
        }

        protected bool DidResponseArrived()
        {
            return DidResponseArrived(TimeSpan.FromSeconds(15));
        }

        protected bool DidResponseArrived(TimeSpan timeout)
        {
            return waitHandle.WaitOne(timeout);
        }
    }
}