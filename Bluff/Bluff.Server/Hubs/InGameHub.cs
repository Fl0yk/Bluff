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

            //Добавляем клиента именно к группе SignalR
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // если игроков достаточно для начала - начинаем игру
            // иначе - отправляем всем пользователем подключенных игроков для отрисовки
            if (_groupService.IsGameReady(groupName))
            {
               
                await Clients.Groups(groupName).SendAsync("HandleUserReadyCheck", client);
            }
            else
            {
                await Clients.Groups(groupName).SendAsync("HandleUserConnected", client);
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

                // получаем того, кто будет начинать ход
                var turnBeginner = game.Clients[game.TurnBeginnerIndex];

                // начинаем игру
                await Clients.Groups(game.GroupName).SendAsync("HandleGameStart", turnBeginner);
            }
            else 
                // возвращаем количество готовых пользователей
                await Clients.Groups(game.GroupName).SendAsync("HandleUserReady", game.ReadyUsers);
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

        /// <summary>
        /// Метод для того, чтоб сделать ставку
        /// </summary>
        /// <param name="groupName">Название группы, из которой сделана ставка</param>
        /// <param name="username">Имя игрока, который сделал ставку</param>
        /// <param name="newBet">Ставка, которую сделал игрок</param>
        public async Task PlaceABet(string groupName, string username, Bet newBet)
        {
            Game? curGame = _groupService.GetGameByName(groupName);

            if (curGame is null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "несуществующая игра");
                return;
            }

            //Получаем индекс следующего игрока в коллекции и его имя
            int indexOfNextUser = (curGame.Clients.FindIndex(c => c.Name == username) + 1) % curGame.Clients.Count;
            string nextUser = curGame.Clients[indexOfNextUser].Name;

            //Сохраняем в игре новую ставку
            curGame.Bet = newBet;

            //Отправляем всем клиентам группы в метод GetNewBet сделанную ставку и имя следующего игрока
            await Clients.Groups(curGame.GroupName).SendAsync("GetNewBet", newBet, nextUser);
        }

        /// <summary>
        /// Метод, оспоривающий ставку
        /// </summary>
        /// <param name="groupName">Название группы, в которой оспаривается ставка</param>
        /// <param name="BetterUsername">Имя игрока, который сделал ставку</param>
        /// <param name="ChallengerUsername">Имя игрока, который оспаривает ставку</param>
        /// <param name="bet">Оспариваемая ставка</param>
        public async Task ChallengeBet(string groupName, string BetterUsername, string ChallengerUsername, Bet bet)
        {
            Game? curGame = _groupService.GetGameByName(groupName);

            if (curGame is null)
            {
                await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "несуществующая игра");
                return;
            }

            // число выпавших кубиков, указанных в ставке
            int appearedCubesNumber = 0;

            // Каждый клиент хранит в себе массив из 6 элементов,
            // индексом которого является значение выпавшее на кубике минус один, а по индексу
            // хранится количество выпавших кубиков с данным значением.
            // Пример: у игрока выпало три кубика со значением 1 и два кубика со звездой.
            // В массиве по 0 индексу будет лежать число 3, а по индексу 5 - 2

            // Подсчет количевства выпавших кубиков со значением, указанным в ставке
            foreach (var client in  curGame.Clients)
                appearedCubesNumber += client.Cubes[bet.CubeValue];
            
            int difference = appearedCubesNumber - bet.Count;

            // Определние того, кто будет терять кубики
            if (difference > 0)
            {
                // кубики теряет оспаривавший
                var challenger = curGame.Clients.Where(c => c.Name == ChallengerUsername).FirstOrDefault();
                if (challenger is null)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "несуществующее имя оспаривателя");
                    return;
                }

                // отнимаем кубики
                challenger.CubesCount -= difference;
            }
            else if (difference < 0) 
            {
                // кубики теряет ставивший
                var better = curGame.Clients.Where(c => c.Name == BetterUsername).FirstOrDefault();
                if (better is null)
                {
                    await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "несуществующее имя ставщика");
                    return;
                }

                // отнимаем кубики
                better.CubesCount -= difference;
            }
            else
            {
                // кубики теряют все кроме ставившего
                foreach (var client in curGame.Clients)
                    if (client.Name != BetterUsername)
                        client.CubesCount--;
            }


            // проверка на необходиость завершения игры(остался один игрок с кубиками)
            int playersLeft = 0;
            // Храним возможного победителя, чтобы его потом не искать
            Client? possibleWinner = null;

            foreach (var client in curGame.Clients)
            {
                if (client.CubesCount > 0)
                {
                    playersLeft++;
                    possibleWinner = client;
                }
            }

            if (playersLeft > 1)
            {
                // отбор начинающего ход

                // Получаем индекс следующего игрока в коллекции и его имя
                int indexOfNextUser = (curGame.TurnBeginnerIndex + 1) % curGame.Clients.Count;

                // Посокльку следующий игрок может выбыть, то
                // ищем следующего действующего игрока(количество кубиков больше 0).
                // Предполагается, что даже если пользователь выйдет в списке клиентов
                // останется сущность с 0 кубиков.
                while (curGame.Clients[indexOfNextUser].CubesCount <= 0)
                    indexOfNextUser = (indexOfNextUser + 1) % curGame.Clients.Count;

                await Clients.Groups(curGame.GroupName).SendAsync("HandleGameStart", curGame.Clients[indexOfNextUser]);
            }
            else if (playersLeft == 1)
            {
                if (possibleWinner is null)
                    throw new Exception("Уххх, что то пошло по бороде. Победитель не может быть null в этом методе");

                // вызов метода отображения победителя
                await Clients.Groups(curGame.GroupName).SendAsync("HandleWinner", possibleWinner!.Name);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync(exceptionHandler, "произошла ничья. Такого быть не может");
                return;
            }
        }
    }
}
