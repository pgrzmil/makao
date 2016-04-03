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
    public class GameRoomHubTests : HubTests
    {
        const int numberOfPlayers = 4;

        private IList<GameRoom> gameRooms;
        private IList<Player> players;

        [TestInitialize]
        public override void PrepareForTests()
        {
            base.PrepareForTests(numberOfPlayers);

            players = ConnectMultiplePlayers(numberOfPlayers);
            gameRooms = null;
        }

        [TestMethod]
        public void GameRoomHub_GetGameRooms_Test()
        {
            gameRooms = DefaultProxy.InvokeHubMethod<IList<GameRoom>>("GameRoomHub", "GetGameRooms");
            Assert.IsNotNull(gameRooms);
        }

        [TestMethod]
        public void GameRoomHub_EnterGameRoom_Test()
        {
            var gameRoomId = "1";
            var status = false;
            gameRooms = new List<GameRoom> { new GameRoom { GameRoomId = gameRoomId, NumberOfPlayers = 4, MoveTime = 10 } };

            for (int i = 0; i < hubProxies.Count; i++)
            {
                status = hubProxies[i].InvokeHubMethod<bool>("GameRoomHub", "EnterGameRoom", (proxy) =>
                {
                    proxy.On<GameRoom, Player>("PlayerEnteredRoom", (gameRoom, newPlayer) =>
                    {
                        gameRooms[0] = gameRoom;

                        if (gameRoom.Players.Count == numberOfPlayers)
                            NotifyResponseArrived();
                    });
                }, players[i].SessionId, gameRoomId);

                if (!status)
                    NotifyResponseArrived();
            }

            Assert.IsTrue(DidResponseArrived(), "Request was canceled");
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
        public void GameRoomHub_LeaveRoom_Test()
        {
        }

        [TestMethod]
        public void GameRoomHub_SetReady_Test()
        {
        }

        [TestMethod]
        public void GameRoomHub_StartGame_Test()
        {
        }

        [TestMethod]
        public void GameRoomHub_PlayCard_Test()
        {
        }

        [TestMethod]
        public void GameRoomHub_PlayGameToEnd_Test()
        {
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}