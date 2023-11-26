using Bluff.Client.Services.Servers;
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
    }
}
