using Makao.Hub.Models;
using Makao.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Makao.Tests.ModelsTests
{
    [TestClass]
    public class DeckTests
    {
        Random rand = new Random();

        [TestMethod]
        public void Deck_CreateDeck_Test()
        {
            GameRoomMock gameRoom = PrepareGameRoom();
            gameRoom.PopulateDeckMock();

            Assert.IsNotNull(gameRoom.Deck);
            Assert.AreEqual(52, gameRoom.Deck.Cards.Count);
        }

        [TestMethod]
        public void Deck_Shuffle_Test()
        {
            GameRoomMock gameRoom = PrepareGameRoom();

            var ranks = Enum.GetValues(typeof(Models.CardRanks)).Cast<Models.CardRanks>();
            var numberOfCardsWithSameSuit = gameRoom.Deck.Cards.Take(ranks.Count()).Count(c => c.Suit == gameRoom.Deck.Cards.First().Suit);

            Assert.AreNotEqual(ranks.Count(), numberOfCardsWithSameSuit);
        }

        [TestMethod]
        public void Deck_CreateDeckFromCollection_Test()
        {
            GameRoomMock gameRoom = PrepareGameRoom();

            var expectedCardsCount = rand.Next(2, gameRoom.Deck.Cards.Count - 2);
            var expectedCards = gameRoom.TakeCards(expectedCardsCount);

            gameRoom.Deck = new Deck(expectedCards);
            var actualCardsCount = gameRoom.Deck.Cards.Count;
            var actualCards = gameRoom.Deck.Cards;

            Assert.AreEqual(expectedCardsCount, actualCardsCount);
            for (int i = 0; i < expectedCardsCount; i++)
            {
                Assert.IsTrue(actualCards.Contains(expectedCards[i]));
            }
        }

        [TestMethod]
        public void Deck_TakeCard_Test()
        {
            GameRoomMock gameRoom = PrepareGameRoom();

            var expectedCard = gameRoom.Deck.Cards.First();
            var expectedLeftCardsCount = gameRoom.Deck.Cards.Count - 1;
            var expectedNextCard = gameRoom.Deck.Cards[1];

            var actualCard = gameRoom.TakeCards().First();
            var actualLeftCardsCount = gameRoom.Deck.Cards.Count;
            var actualNextCard = gameRoom.TakeCards().First();

            Assert.AreSame(expectedCard, actualCard);
            Assert.AreEqual(expectedLeftCardsCount, actualLeftCardsCount);
            Assert.AreSame(expectedNextCard, actualNextCard);
        }

        [TestMethod]
        public void Deck_TakeCards_Test()
        {
            GameRoomMock gameRoom = PrepareGameRoom();

            var numberOfCardsToTake = rand.Next(2, gameRoom.Deck.Cards.Count);

            var expectedTakenCards = gameRoom.Deck.Cards.Take(numberOfCardsToTake).ToList();
            var expectedLeftCardsCount = gameRoom.Deck.Cards.Count - numberOfCardsToTake;
            var expectedNextCard = gameRoom.Deck.Cards[numberOfCardsToTake];

            var actualTakenCards = gameRoom.TakeCards(numberOfCardsToTake);
            var actualLeftCardsCount = gameRoom.Deck.Cards.Count;
            var actualNextCard = gameRoom.TakeCards().First();

            Assert.AreEqual(expectedTakenCards.Count, actualTakenCards.Count, "Number of cards returned is incorrect");
            for (int i = 0; i < numberOfCardsToTake; i++)
            {
                Assert.AreSame(expectedTakenCards[i], actualTakenCards[i], "Cards taken are not from begining of collection");
            }
            Assert.AreEqual(expectedLeftCardsCount, actualLeftCardsCount);
            Assert.AreSame(expectedNextCard, actualNextCard);
        }

        [TestMethod]
        public void Deck_EmptyDeck_Test()
        {
            GameRoomMock gameRoom = PrepareGameRoom();

            var expectedTakenCardsCount = gameRoom.Deck.Cards.Count;
            var actualTakenCardsCount = gameRoom.TakeCards(expectedTakenCardsCount).Count;

            var wasExceptionThrown = false;
            try
            {
                gameRoom.TakeCards();
            }
            catch (NotEnoughCardsException)
            {
                wasExceptionThrown = true;
                var expectedLeftCardsCount = 0;
                var actualLeftCardsCount = gameRoom.Deck.Cards.Count;
                Assert.AreEqual(expectedLeftCardsCount, actualLeftCardsCount);
            }

            Assert.IsTrue(wasExceptionThrown);
            Assert.AreEqual(expectedTakenCardsCount, actualTakenCardsCount);
        }

        private static GameRoomMock PrepareGameRoom()
        {
            var game = new GameRoom { GameRoomId = "1" };
            GameRoomMock gameRoom = null;
            gameRoom = new GameRoomMock(game);

            gameRoom.AddPlayer(new Player("PlayerName1"));
            gameRoom.AddPlayer(new Player("PlayerName2"));
            gameRoom.Start();
            return gameRoom;
        }
    }
}