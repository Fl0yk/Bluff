namespace Bluff.Client.models
{
    public class Board
    {
        public Cell[] Cells { get; set; }

        private static readonly Dictionary<int, (int row, int column)> starPositions;

        private static readonly Dictionary<int, (int row, int column)> otherPositions;

        static Board()
        {
            //Задаем, в каких позициях находятся звезды
            starPositions = new() {
                { 1, (0, 1) },
                { 2, (0, 4) },
                { 3, (0, 7) },
                { 4, (1, 9) },
                { 5, (4, 9) },
                { 6, (6, 8) },
                { 7, (6, 5) },
                { 8, (6, 2) },
                { 9, (5, 0) },
                { 10, (2, 0) },
            };

            //Задаем, в каких позициях находятся остальные ячейки
            otherPositions = new() {
                { 1, (0, 0) },
                { 2, (0, 2) },
                { 3, (0, 3) },
                { 4, (0, 5) },
                { 5, (0, 6) },
                { 6, (0, 8) },
                { 7, (0, 9) },
                { 8, (2, 9) },
                { 9, (3, 9) },
                { 10, (5, 9) },
                { 11, (6, 9) },
                { 12, (6, 7) },
                { 13, (6, 6) },
                { 14, (6, 4) },
                { 15, (6, 3) },
                { 16, (6, 1) },
                { 17, (6, 0) },
                { 18, (4, 0) },
                { 19, (3, 0) },
                { 20, (1, 0) }
            };
        }

        public Board() 
        {
            Cells = new Cell[30];
            InitStar();
            InitOtherCells();
            //foreach (var cell in Cells)
                //Console.WriteLine(cell.Id + " " + cell.Value);
        }

        /// <summary>
        /// Инициализируем звезды в нужном месте массива
        /// </summary>
        private void InitStar()
        {
            for(int i = 1; i < 11; i++)
            {
                Cells[3 * i - 2] = new Cell() { Id = 3 * i - 1, 
                                                IsStar = true, 
                                                Position = starPositions[i], 
                                                Value = i };
            }
        }

        /// <summary>
        /// Инициализируем все ячейки, кроме звезд
        /// </summary>
        private void InitOtherCells()
        {
            for(int i = 1; i < 21; i++)
            {
                Cells[3 * i / 2 - 1] = new Cell() { Id = 3 * i / 2, 
                                                    IsStar = false, 
                                                    Position = otherPositions[i], 
                                                    Value = i };
            }
        }

        /// <summary>
        /// Перевод список ячеек в двумерную матрицу для заполнения таблицы в html
        /// </summary>
        /// <returns>Двумерный массив ячеек</returns>
        public Cell[,] ToMatrix()
        {
            Cell[,] matrix = new Cell[7, 10];

            foreach(Cell cell in Cells)
            {
                matrix[cell.Position.row, cell.Position.column] = cell;
            }

            return matrix;
        }

        /// <summary>
        /// Возвращает массив допустимых ячеек для выбора
        /// </summary>
        /// <param name="minId">Минимальный Id ячейки, которую можно выбрать</param>
        /// <param name="isStar">Является ли выбранный кубик звездой</param>
        /// <returns></returns>
        public Cell[] AvailableCells(int minId, bool isStar)
        {
            Cell[] cells = this.Cells.ToArray();

            foreach(Cell cell in cells)
            {
                if(cell.Id >= minId && cell.IsStar == isStar)
                {
                    cell.IsAvailable = true;
                }
                else
                {
                    cell.IsAvailable = false;
                }
            }

            return cells;
        }
    }
}
