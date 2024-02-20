using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWithWPF
{
    // represents the O shaped Tetris block, inheriting from the abstract Blocks class
    public class OBlocks : Blocks
    {
        // defines the shapes of the O block in its four rotation states
        public readonly PositionOffBlocks[][] tiles = new PositionOffBlocks[][]
        {
            // since this is a square, we can just use 1 rotation state
            new PositionOffBlocks[] { new(0,0), new(0,1), new(1,0), new(1,1) },

        };

        // uniquely identifies the O block type
        public override int Id => 4;

        // sets the starting offset for the O block, positioned at the top and center of the area
        protected override PositionOffBlocks StartOffset => new PositionOffBlocks(0, 4);

        // overrides the tiles prop to provide specific rotation shapes for the O block
        protected override PositionOffBlocks[][] Tiles => tiles;
    }
}
