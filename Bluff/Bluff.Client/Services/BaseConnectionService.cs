using Microsoft.AspNetCore.SignalR.Client;

namespace Bluff.Client.Services
{
    public abstract class BaseConnectionService : IBaseConnectionService
    {
        protected HubConnection _connection = null!;
        public string ErrorMessage { get; set; } = string.Empty;

        //public ConnectionService(IConfiguration config)
        //{
        //    string hubUrl = config.GetSection("HubUrl").Value ?? throw new KeyNotFoundException("Не удалось получить url хаба");
        //    _connection = new HubConnectionBuilder()
        //                .WithUrl(hubUrl)
        //                .Build();

        //    Servers = new();
        //}

        public async Task<bool> ConnectToHub()
        {
            if (_connection.State == HubConnectionState.Connected)
                return true;

            try
            {
                // подключемся к хабу
                await _connection.StartAsync();
                //Console.WriteLine("Good connect");
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public void CreateConnection(string method, Action<string, string> handler)
        {
            _connection.On(method, handler);
        }

        public void CreateConnection(string method, Action<string> handler)
        {
            _connection.On(method, handler);
        }

        //public async Task<bool> CreateGroup(string username, string groupName)
        //{
        //    try
        //    {
        //        // отправка сообщения
        //        await _connection.InvokeAsync("CreateGroup", username, groupName);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Ошибка при создании сервера: " + ex.Message;
        //        return false;
        //    }
        //}
    }
}
