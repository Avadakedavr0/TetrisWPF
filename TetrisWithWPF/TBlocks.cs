using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWithWPF
{
    // represents the T shaped Tetris block, inheriting from the abstract Blocks class
    public class TBlocks : Blocks
    {
        // defines the shapes of the T block in its four rotation states
        public readonly PositionOffBlocks[][] tiles = new PositionOffBlocks[][]
        {
            // horizontal shape pointing to the right
            new PositionOffBlocks[] { new(1,0), new(1,1), new(1,2), new(1,3) },
            // vertical shape pointing down
            new PositionOffBlocks[] { new(0,2), new(1,2), new(2,2), new(3,2) },
            //horizontyal shape pointing left
            new PositionOffBlocks[] { new(2,0), new(2,1), new(2,2), new(2,3) },
            // vertical shape pointing up
            new PositionOffBlocks[] { new(0,1), new(1,1), new(2,1), new(3,1) },
        };

        // uniquely identifies the T block type
        public override int Id => 6;

        // sets the starting offset for the T block, positioned at the top and center of the area
        protected override PositionOffBlocks StartOffset => new PositionOffBlocks(-1, 3);

        // overrides the tiles prop to provide specific rotation shapes for the T block
        protected override PositionOffBlocks[][] Tiles => tiles;
    }
}
