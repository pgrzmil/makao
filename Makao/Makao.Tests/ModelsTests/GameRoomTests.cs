using Makao.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Makao.Tests.ModelsTests
{
    [TestClass]
    public class GameRoomTests
    {
        Random rand = new Random();

        [TestMethod]
        public void GameRoom_CreateGameRoom_Test()
        {
            var expectedGameRoomId = "expectedGameRoomId";
            var gameRoom = new GameRoom { GameRoomId = expectedGameRoomId };

            var actualGameRoomId = gameRoom.GameRoomId;

            Assert.IsNotNull(gameRoom);
            Assert.AreEqual(expectedGameRoomId, actualGameRoomId);
        }

        [TestMethod]
        public void GameRoom_AddPlayer_Test()
        {
            var gameRoom = new GameRoom { GameRoomId = "1" };

            var expectedPlayer = new Player("PlayerName");

            gameRoom.AddPlayer(expectedPlayer);

            var actualPlayer = gameRoom.Players.First();

            Assert.AreSame(expectedPlayer, actualPlayer);
        }

        [TestMethod]
        public void GameRoom_AddMorePlayersThanMaximum_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var randNumber = rand.Next(1, 5);
            var addedPlayers = new List<Player>();
            for (int i = 0; i < gameRoom.NumberOfPlayers + randNumber; i++)
            {
                var player = new Player("PlayerName" + i);
                addedPlayers.Add(player);
                gameRoom.AddPlayer(player);
            }

            var expectedNumberOfPlayers = gameRoom.NumberOfPlayers;
            var expectedPlayersCollection = addedPlayers.Take(gameRoom.NumberOfPlayers).ToList();

            var actualPlayersCollection = gameRoom.Players;
            var actualNumberOfPlayers = gameRoom.Players.Count;

            Assert.AreEqual(expectedNumberOfPlayers, actualNumberOfPlayers);

            for (int i = 0; i < actualPlayersCollection.Count; i++)
            {
                Assert.AreSame(expectedPlayersCollection[i], actualPlayersCollection[i]);
            }
        }

        [TestMethod]
        public void GameRoom_RemovePlayer_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var numberOfPlayers = rand.Next(1, gameRoom.NumberOfPlayers);
            AddPlayers(gameRoom, numberOfPlayers);

            var expectedRemovedPlayer = gameRoom.Players.First();
            var expectedNumberOfPlayers = numberOfPlayers - 1;

            gameRoom.RemovePlayer(expectedRemovedPlayer);

            var actualNumberOfPlayers = gameRoom.Players.Count;

            Assert.AreEqual(expectedNumberOfPlayers, actualNumberOfPlayers);
            Assert.IsFalse(gameRoom.Players.Contains(expectedRemovedPlayer));
        }

        [TestMethod]
        public void GameRoom_StartGameWithoutPlayers_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            gameRoom.Start();

            Assert.IsFalse(gameRoom.IsRunning);
        }

        [TestMethod]
        public void GameRoom_StartGame_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var numberOfPlayers = rand.Next(2, gameRoom.NumberOfPlayers);
            AddPlayers(gameRoom, numberOfPlayers);

            gameRoom.Start();

            var expectedNumberOfCardsOnStack = 1;
            var expectedNumberOfCardsInHand = 5;

            var actualNumberOfCardsOnStack = gameRoom.Stack.Count;

            Assert.IsTrue(gameRoom.IsRunning);
            Assert.IsNotNull(gameRoom.Deck);
            Assert.AreEqual(expectedNumberOfCardsOnStack, actualNumberOfCardsOnStack);
            foreach (var player in gameRoom.Players)
            {
                var actualNumberOfCardsInHand = player.Hand.Count;
                Assert.AreEqual(expectedNumberOfCardsInHand, actualNumberOfCardsInHand);
            }
        }

        [TestMethod]
        public void GameRoom_DealCards_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var numberOfPlayers = rand.Next(2, gameRoom.NumberOfPlayers);
            AddPlayers(gameRoom, numberOfPlayers);
            gameRoom.Start();

            var expectedNumberOfCardsLeftInDeck = 52 - (5 * numberOfPlayers) - 1;
            var actualNumberOfCardsLeftInDeck = (gameRoom.Deck as DeckMock).GetCards().Count;

            Assert.AreEqual(expectedNumberOfCardsLeftInDeck, actualNumberOfCardsLeftInDeck);

            foreach (var player in gameRoom.Players)
            {
                foreach (var card in player.Hand)
                {
                    Assert.IsFalse((gameRoom.Deck as DeckMock).GetCards().Contains(card));
                }
            }
        }

        [TestMethod]
        public void GameRoom_PlayCard_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var numberOfPlayers = rand.Next(2, gameRoom.NumberOfPlayers);
            AddPlayers(gameRoom, numberOfPlayers);
            gameRoom.Start();

            var currentPlayer = gameRoom.CurrentPlayer();
            var currentPlayerIndex = gameRoom.CurrentPlayerIndex;
            var playedCard = currentPlayer.Hand.First();

            var expectedHand = currentPlayer.Hand.Where(c => c != playedCard).ToList();
            var expectedNumberOfCardsOnStack = gameRoom.Stack.Count + 1;

            var result = gameRoom.PlayCard(currentPlayer.SessionId, playedCard);

            var actualHand = currentPlayer.Hand;
            var actualNumberOfCardsOnStack = gameRoom.Stack.Count;

            Assert.AreNotEqual(currentPlayerIndex, gameRoom.CurrentPlayerIndex);
            Assert.AreEqual(expectedNumberOfCardsOnStack, actualNumberOfCardsOnStack);
            Assert.AreEqual(expectedHand.Count, actualHand.Count);
            foreach (var card in expectedHand)
            {
                Assert.IsTrue(actualHand.Contains(card));
            }
        }

        [TestMethod]
        public void GameRoom_UpdatePlayerIndex_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var numberOfPlayers = rand.Next(2, gameRoom.NumberOfPlayers);
            AddPlayers(gameRoom, numberOfPlayers);

            gameRoom.Start();
            var expectedCurrentPlayerIndex = gameRoom.CurrentPlayerIndex + 1 == numberOfPlayers ? 0 : gameRoom.CurrentPlayerIndex + 1;

            gameRoom.UpdateCurrentPlayerIndexMock();

            var actualCurrentPlayerIndex = gameRoom.CurrentPlayerIndex;

            Assert.AreEqual(expectedCurrentPlayerIndex, actualCurrentPlayerIndex);
        }

        [TestMethod]
        public void GameRoom_GameOver_Test()
        {
            var gameRoom = new GameRoomMock { GameRoomId = "1" };

            var numberOfPlayers = rand.Next(2, gameRoom.NumberOfPlayers);
            AddPlayers(gameRoom, numberOfPlayers);
            gameRoom.Start();

            var currentPlayer = gameRoom.CurrentPlayer();
            currentPlayer.Hand.RemoveRange(0, currentPlayer.Hand.Count - 1);

            gameRoom.GameOver += (winner) =>
            {
                Assert.AreSame(currentPlayer, winner);
                Assert.AreEqual(winner.Hand.Count, 0);
                Assert.IsFalse(gameRoom.IsRunning);
            };

            gameRoom.PlayCard(currentPlayer.SessionId, currentPlayer.Hand.First());
        }

        private void AddPlayers(GameRoomMock gameRoom, int numberOfPlayers)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                var player = new Player("PlayerName" + i);
                player.SessionId = player.Name;

                gameRoom.AddPlayer(player);
            }
        }
    }
}