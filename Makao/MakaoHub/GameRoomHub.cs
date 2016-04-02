using Makao.Data;
using Makao.Hub.Models;
using Makao.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Makao.Hub
{
    public class GameRoomHub : BaseHub
    {
        public GameRoomHub()
        {
        }

        public IList<GameRoomModel> GetGameRooms()
        {
            //IDEA: do we need to check sessionId?
            return SharedData.GameRooms;
        }

        public bool EnterGameRoom(string sessionId, string gameRoomId)
        {
            var status = false;
            var gameRoom = SharedData.GameRooms.Cast<GameRoom>().FirstOrDefault(g => g.GameRoomId == gameRoomId);
            var player = SharedData.Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (gameRoom != null && player != null && gameRoom.Players.Count < gameRoom.NumberOfPlayers)
            {
                UpdateConnectionId(player);

                LeaveGameRoom(sessionId);
                gameRoom.AddPlayer(player);

                Groups.Add(player.ConnectionId, gameRoom.GameRoomId).Wait();
                Clients.Group(gameRoom.GameRoomId).PlayerEnteredRoom(gameRoom, player);
                status = true;
            }

            return status;
        }

        public void LeaveGameRoom(string sessionId)
        {
            var gameRooms = SharedData.GameRooms.Cast<GameRoom>().Where(g => g.Players.Count(p => p.SessionId == sessionId) != 0).ToList();
            var player = SharedData.Players.FirstOrDefault(p => p.SessionId == sessionId);

            foreach (var gameRoom in gameRooms)
            {
                gameRoom.RemovePlayer(player);

                UpdateConnectionId(player);
                Groups.Remove(player.ConnectionId, gameRoom.GameRoomId).Wait();
                Clients.Group(gameRoom.GameRoomId).PlayerLeftRoom(gameRoom);
            }
        }

        //TODO: Add method to change name
        //TODO: Add method to change number of players
        //TODO: Add method to change time for move

        public bool SetPlayerReady(string sessionId, string gameRoomId, bool isReady)
        {
            var status = false;
            var gameRoom = SharedData.GameRooms.Cast<GameRoom>().FirstOrDefault(g => g.GameRoomId == gameRoomId);
            var player = SharedData.Players.FirstOrDefault(p => p.SessionId == sessionId);

            if (gameRoom != null && player != null)
            {
                player.IsReady = isReady;
                status = true;

                UpdateConnectionId(player);
                Clients.Group(gameRoom.GameRoomId).SetPlayerReadyResponse(gameRoom);

                TryStartGame(gameRoom);
            }
            return status;
        }

        private Task TryStartGame(GameRoom gameRoom)
        {
            return new Task(() =>
            {
                var isEveryPlayerReady = gameRoom.Players.Count(p => p.IsReady) == gameRoom.NumberOfPlayers;

                if (isEveryPlayerReady)
                {
                    gameRoom.Start();
                    Clients.Group(gameRoom.GameRoomId).NotifyGameStart(gameRoom);
                }
            });
        }

        public bool PlayCard(string sessionId, string gameRoomId, Card card)
        {
            var status = false;
            var gameRoom = SharedData.GameRooms.Cast<GameRoom>().FirstOrDefault(g => g.GameRoomId == gameRoomId);
            gameRoom.GameOver += (winner) =>
            {
                Clients.Group(gameRoom.GameRoomId).GameOver(winner);
                gameRoom.Reset();
            };

            if (gameRoom != null)
            {
                status = gameRoom.PlayCard(sessionId, card);

                Clients.Group(gameRoom.GameRoomId).PlayerPlayedCard(gameRoom);
            }

            return status;
        }

        public bool TakeCard(string sessionId, string gameRoomId)
        {
            var status = false;
            var gameRoom = SharedData.GameRooms.Cast<GameRoom>().FirstOrDefault(g => g.GameRoomId == gameRoomId);

            if (gameRoom != null)
            {
                status = gameRoom.GiveCardsToPlayer(sessionId);
                Clients.Group(gameRoom.GameRoomId).PlayerTookCard(gameRoom);
            }
            return status;
        }
    }
}