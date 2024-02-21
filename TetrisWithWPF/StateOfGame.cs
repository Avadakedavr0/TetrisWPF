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
        public int Score {  get; private set; }

        // constructor initializes the game state, creating the game area and the queue of blocks
        public StateOfGame()
        {
            GameGrid = new GameGrid(22, 10); // standard Tetris game area size
            QueueBlocks = new QueueBlocks(); // initializes the queue of blocks
            CurrentBlock = QueueBlocks.GetAndUpdate(); // sets the current block to the next block from the queue
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
    }
}
