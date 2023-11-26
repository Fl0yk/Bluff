using Bluff.Domain;
using Bluff.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class InGameHub : Hub
    {
        private readonly string exceptionHandler;
        private readonly IGroupService _groupService;
        public InGameHub(IGroupService groupService, IConfiguration config) : base()
        {
            _groupService = groupService;
            exceptionHandler = config.GetSection("ExHandleMethod")
                           .Value ?? throw new KeyNotFoundException("Не удалось получить название метода для обработок ошибок на стороне клиента");
        }

        /// <summary>
        /// Метод добавляет клиента в список игроков и запускает процесс начала игры
        /// </summary>
        /// <param name="userName">Имя клиента, который подключается к игре</param>
        /// <param name="groupName">Название группы к которой подключается клиент</param>
        public async Task UserConnected(string userName, string groupName)
        {
            // проверка существования игры
            if (_groupService.GetGameByName(groupName) is null)
                await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "несуществующая игра");

            var client = new Client(Context.ConnectionId, userName, groupName);

            //  проверка уникальности имени пользователя
            if (!_groupService.IsExistClientInGame(groupName, userName))
                _groupService.AddUserToGame(groupName, client);
            else
                await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "недействительное имя пользователя");


            // если игроков достаточно для начала - начинаем игру
            // иначе - отправляем всем пользователем подключенных игроков для отрисовки
            if (_groupService.IsGameReady(groupName))
            {
                // ДЛЯ МИХИ - метод HandleGameStart должен спросить у игроков, готовы ли они начать
                // если все готовы - то должен вызвать метод НАЗВАНИЕ МЕТОДА передав в него АРГУМЕНТЫ
                await Clients.All.SendAsync("HandleUserReadyCheck", client);
            }
            else
            {
                await Clients.All.SendAsync("HandleUserConnected", client);
            }
        }

        /// <summary>
        /// Метод обрабатывает готовность пользователя к игре и начало игры
        /// </summary>
        /// <param name="groupName">Название группы в которой клиент нажал готов</param>
        public async Task UserReady(string groupName)
        {
            // проверка на существование игры
            var game = _groupService.GetGameByName(groupName);
            if (game is null)
                throw new Exception("нет такой игры");

            // увеличение количества клиентов нажавших готов
            game.ReadyUsers++;
            // начало игры, если все пользователи нажали готов
            // иначе - просто обработка готовности
            if (game.ReadyUsers == game.UserToStart)
            {
                // рандомим кубики
                RandomUserCubes(game);

                // начинаем игру
                await Clients.All.SendAsync("HandleGameStart", game.Clients.First());
            }
            else 
                // возвращаем количество готовых пользователей
                await Clients.All.SendAsync("HandleUserReady", game.ReadyUsers);
        }

        private void RandomUserCubes(Game game)
        {
            Random random = new Random();
            // перебираем всех клиентов
            foreach (var client in game.Clients)
            {
                // рандомим для них значения кубиков
                for (int i = 0; i < 5; i++)
                    // индекс - значение выпавшее на кубике
                    // значение по индексу - сколько раз он выпал
                    client.Cubes[random.Next() % 6]++;
            }
        }

        public async Task GetAllClients(string groupName)
        {
            // проверка существования игры
            var game = _groupService.GetGameByName(groupName);
            if (game is null)
                await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "несуществующая игра");

            // отправка всех клиентов на фронт
            await Clients.Client(Context.ConnectionId).SendAsync("HandleGetAllClients", game!.Clients);
        }
    }
}
