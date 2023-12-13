namespace Bluff.Domain
{
    public class Game
    {
        public string GroupName { get; set; }
        
        public List<Client> Clients { get; set; } = new();

        public Client? BetAuthor { get; set; }

        public Bet? Bet { get; set; }

        public int UserToStart { get; set; }

        public int ReadyUsers { get; set; } = 0;
    }
}
