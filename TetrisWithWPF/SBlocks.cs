using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWithWPF
{
    // represents the S shaped Tetris block, inheriting from the abstract Blocks class
    public class SBlocks : Blocks
    {
        // defines the shapes of the S block in its four rotation states
        public readonly PositionOffBlocks[][] tiles = new PositionOffBlocks[][]
        {
            new PositionOffBlocks[] { new(0,1), new(0,2), new(1,0), new(1,1) },
            new PositionOffBlocks[] { new(0,1), new(1,1), new(1,2), new(2,2) },
            new PositionOffBlocks[] { new(1,1), new(1,2), new(2,0), new(2,1) },
            new PositionOffBlocks[] { new(0,0), new(1,0), new(1,1), new(2,1) }
        };

        // uniquely identifies the S block type
        public override int Id => 5;

        // sets the starting offset for the S block, positioned at the top and center of the area
        protected override PositionOffBlocks StartOffset => new PositionOffBlocks(0, 3);

        // overrides the tiles prop to provide specific rotation shapes for the S block
        protected override PositionOffBlocks[][] Tiles => tiles;
    }
}
