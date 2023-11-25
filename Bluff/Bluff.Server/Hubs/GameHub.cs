using Bluff.Domain;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class GameHub : Hub
    {
        private List<Client> clients = new();

        //Если этот хаб контролирует работу с серверами - тогда это и поле ниже не надо
        private Client? betAuthor;
        
        //его тоже не надо
        private Bet? bet;

        private List<string> groups = new();

        /// <summary>
        /// Метод добавляет клиента в группу. Также сохраняет название созданной группы
        /// </summary>
        /// <param name="userName">Имя клиента, который создает группу</param>
        public async Task CreateGroup(string userName, string groupName)
        {
            clients.Add(new Client { Name = userName, Id = Context.ConnectionId });
            groups.Add(groupName);
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