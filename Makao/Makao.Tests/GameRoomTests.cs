using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Makao.Tests
{
    [TestClass]
    public class GameRoomTests : HubTests
    {
        private IList<GameRoom> gameRooms;
        private Player player;

        [TestInitialize]
        public override void PrepareForTests()
        {
            base.PrepareForTests();
            ConnectPlayer();
            gameRooms = null;
        }

        [TestMethod]
        public void GetGameRoomsTest()
        {
            InvokeHubMethod<IList<GameRoom>>("GameRoomHub", "GetGameRooms", "GetGameRoomsResponse", (gameRoomsList) =>
            {
                gameRooms = gameRoomsList;
            }, player.SessionId);

            Assert.IsNotNull(gameRooms);
        }

        private void ConnectPlayer()
        {
            InvokeHubMethod<Player, bool>("SessionHub", "Connect", "ConnectResponse", (playr, success) =>
            {
                this.player = playr;
            });
            ResetConnection();
        }
    }
}