using Bluff.Domain;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class ServerHub : Hub
    {
        private List<string> groups = new();

        /// <summary>
        /// Метод добавляет клиента в группу. Также сохраняет название созданной группы
        /// </summary>
        /// <param name="userName">Имя клиента, который создает группу</param>
        public async Task CreateGroup(string userName, string groupName)
        {
            
        }
        /// <summary>
        /// Получение списка всех групп, созданных на данный момент
        /// </summary>
        public async Task GetServerList()
        {
            await Clients.All.SendAsync("GetServerList", groups);
        }
    }
}