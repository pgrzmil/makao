using Makao.Models;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Makao.Tests
{
    [TestClass]
    public class GameRoomTests : HubTests
    {
        const int numberOfPlayers = 4;

        private IList<GameRoom> gameRooms;
        private IList<Player> players;

        [TestInitialize]
        public override void PrepareForTests()
        {
            base.PrepareForTests(numberOfPlayers);

            ConnectMultiplePlayers();
            gameRooms = null;
        }

        [TestMethod]
        public void GetGameRoomsTest()
        {
            gameRooms = DefaultProxy.InvokeHubMethod<IList<GameRoom>>("GameRoomHub", "GetGameRooms");
            Assert.IsNotNull(gameRooms);
        }

        [TestMethod]
        public void EnterGameRoomTest()
        {
            var gameRoomId = "1";
            var status = false;
            gameRooms = new List<GameRoom> { new GameRoom(gameRoomId) };
            waitHandle = new AutoResetEvent(false);

            for (int i = 0; i < hubProxies.Count; i++)
            {
                status = hubProxies[i].InvokeHubMethod<bool>("GameRoomHub", "EnterGameRoom", (proxy) =>
                {
                    proxy.On<GameRoom, Player>("PlayerEnteredRoom", (gameRoom, newPlayer) =>
                    {
                        gameRooms[0] = gameRoom;

                        if (gameRoom.Players.Count == numberOfPlayers)
                            waitHandle.Set();
                    });
                }, players[i].SessionId, gameRoomId);

                if (!status)
                    waitHandle.Set();
            }

            Assert.IsTrue(waitHandle.WaitOne(30 * 1000), "Request was canceled");
            Assert.IsTrue(status, "Players weren't added to room");
            Assert.IsNotNull(gameRooms.FirstOrDefault(), "Game room wasn't returned");
            Assert.IsTrue(gameRooms.FirstOrDefault().GameRoomId == gameRoomId, "Players weren't added to correct game room");
            Assert.IsTrue(gameRooms.FirstOrDefault().Players.Count == numberOfPlayers, "Number of players added to game room is different than number specified in test");
            foreach (var player in players)
            {
                Assert.IsTrue(gameRooms.FirstOrDefault().Players.Count(p => p.Name == player.Name) != 0, "Not all players were added to game room");
            }
        }

        [TestMethod]
        public void ConnectMultiplePlayers()
        {
            players = new List<Player>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                var player = hubProxies[i].InvokeHubMethod<Player>("SessionHub", "Connect");
                this.players.Add(player);
                hubProxies[i].ResetConnection();
            }

            Assert.IsNotNull(players);
            Assert.IsTrue(players.Count == numberOfPlayers);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}