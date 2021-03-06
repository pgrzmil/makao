﻿using Makao.Models;
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
            var deck = new DeckMock();

            Assert.IsNotNull(deck);
            Assert.AreEqual(52, deck.GetCards().Count);
        }

        [TestMethod]
        public void Deck_Shuffle_Test()
        {
            var deck = new DeckMock();

            var ranks = Enum.GetValues(typeof(Models.CardRanks)).Cast<Models.CardRanks>();
            var numberOfCardsWithSameSuit = deck.GetCards().Take(ranks.Count()).Count(c => c.Suit == deck.GetCards().First().Suit);

            Assert.AreNotEqual(ranks.Count(), numberOfCardsWithSameSuit);
        }

        [TestMethod]
        public void Deck_CreateDeckFromCollection_Test()
        {
            var deck = new DeckMock();

            var expectedCardsCount = rand.Next(2, deck.GetCards().Count - 2);
            var expectedCards = deck.TakeCards(expectedCardsCount);

            deck = new DeckMock(expectedCards);
            var actualCardsCount = deck.GetCards().Count;
            var actualCards = deck.GetCards();

            Assert.AreEqual(expectedCardsCount, actualCardsCount);
            for (int i = 0; i < expectedCardsCount; i++)
            {
                Assert.IsTrue(actualCards.Contains(expectedCards[i]));
            }
        }

        [TestMethod]
        public void Deck_TakeCard_Test()
        {
            var deck = new DeckMock();

            var expectedCard = deck.GetCards().First();
            var expectedLeftCardsCount = deck.GetCards().Count - 1;
            var expectedNextCard = deck.GetCards()[1];

            var actualCard = deck.TakeCards().First();
            var actualLeftCardsCount = deck.GetCards().Count;
            var actualNextCard = deck.TakeCards().First();

            Assert.AreSame(expectedCard, actualCard);
            Assert.AreEqual(expectedLeftCardsCount, actualLeftCardsCount);
            Assert.AreSame(expectedNextCard, actualNextCard);
        }

        [TestMethod]
        public void Deck_TakeCards_Test()
        {
            var deck = new DeckMock();

            var numberOfCardsToTake = rand.Next(2, deck.GetCards().Count);

            var expectedTakenCards = deck.GetCards().Take(numberOfCardsToTake).ToList();
            var expectedLeftCardsCount = deck.GetCards().Count - numberOfCardsToTake;
            var expectedNextCard = deck.GetCards()[numberOfCardsToTake];

            var actualTakenCards = deck.TakeCards(numberOfCardsToTake);
            var actualLeftCardsCount = deck.GetCards().Count;
            var actualNextCard = deck.TakeCards().First();

            Assert.AreEqual(expectedTakenCards.Count, actualTakenCards.Count, "Number of cards returned is incorrect");
            for (int i = 0; i < numberOfCardsToTake; i++)
            {
                Assert.AreSame(expectedTakenCards[i], actualTakenCards[i], "GetCards() taken are not from begining of collection");
            }
            Assert.AreEqual(expectedLeftCardsCount, actualLeftCardsCount);
            Assert.AreSame(expectedNextCard, actualNextCard);
        }

        [TestMethod]
        public void Deck_EmptyDeck_Test()
        {
            var deck = new DeckMock();

            var expectedTakenCardsCount = deck.GetCards().Count;
            var actualTakenCardsCount = deck.TakeCards(expectedTakenCardsCount).Count;

            var wasExceptionThrown = false;
            try
            {
                deck.TakeCards();
            }
            catch (NotEnoughCardsException)
            {
                wasExceptionThrown = true;
                var expectedLeftCardsCount = 0;
                var actualLeftCardsCount = deck.GetCards().Count;
                Assert.AreEqual(expectedLeftCardsCount, actualLeftCardsCount);
            }

            Assert.IsTrue(wasExceptionThrown);
            Assert.AreEqual(expectedTakenCardsCount, actualTakenCardsCount);
        }
    }
}