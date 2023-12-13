using Bluff.Domain;

namespace Bluff.Client.Services.InGame
{
    public interface IInGameHubService : IBaseConnectionService
    {
        /// <summary>
        /// Метод подключает пользователя к конкретной игре
        /// </summary>
        /// <param name="userName">Имя клиента, который подключается к игре</param>
        /// <param name="groupName">Название группы к которой подключается клиент</param>
        public Task<bool> ConnectToServerRequest(string username, string groupName);

        public void CreateConnection(string method, Action<Domain.Client> handler);

        public void CreateConnection(string method, Action<List<Domain.Client>> handler);

        public void CreateConnection(string method, Action<int> handler);

        public void CreateConnection(string method, Action<Bet, string> handler);

        public void CreateConnection(string method, Action handler);

        /// <summary>
        /// Метод уведомляет сервер о нажатии кнопки готов пользователем
        /// </summary>
        /// <param name="groupName">Название группы в которой была нажата кнопка готов</param>

        public Task<bool> UserReadyRequest(string groupName);

        /// <summary>
        /// Метод получает всех пользователей подключенных к одной группе
        /// </summary>
        /// <param name="groupName">Название группы у которой будут получены все пользователи</param>
        public Task<bool> GetClientsRequest(string groupName);

        public Task<bool> PlaceABetRequest(string groupName, string username, int cubeVal, int count, int id);

        public Task<bool> Dispute(string groupName, string username);
    }
}
