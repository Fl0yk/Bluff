using Bluff.Domain;

namespace Bluff.Client.models
{
    public class GameModel
    {
        public Board GameBoard { get; set; }
        public string GameName { get; set; }
        public string? CurUser { get; set; }
        public List<Bluff.Domain.Client> Clients { get; set; }
        GameState State { get; set; }
        public Bet? Bet { get; set; }

        public GameModel(string gameName) 
        {
            GameBoard = new Board();
            Clients = new();
            GameName = gameName;
            State = GameState.WaitStart;
        }

        public void Start(string firstClient)
        {
            if(CurUser == firstClient)
            {
                State = GameState.Bet;
            }
            else
            {
                State = GameState.ExpectBet;
            }
        }
    }

    public enum GameState
    {
        WaitStart,
        ExpectBet,
        Bet
    }

}
