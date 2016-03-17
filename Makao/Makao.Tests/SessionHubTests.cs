using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Makao.Tests
{
    [TestClass]
    public class SessionHubTests : HubTests
    {
        [TestInitialize]
        public override void PrepareForTests()
        {
            base.PrepareForTests();
        }

        [TestMethod]
        public void ConnectPlayerTest()
        {
            Player player = null;
            player = DefaultProxy.InvokeHubMethod<Player>("SessionHub", "Connect");

            Assert.IsNotNull(player);
        }
    }
}