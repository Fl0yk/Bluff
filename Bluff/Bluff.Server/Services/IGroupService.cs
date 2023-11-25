using Bluff.Domain;
using System.Runtime.InteropServices;

namespace Bluff.Server.Services
{
    public interface IGroupService
    {
        public Game? GetGameByName(string name);

        public bool CreateGame(string name, int userToStart = 2);

        public bool AddUserToGame(string groupName, Client client);

        public bool DeleteUserFromGame(string groupName, string clientId);

        public bool DeleteGame(string groupName);

        public List<string> GetNamesOfAllGames();

        public bool IsExistClientInGame(string groupName, string username);

        public bool IsGameReady(string groupName);
    }
}
