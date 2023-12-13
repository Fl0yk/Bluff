using Bluff.Client.Services.Servers;
using Bluff.Domain;
using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Common;

namespace Bluff.Client.Services.InGame
{
    public class InGameHubService : BaseConnectionService, IInGameHubService
    {
        public InGameHubService(IConfiguration config)
        {
            string hubUrl = config.GetSection("InGameHubUrl").Value
                    ?? throw new KeyNotFoundException("Не удалось получить url хаба игр");

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        }

        public async Task<bool> ConnectToServerRequest(string username, string groupName)
        {
            try
            {
                await _connection.SendAsync("UserConnected", username, groupName);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при подключении к хабу: {ex.Message}";
                return false;
            }
        }

        public void CreateConnection(string method, Action<Domain.Client> handler)
        {
            _connection.On(method, handler);
        }

        public void CreateConnection(string method, Action<List<Domain.Client>> handler)
        {
            _connection.On(method, handler);
        }

        public void CreateConnection(string method, Action<int> handler)
        {
            _connection.On(method, handler);
        }

        public void CreateConnection(string method, Action<Bet, string> handler)
        {
            _connection.On(method, handler);
        }

        public void CreateConnection(string method, Action handler)
        {
            _connection.On(method, handler);
        }

        public async Task<bool> UserReadyRequest(string groupName)
        {
            try
            {
                await _connection.SendAsync("UserReady", groupName);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при подключении к хабу: {ex.Message}";
                return false;
            }
        }

        public async Task<bool> GetClientsRequest(string groupName)
        {
            try
            {
                await _connection.SendAsync("GetAllClients", groupName);
                return true;
            }
            catch (Exception ex) 
            {
                ErrorMessage = $"Ошибка при подключении к хабу: {ex.Message}";
                return false;
            }
        }

        public async Task<bool> PlaceABetRequest(string groupName, string username, int cubeVal, int count, int id)
        {
            Bet bet = new() { CubeValue = cubeVal, Count = count, CellId = id };

            try
            {
                await _connection.SendAsync("PlaceABet", groupName, username, bet);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при подключении к хабу: {ex.Message}";
                return false;
            }
        }

        public async Task<bool> Dispute(string groupName, string username)
        {
            try
            {
                await _connection.SendAsync("ChallengeBet", groupName, username);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при подключении к хабу: {ex.Message}";
                await Console.Out.WriteLineAsync(ErrorMessage);
                return false;
            }
        }
    }
}
