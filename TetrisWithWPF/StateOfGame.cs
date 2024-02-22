namespace TetrisWithWPF
{
    // stateOfGame manages the current state of the Tetris game, including the game area, the current block, and the queue of upcoming blocks
    public class StateOfGame
    {
        // the current active block in the game
        private Blocks currentBlock;

        // public prop to get the current block with a private setter that resets the block when it's set
        public Blocks CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset(); // resets the block to its initial state whenever it's updated
                // move the block down by 2 rows if nothing is in the way, so its like it the original game that the block spawns visible instead of headed above the gamegrid itself
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        }

        // represents the game area where the blocks are placed
        public GameGrid GameGrid { get; }
        // manages the queue of blocks that will come next in the game
        public QueueBlocks QueueBlocks { get; }
        // flag indicating whether the game is over
        public bool GameOver { get; private set; }
        // holds the score of the game
        public int Score { get; private set; }
        // holds the block that the player can use later
        public Blocks HeldBlocks { get; private set; }
        // determines if the player can hold blocks
        public bool CanHoldBlocks { get; private set; }

        // constructor initializes the game state, creating the game area and the queue of blocks
        public StateOfGame()
        {
            GameGrid = new GameGrid(22, 10); // standard Tetris game area size
            QueueBlocks = new QueueBlocks(); // initializes the queue of blocks
            CurrentBlock = QueueBlocks.GetAndUpdate(); // sets the current block to the next block from the queue
            CanHoldBlocks = true; // allows holding blocks at the start
        }

        // checks if the current block fits in the game area without overlapping other blocks
        private bool BlockFits()
        {
            foreach (PositionOffBlocks p in CurrentBlock.TilePositions())
            {
                // if any part of the block is outside the game area or collides with existing blocks, it does not fit
                if (!GameGrid.IsEmptyCell(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        // allows the player to hold onto a block to use later
        public void HoldBlocks()
        {
            if (!CanHoldBlocks) // if holding is not allowed, exit the function
            {
                return;
            }

            if (HeldBlocks == null)
            {
                HeldBlocks = CurrentBlock; // hold the current block
                CurrentBlock = QueueBlocks.GetAndUpdate();  // get a new block from the queue
            }
            else
            {
                Blocks tmp = CurrentBlock; // temporarily store the current block
                CurrentBlock = HeldBlocks; // switch to the held block
                HeldBlocks = tmp; // update the held block
            }

            CanHoldBlocks = false; // disallow holding another block until the next one is placed
        }

        // rotates the current block clockwise and if it doesn't fit after rotation it is rotated back
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                currentBlock.RotateCCW();
            }
        }

        // rotates the current block counter-clockwise and if it doesn't fit after rotation it is rotated back
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                currentBlock.RotateCW();
            }
        }

        // moves the current block one cell to the left and if it doesn't fit after moving it is moved back
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        // moves the current block one cell to the right and if it doesn't fit after moving it is moved back
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        // determines if the game is over which occurs if the top two rows of the game area are not empty
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        // places the current block into the game area and updates the game state
        private void PlaceBlock()
        {
            foreach (PositionOffBlocks p in currentBlock.TilePositions())
            {
                // fills the game area cells with the block's Id to indicate they are occupied
                GameGrid[p.Row, p.Column] = currentBlock.Id;
            }
            // clears any rows that are now full
            Score += GameGrid.ClearFullRows();

            // checks if the game is over after placing the block
            if (IsGameOver())
            {
                // if the game is over when blocks have reached the top then set the GameOver property to true
                GameOver = true;
            }
            else
            {
                // if the game is not over update the CurrentBlock with the next block from the queue
                CurrentBlock = QueueBlocks.GetAndUpdate();
                CanHoldBlocks = true;
            }
        }

        public void MoveBlockDown()
        {
            // attempt to move the current block down by one row
            CurrentBlock.Move(1, 0);

            // check if the block fits after moving it down and if not it has collided with other blocks or the bottom of the game area
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0); // move the block back up by one row since it doesn't fit after moving down
                PlaceBlock(); // place the block in its current position because it can't move down further
            }
        }

        // method calculates the maximum number of rows the current tile can drop until it would collide with another tile or the bottom of the grid
        private int TileInstantDrop(PositionOffBlocks p)
        {
            int drop = 0; // start with a drop distance of 0
            // as long as the cell below the current position is empty increment the drop distance
            while (GameGrid.IsEmptyCell(p.Row + drop + 1, p.Column))
            {
                drop++;
            }
            // return the total drop distance calculated
            return drop;
        }

        // determines the maximum distance the current block can drop and does this by checking the drop distance for each tile in the block
        // then taking the smallest distance as the block moves as an unit
        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows; // start with the maximum possible drop distance

            // loop through each tile position in the current block
            foreach (PositionOffBlocks p in CurrentBlock.TilePositions())
            {
                // find the smallest drop distance from all tile positions
                drop = System.Math.Min(drop, TileInstantDrop(p));
            }
            // and return the smallest drop distance
            return drop;
        }

        // drops the current block down instantly to the lowest available position on the grid
        public void DroptheBlock()
        {
            // move the block down by the maximum available drop distance
            currentBlock.Move(BlockDropDistance(), 0);
            // then once the block has been moved down place it on the grid
            PlaceBlock();
        }
    }
}
