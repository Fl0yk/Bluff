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

        // Хранит в себе индекс пользователя, начавшего ход.
        // Нужно чтобы определять того, кто следующим будет ходить,
        // после оспаривания ставки
        public int TurnBeginnerIndex { get; set; }
    }
}
