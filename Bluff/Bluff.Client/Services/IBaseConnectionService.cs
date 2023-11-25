namespace Bluff.Client.Services
{
    public interface IBaseConnectionService
    {
        /// <summary>
        /// Содержит в себе сообщение об ошибке
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Создаем соединение с хабом
        /// </summary>
        /// <param name="method">Имя метода, который хаб может вызывать</param>
        /// <param name="handler">Событие, которое будет происходить при вызове этого метода хабом</param>
        public void CreateConnection(string method, Action<string, string> handler);

        public void CreateConnection(string method, Action<string> handler);

        /// <summary>
        /// Подключение к хабу
        /// </summary>
        /// <returns>Успешное ли подключение</returns>
        public Task<bool> ConnectToHub();
    }
}
