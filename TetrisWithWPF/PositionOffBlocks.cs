using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWithWPF
{
    public class PositionOffBlocks
    {
        public int Row {  get; set; }
        public int Column { get; set; }

        public PositionOffBlocks(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
