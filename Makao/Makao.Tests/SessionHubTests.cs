using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using Makao.Models;

namespace Makao.Tests
{
    [TestClass]
    public class SessionHubTests
    {
        IHubProxy proxy;
        string endpoint = "http://localhost:49642/"; // "http://makao.azurewebsites.net"
        string sessionID;
        bool requestSent = false;
        bool stop = false;

        [TestMethod]
        public void ConnectionTest()
        {
            var connection = new HubConnection(endpoint);
            connection.StateChanged += Connection_StateChanged;
            proxy = connection.CreateHubProxy("SessionHub");
            proxy.On<Player, bool>("ConnectResponse", (player, success)=>
            {
                sessionID = player.SessionId;
                Assert.IsNull(player);
                requestSent = false;
            });
            connection.Start();
            while (!stop) ;
        }

        private void Connection_StateChanged(StateChange state)
        {
            if (state.NewState == ConnectionState.Connected)
            {
                if (!requestSent)
                {
                    proxy.Invoke("Connect");
                    requestSent = true;
                }
            }
        }
    }
}
