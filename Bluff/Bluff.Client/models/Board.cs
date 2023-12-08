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
            //InitCells();
        }

        private void InitCells()
        {
            int id = 0, index = 1, starInd = 1;

            
            //Заполнение верхнего ряда
            for(; index < 8; index++)
            {
                //Заполняем обычные ячейки
                Cells[3 * index / 2 - 1] = new Cell() { Id = id++, IsStar = false, Position = (0, 3 * index / 2 - 1), Value = id };
                //В верхней строчке у нас находятся 3 звезды, так что их заполняем
                if (starInd < 4)
                {
                    int pos = 3 * starInd - 2;
                    //Console.WriteLine(pos + " " + starInd);
                    Cells[pos] = new Cell() { Id = id++, IsStar = true, Position = (0, pos), Value = starInd++ };
                }
            }
            Console.WriteLine("End up");
            //Заполнение правого ряда
            int rowId = 0;
            for (; index < 12; index++)
            {
                //Заполняем обычные ячейки
                Cells[3 * index / 2 - 1] = new Cell() { Id = id++, IsStar = false, Position = (rowId++, 9), Value = id };

                //В прваом ряду у нас находятся 2 звезды, так что их заполняем
                if (starInd < 6)
                {
                    Cells[3 * starInd - 2] = new Cell() { Id = id++, IsStar = true, Position = (rowId++, 9), Value = starInd++ };
                }
            }
            Console.WriteLine("End right");
            //Заполнение нижнего ряда
            int columnId = 8;
            for (; index < 18; index++)
            {
                //В нижней строчке у нас находятся 3 звезды, так что их заполняем
                if (starInd < 9)
                {
                    Cells[3 * starInd - 2] = new Cell() { Id = id++, IsStar = true, Position = (6, columnId--), Value = starInd++ };
                }
                //Заполняем обычные ячейки
                Cells[3 * index / 2 - 1] = new Cell() { Id = id++, IsStar = false, Position = (6, columnId--), Value = id };
            }
            Console.WriteLine("End down");
            //Заполнение левого ряда
            rowId = 5;
            for (; index < 21; index++)
            {
                //В левом ряду у нас находятся 2 звезды, так что их заполняем
                if (starInd < 11)
                {
                    Cells[3 * starInd - 2] = new Cell() { Id = id++, IsStar = true, Position = (rowId--, 0), Value = starInd++ };
                }
                //Заполняем обычные ячейки
                Cells[3 * index / 2 - 1] = new Cell() { Id = id++, IsStar = false, Position = (rowId--, 0), Value = id };
            }
            Console.WriteLine("End left");

            foreach (Cell cell in Cells)
            {
                Console.WriteLine(cell.Id + ": " + cell.Position + " " + cell.Value);
            }
        }

        private void InitStar()
        {
            for(int i = 1; i < 11; i++)
            {
                Cells[3 * i - 2] = new Cell() { Id = i, IsStar = true, Position = starPositions[i], Value = i };
            }
        }

        private void InitOtherCells()
        {
            for(int i = 1; i < 21; i++)
            {
                Cells[3 * i / 2 - 1] = new Cell() { Id = i, IsStar = false, Position = otherPositions[i], Value = i };
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
    }
}
