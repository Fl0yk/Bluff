using Bluff.Domain;
using Bluff.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class ServerHub : Hub
    {
        private IGroupService _groupService;
        private string exHandleMethodName;
        public ServerHub(IGroupService groupService, IConfiguration config) : base()
        {
            _groupService = groupService;
            exHandleMethodName = config.GetSection("ExHandleMethod")
                           .Value ?? throw new KeyNotFoundException("Не удалось получить назваание методаа для обрааботок ошибок на стороне клиента");
        }

        /// <summary>
        /// Метод добавляет клиента в группу. Также сохраняет название созданной группы
        /// </summary>
        /// <param name="userName">Имя клиента, который создает группу</param>
        public async Task CreateGroup(string groupName, int userToStart)
        {
            //if(_groupService.IsExistClientInGame(groupName, userName))
            //{
            //    await Clients.Client(Context.ConnectionId).SendAsync("CatchErrorMessage", "Игрок с таким именем уже существует в этом лобби");
            //    return;
            //}

            if(_groupService.GetGameByName(groupName) is not null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync(exHandleMethodName, "Лобби с таким именем уже существует");
                return;
            }

            _groupService.CreateGame(groupName, userToStart);
            await Clients.All.SendAsync("GetServerList", _groupService.GetNamesOfAllGames());
        }

        /// <summary>
        /// Получение списка всех групп, созданных на данный момент
        /// </summary>
        public async Task GetServerList()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("GetServerList", _groupService.GetNamesOfAllGames());
        }
    }
}