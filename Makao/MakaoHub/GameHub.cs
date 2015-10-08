using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Makao.GameItems;
using Makao.Models;
using Player = Makao.GameItems.Player;

namespace MakaoHub
{
    public class GameHub : Hub
    {
        private static IList<Player> AwaitingPlayers;
        private static IList<Game> Games;

        public GameHub()
        {

        }

        static GameHub()
        {
            AwaitingPlayers = new List<Player>();
            Games = new List<Game>();
        }

        public void ConnectPlayer(string name)
        {
            var player = new Player(name) { ConnectionId = Context.ConnectionId };
            AwaitingPlayers.Add(player);
            Clients.Caller.SetPlayerId(player.PlayerId);

            TryCreateGame();
        }

        public void DisconnectPlayer(Guid playerId)
        {
            var playerToRemove = AwaitingPlayers.First(x => x.PlayerId == playerId);
            if (playerToRemove != null)
                AwaitingPlayers.Remove(playerToRemove);
        }

        private void TryCreateGame()
        {

        }

        public void PutCardOnStack(Guid playerId, Card card)
        {

        }

        public void Send(string message)
        {
            Clients.All.AddMessage(message);
        }
    }
}