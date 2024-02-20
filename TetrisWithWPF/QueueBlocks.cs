using System;

namespace TetrisWithWPF
{
    // manages the queue of the next block that will be played
    public class QueueBlocks
    {
        // the array holds the different kinds of blocks that can be used
        private readonly Blocks[] blocks = new Blocks[]
        {
            new IBlocks(),
            new JBlocks(),
            new LBlocks(),
            new OBlocks(),
            new SBlocks(),
            new TBlocks(),
            new ZBlocks()
        };

        // random object to make the selection of next block random
        private readonly Random random = new Random();

        // property holds the next block
        public Blocks NextBlock { get; private set; }

        // constructor for queueBlocks, wich initialise the NextBlock property with a random block
        public QueueBlocks()
        {
            NextBlock = RandomBlock();
        }

        // private method that selects a random block from the array of available block types
        private Blocks RandomBlock()
        {
            // wich returns a randomly selected block using the random object
            return blocks[random.Next(blocks.Length)];
        }

        // returns the current NextBlock and updates it to a new block, to ensure its different from the current one
        public Blocks GetAndUpdate()
        {
            // first we store the current NextBlock
            Blocks blocks = NextBlock;
            // we loop to ensure the new NextBlock is different from the current one
            do
            {
                // update NextBlock to a new random block
                NextBlock = RandomBlock();
            }
            // if the new block has the same ID as the current one, select again to rpevent the same block from being repeated
            while (blocks.Id == NextBlock.Id);
            // return the original NextBlock, that is stored in currentBlock
            return blocks;
        }
    }
}
