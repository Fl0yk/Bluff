namespace Bluff.Client.Services.Servers
{
    public interface IServersHubService : IBaseConnectionService
    {
        public Task<bool> CreateGroup(string userName, int userToStart);

        public Task<bool> GetServerListRequest();
    }
}
