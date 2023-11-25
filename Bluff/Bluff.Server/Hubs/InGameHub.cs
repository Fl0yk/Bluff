using Bluff.Domain;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class InGameHub : Hub
    {
        //
        private List<Client> clients = new();

        private Client? betAuthor;

        private Bet? bet;

        public int userToStart = 2;

        /// <summary>
        /// Метод добавляет клиента в список игроков и проверяет,
        /// </summary>
        /// <param name="userName">Имя клиента, который подключается к игре</param>
        public async Task UserConnected(string userName)
        {
            // если каким то образом пользователь подключился когда
            // достигнуто максимальное количество пользователей - бросаем исключение
            if (clients.Count == userToStart)
                throw new Exception("Ошибка: достигнуто максимальное количество пользователей");


            //  добавляем нового игрока
            clients.Add(new Client { Name = userName, Id = Context.ConnectionId });

            // если игроков достаточно для начала - начинаем игру
            // иначе - отправляем всем пользователем подключенных игроков для отрисовки
            if (clients.Count == userToStart)
            {
                // ДЛЯ МИХИ - метод HandleGameStart должен спросить у игроков, готовы ли они начать
                // если все готовы - то должен вызвать метод НАЗВАНИЕ МЕТОДА передав в него АРГУМЕНТЫ
                Clients.All.SendAsync("HandleGameStart", clients);
            }
            else
            {
                Clients.All.SendAsync("UserConnected", clients);
            }
        }
    }
}
