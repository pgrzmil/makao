using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Makao.Tests
{
    [TestClass]
    public class SessionHubTests : HubTests
    {
        [TestMethod]
        public void SessionHub_ConnectPlayer_Test()
        {
            PrepareForTests();

            Player player = null;
            player = DefaultProxy.InvokeHubMethod<Player>("SessionHub", "Connect");

            Assert.IsNotNull(player);
        }

        [TestMethod]
        public void SessionHub_ConnectMultiplePlayers_Test()
        {
            var numberOfPlayers = 4;
            PrepareForTests(numberOfPlayers);

            var players = ConnectMultiplePlayers(numberOfPlayers);

            Assert.IsNotNull(players);
            Assert.IsTrue(players.Count() == numberOfPlayers);
        }
    }
}