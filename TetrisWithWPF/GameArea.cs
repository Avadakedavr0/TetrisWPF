namespace TetrisWithWPF
{
    public class GameArea
    {
        // setting up game area
        private readonly int[,] area;
        public int Rows { get; }
        public int Columns { get; }
        public int this[int r, int c]
        {
            get => area[r, c];
            set => area[r, c] = value;
        }

        // setting up rows and columns
        public GameArea(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            area = new int[rows, columns];
        }

        public bool isInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        // check if the cell is empty or not
        public bool IsEmptyCell(int r, int c)
        {
            return IsEmptyCell(r, c) && area[r, c] == 0;
        }

        // next check if row is full
        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (area[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        //  next we need to check if row is full
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (area[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
