using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Makao.Tests
{
    [TestClass]
    public class SessionHubTests : HubTests
    {
        private Player player;

        [TestInitialize]
        public override void PrepareForTests()
        {
            base.PrepareForTests();
            player = null;
        }

        [TestMethod]
        public void ConnectionTest()
        {
            InvokeHubMethod<Player, bool>("SessionHub", "Connect", "ConnectResponse", (playr, success) =>
            {
                this.player = playr;
            });

            Assert.IsNotNull(player);
        }
    }
}