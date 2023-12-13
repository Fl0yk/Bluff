using Bluff.Domain;

namespace Bluff.Client.models
{
    public class GameModel
    {
        public Board GameBoard { get; set; }
        //Название игры(группы, сервера)

        public event Action UpdatePage;

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
        //Имя победителя
        public string? WinnerName {  get; set; }
        //Храним имена клиентов
        public List<string> Clients { get; set; }
        private GameState _state;
        public GameState State 
        { get
            {
                return this._state;
            }
            set
            {
                if(this._state != value)
                {
                    this._state= value;
                    UpdatePage?.Invoke();
                }
            }
        }
        public Bet? Bet { get; set; }
        //Количество кубиков в игре
        public int CountOfCubes {  get; set; }

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
                State = GameState.ChangeMenu;
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
                State = GameState.ChangeMenu;
            else
                State = GameState.ExpectBet;
        }
    }

    public enum GameState
    {
        WaitStart,
        ChangeMenu,
        ExpectBet,
        Bet,
        EndGame
    }

}
