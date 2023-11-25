namespace Bluff.Client.Services.Servers
{
    public interface IServersHubService : IBaseConnectionService
    {
        public void CreateConnection(string method, Action<List<string>> handler);
        public Task<bool> CreateGroup(string userName, int userToStart);

        public Task<bool> GetServerListRequest();
    }
}
