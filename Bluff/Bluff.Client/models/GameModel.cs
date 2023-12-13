using Bluff.Domain;

namespace Bluff.Client.models
{
    public class GameModel
    {
        public Board GameBoard { get; set; }
        //Название игры(группы, сервера)

        public event Action UpdatePage;

        public event Action UpdatePlayers;

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
        public List<Player> Clients { get; set; }
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
                    UpdatePlayers?.Invoke();
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

            //UpdateCountOfCubs();
            
            Bet = null;
            UpdatePage?.Invoke();
        }

        public void GetNewBet(Bet newBet, string nextUser)
        {
            Bet = newBet;

            if (CurUser == nextUser)
                State = GameState.ChangeMenu;
            else
                State = GameState.ExpectBet;
        }

        public void UpdatePlayersList(List<Domain.Client> clients)
        {
            Clients = new List<Player>(clients.Select(c => new Player()
            {
                Name = c.Name,
                CubeCount = c.CubesCount,
                Cubes = c.Cubes.ToArray()
            }));

            UpdateCountOfCubs();

            UpdatePage?.Invoke();
            UpdatePlayers?.Invoke();
        }

        private void UpdateCountOfCubs()
        {
            CountOfCubes = 0;

            foreach (Player player in Clients)
            {
                CountOfCubes += player.CubeCount;
            }
        }
    }

    public enum GameState
    {
        WaitStart,
        ChangeMenu,
        ExpectBet,
        Bet,
        Dispute,
        EndGame
    }

}
