using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWithWPF
{
    // represents the position of a block in terms of rows and columns
    public class PositionOffBlocks
    {
        // get or set the row position of the block
        public int Row {  get; set; }

        // get or set the column position of the block
        public int Column { get; set; }

        // constructor to set the initial position of the block
        public PositionOffBlocks(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
