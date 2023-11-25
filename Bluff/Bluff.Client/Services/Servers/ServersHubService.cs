using Microsoft.AspNetCore.SignalR.Client;

namespace Bluff.Client.Services.Servers
{
    public class ServersHubService : BaseConnectionService, IServersHubService
    {
        public List<string> Servers { get; set; }

        public ServersHubService(IConfiguration config) 
        {
            string hubUrl = config.GetSection("ServersHubUrl")
                            .Value ?? throw new KeyNotFoundException("Не удалось получить url хаба серверов");

            _connection = new HubConnectionBuilder()
                                .WithUrl(hubUrl)
                                .Build();

            Servers = new();
        }

        public async Task<bool> CreateGroup(string userName, int userToStart)
        {
            try
            {
                await _connection.InvokeAsync("CreateGroup", userName, userToStart);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при создании группы: " + ex.Message;
                return false;
            }
        }

        public async Task<bool> GetServerListRequest()
        {
            try
            {
                await _connection.InvokeAsync("GetServerList");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка при запросе групп: " + ex.Message;
                return false;
            }
        }
    }
}
