using System.Collections.Generic;
using System.ComponentModel;

namespace TetrisWithWPF
{
    // defining the base class for the tetris blocks
    public abstract class Blocks
    {
        // abstract prop to hold dfifferent rotation states
        protected abstract PositionOffBlocks[][] Tiles { get; }

        // abstract prop to define start position offset
        protected abstract PositionOffBlocks StartOffset { get; }

        // abstract prop to identify each block type
        public abstract int Id { get; }

        // variabel to track current rotation state of the block
        private int rotationState;

        // offset for the block position to the game area
        private PositionOffBlocks offset;

        // constructor to initialise the block with its starting position
        public Blocks()
        {
            offset = new PositionOffBlocks(StartOffset.Row, StartOffset.Column);
        }

        // enumerates the position of the block tiles based on its current rotation and offset
        public IEnumerable<PositionOffBlocks> TilePositions()
        {
            foreach (PositionOffBlocks p in Tiles[rotationState])
            {
                // to adjust each tiles position by the current offset to get the actual position in the game area
                yield return new PositionOffBlocks(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        // rotates the block clockwise by changing its rotation state
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        // rotates the block counterclockwise by changing its rotation state
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                // if the block is in initial rotation state, set to last rotation state
                rotationState = Tiles.Length - 1;
            }
            else
            {
                // otherwise just decrement the rotation state
                rotationState--;
            }
        }

        // moves the block by a specified number of rows and columns
        public void Move(int rows, int columns)
        {
            // updates the block offset to the specified movement
            offset.Row += rows;
            offset.Column += columns;
        }

        // resets the block to its initial state, for rotation and position
        public void Reset()
        {
            rotationState = 0; // resets rotation state to default
            offset.Row = StartOffset.Row; // resets row offset to start position
            offset.Column = StartOffset.Column; // resets column offset to start position
        }
    }
}
