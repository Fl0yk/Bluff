using Bluff.Domain;
using Bluff.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class InGameHub : Hub
    {
        IGroupService _groupService;
        public InGameHub(IGroupService groupService) : base()
        {
            _groupService = groupService;
        }

        /// <summary>
        /// Метод добавляет клиента в список игроков и запускает процесс начала игры
        /// </summary>
        /// <param name="userName">Имя клиента, который подключается к игре</param>
        /// <param name="groupName">Название группы к которой подключается клиент</param>

        public async Task UserConnected(string userName, string groupName)
        {
            var client = new Client { Name = userName, Id = Context.ConnectionId };

            //  ДОБАВИТЬ ПРОВЕРКУ НА ДОПУСТИМОСТЬ ВВЕДЕНОГО ИМЕНИ ПОЛЬЗОВАТЕЛЯ
            _groupService.AddUserToGame(groupName, client);

            // если игроков достаточно для начала - начинаем игру
            // иначе - отправляем всем пользователем подключенных игроков для отрисовки
            if (_groupService.IsGameReady(groupName))
            {
                // ДЛЯ МИХИ - метод HandleGameStart должен спросить у игроков, готовы ли они начать
                // если все готовы - то должен вызвать метод НАЗВАНИЕ МЕТОДА передав в него АРГУМЕНТЫ
                await Clients.All.SendAsync("HandleGameStart", client);
            }
            else
            {
                await Clients.All.SendAsync("UserConnected", client);
            }
        }
    }
}
