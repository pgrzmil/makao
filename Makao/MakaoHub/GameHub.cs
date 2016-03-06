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
        public GameHub()
        {

        }

        static GameHub()
        {
            SharedData.AwaitingPlayers = new List<Player>();
            SharedData.Games = new List<Game>();
        }

        public void ConnectPlayer(string name)
        {
            var player = new Player(name) { ConnectionId = Context.ConnectionId };
            SharedData.AwaitingPlayers.Add(player);
            Clients.Caller.SetPlayerId(player.PlayerId);

            TryCreateGame();
        }

        public void DisconnectPlayer(Guid playerId)
        {
            var playerToRemove = SharedData.AwaitingPlayers.First(x => x.PlayerId == playerId);
            if (playerToRemove != null)
                SharedData.AwaitingPlayers.Remove(playerToRemove);
        }

        private void TryCreateGame()
        {

        }

        public void PutCardOnStack(Guid playerId, Card card)
        {

        }

        public void SendMessage(string message)
        {
            Clients.All.AddMessage(message);
        }
    }
}