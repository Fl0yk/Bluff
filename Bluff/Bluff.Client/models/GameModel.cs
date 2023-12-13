using Bluff.Domain;

namespace Bluff.Client.models
{
    public class GameModel
    {
        public Board GameBoard { get; set; }
        //Название игры(группы, сервера)

        private string _gameName;
        //Вы спросите, зачем
        //А я отвечу, чтоб только 1 раз инициализировать 
        public string GameName { 
            get
            {
                return _gameName;
            }
            set
            {
                _gameName ??= value;
            }
        }
        //Имя нынешнего клиента
        public string? CurUser { get; set; }
        //Храним имена клиентов
        public List<string> Clients { get; set; }
        public GameState State { get; set; }
        public Bet? Bet { get; set; }

        public GameModel() 
        {
            GameBoard = new Board();
            Clients = new();
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

        public void GetNewBet(Bet newBet, string nextUser)
        {
            Bet = newBet;
            
            if (CurUser == nextUser)
                State = GameState.Bet;
            else
                State = GameState.ExpectBet;
        }
    }

    public enum GameState
    {
        WaitStart,
        ExpectBet,
        Bet
    }

}
