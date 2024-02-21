using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWithWPF
{
    // represents the L shaped Tetris block, inheriting from the abstract Blocks class
    public class LBlocks : Blocks
    {
        // defines the shapes of the I block in its four rotation states
        public readonly PositionOffBlocks[][] tiles = new PositionOffBlocks[][]
        {
            new PositionOffBlocks[] { new(0,2), new(1,0), new(1,1), new(1,2) },
            new PositionOffBlocks[] { new(0,1), new(1,1), new(2,1), new(2,2) },
            new PositionOffBlocks[] { new(1,0), new(1,1), new(1,2), new(2,0) },
            new PositionOffBlocks[] { new(0,0), new(0,1), new(1,1), new(2,1) }
        };

        // uniquely identifies the L block type
        public override int Id => 3;

        // sets the starting offset for the L block, positioned at the top and center of the area
        protected override PositionOffBlocks StartOffset => new PositionOffBlocks(0, 3);

        // overrides the tiles prop to provide specific rotation shapes for the L block
        protected override PositionOffBlocks[][] Tiles => tiles;
    }
}
