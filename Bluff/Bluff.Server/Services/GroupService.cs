using Bluff.Domain;

namespace Bluff.Server.Services
{
    public class GroupService : IGroupService
    {
        private List<Game> _games = new();

        public bool AddUserToGame(string groupName, Client client)
        {
            Game? curGame = _games.FirstOrDefault(g => g.GroupName.Equals(groupName));

            if(curGame is not null)
            {
                curGame.Clients.Add(client);
                return true;
            }

            return false;
        }

        public bool CreateGame(string name, int userToStart = 2)
        {
            if (_games.FirstOrDefault(g => g.GroupName.Equals(name)) is null)
            {
                _games.Add(new Game() { GroupName = name, UserToStart = userToStart });
                return true;
            }

            return false;
        }

        public bool DeleteGame(string groupName)
        {
            Game? curGame = _games.FirstOrDefault(g => g.GroupName.Equals(groupName));

            if (curGame is not null)
            {
                _games.Remove(curGame);
                return true;
            }

            return false;
        }

        public bool DeleteUserFromGame(string groupName, string clientId)
        {
            Game? curGame = _games.FirstOrDefault(g => g.GroupName.Equals(groupName));
            if (curGame is null)
            {
                return false;
            }

            Client? curClient = curGame.Clients.FirstOrDefault(c => c.Id.Equals(clientId));
            if (curClient is null)
            {
                return false;
            }

            curGame.Clients.Remove(curClient);
            return true;
        }

        public Game? GetGameByName(string name)
        {
            return _games.FirstOrDefault(g => g.GroupName.Equals(name));
        }

        public bool IsGameReady(string groupName)
        {
            var game = _games.FirstOrDefault(g => g.GroupName.Equals(groupName));
            if (game is not null)
                return game.Clients.Count() == game.UserToStart;
            else
                return false;

        }

        public List<string> GetNamesOfAllGames()
        {
            return _games.Select(g => g.GroupName).ToList();
        }

        public bool IsExistClientInGame(string groupName, string username)
        {
            Game curGame = _games.First(g => g.GroupName.Equals(groupName));

            return curGame.Clients.FirstOrDefault(c => c.Name.Equals(username)) is null ? false : true;
        }
    }
}
