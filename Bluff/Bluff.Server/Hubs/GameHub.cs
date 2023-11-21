using Bluff.Domain;
using Microsoft.AspNetCore.SignalR;

namespace Bluff.Server.Hubs
{
    public class GameHub : Hub
    {
        private List<Client> clients {  get; set; }
        
        private Client? betAuthor { get; set; }
        
        private Bet? bet {  get; set; }

        public async Task UserConnected(string userName)
        {
            
        }
    }
}