using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TetrisWithWPF
{
    /// <summary>
    /// Contains all classes and resources for the Tetris game built with Windows Presentation Foundation (WPF).
    /// This includes the main game window, game logic, and utility classes to manage game states, Tetriminos, and game grid.
    /// </summary>
    
    // main class for the Tetris game window
    public partial class MainWindow : Window
    {
        // array of image sources representing the empty and various tiles used in the game
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            // each BitmapImage is created by providing the URI of the image file
            // The Relative kind indicates that the paths are relative to the project structure
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile2.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile3.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile4.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile5.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile6.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile7.png", UriKind.Relative))
        };

        // array of image sources for each type of block in the game
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            // images for blocks used when a specific block type is held or is next in the queue
            new BitmapImage(new Uri("Assets/EmptyBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/IBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/JBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/LBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/OBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/SBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TBlock.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/ZBlock.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls; // 2d array

        // game speed variables
        private readonly int maxDelay = 800; // maximum delay between block moves
        private readonly int minDelay = 300;// minimum delay for fast game speed
        private readonly int delayDecrease = 30; // decrease in delay as game progresses
        private List<int> highScores = new List<int>(); // list to store high scores
        private string highScoresFilePath = System.IO.Path.Combine( // file path to save high score
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "TetrisWithWPF_HighScores.txt");
        private StateOfGame stateOfGame = new StateOfGame(); // object to hold the current state of the game

        // constructor
        public MainWindow()
        {
            InitializeComponent(); // initialize components from XAML
            imageControls = SetupGameCanvas(stateOfGame.GameGrid); // set up game canvas with image controls
        }

        // method to create image controls and place them on the canvas
        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            // local variable to hold created image controls
            Image[,] localImageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25; // setting cellsize
            // loop through grid rows and columns
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    // create new image control for each cell
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    // position image control on the canvas
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 15); // to make you see that the top row actually is full when it is game over
                    Canvas.SetLeft(imageControl, c * cellSize);
                    // add image control to the canvas and local array
                    GameCanvas.Children.Add(imageControl);
                    localImageControls[r, c] = imageControl;
                }
            }
            return localImageControls;
        }

        // method to handle the 'Start Game' button click
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed; // hide the start screen
            StartGame(); // call the method to start the game
        }

        // async method to start the game loop
        private async void StartGame()
        {
            await GameLoop(); // start the game loop
        }

        // method to draw the entire game grid with the appropriate images for each tile
        private void DrawGrid(GameGrid grid)
        {
            // iterate through each cell in the game grid
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    // get the id that represents the tile type
                    int id = grid[r, c];
                    // reset opacity to fully visible and set the corresponding image for the tile
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        // draws the falling block in its current position
        private void DrawBlock(Blocks blocks)
        {
            // go through each tile position in the current block
            foreach (PositionOffBlocks p in blocks.TilePositions())
            {
                // set the image and opacity for each block part to make it visible
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[blocks.Id];
            }
        }
        // the method updates the display to show the next block in the queue
        private void DrawNextBlock(QueueBlocks queueBlocks)
        {
            // get the next block from the queue
            Blocks next = queueBlocks.NextBlock;
            // set the image for the 'Next' block display
            Next.Source = blockImages[next.Id];
        }

        // method visualizes where the current block would land if dropped like a ghostblock
        private void DrawGhostBlock(Blocks blocks)
        {
            // first calculate how far the block would drop without colliding
            int dropDistance = stateOfGame.BlockDropDistance();
            // draw a ghost image of the block at the drop location
            foreach (PositionOffBlocks p in blocks.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.2;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[blocks.Id];
            }
        }

        // method updates the game UI with the current state
        private void Draw(StateOfGame stateOfGame)
        {
            // draw the grid, ghost block, actual block, next block and held block
            DrawGrid(stateOfGame.GameGrid);
            DrawGhostBlock(stateOfGame.CurrentBlock);
            DrawBlock(stateOfGame.CurrentBlock);
            DrawNextBlock(stateOfGame.QueueBlocks);
            DrawHeldBlock(stateOfGame.HeldBlocks);
            // update the score display
            ScoreText.Text = $"Score: {stateOfGame.Score * 100}"; // with each cleared line you get 100 points
        }

        // the main game loop that runs throughout the game
        private async Task GameLoop()
        {
            // continuously draw the game state and handle block movements
            Draw(stateOfGame);
            // keep looping until the game is over
            while (!stateOfGame.GameOver)
            {
                // calculate the delay for the current game speed
                int delay = Math.Max(minDelay, maxDelay - (stateOfGame.Score / 100) * delayDecrease);
                // wait for the delay duration before moving the block down
                await Task.Delay(delay);
                // move the block down and redraw the game state
                stateOfGame.MoveBlockDown();
                Draw(stateOfGame);
            }
            // when the game is over, update the UI and high scores
            OnGameOver();
            GameOver.Visibility = Visibility.Visible;
            TheFinalScoreText.Text = $"Total score: {stateOfGame.Score * 100}";
        }

        //  to display the held block image in the UI
        private void DrawHeldBlock(Blocks heldBlock)
        {
            // if no block is being held then show the empty block image
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0]; // empty block image index is assumed to be 0
            }
            else
            {
                // if a block is held then display its corresponding image
                HoldImage.Source = blockImages[heldBlock.Id];  // each block type has a unique id associated with an image
            }
        }

        // a method to handle keyboard input during the game
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // if it is game over then ignore key presses
            if (stateOfGame.GameOver)
            {
                return;
            }
            // switch on the key pressed and call the appropriate method
            switch (e.Key)
            {
                case Key.Left:
                    stateOfGame.MoveBlockLeft(); // move the block left
                    break;
                case Key.Right:
                    stateOfGame.MoveBlockRight(); // move the block right
                    break;
                case Key.Down:
                    stateOfGame.MoveBlockDown(); // move the block down line per line
                    break;
                case Key.X:
                    stateOfGame.RotateBlockCW(); // rotate the block right (clockwise)
                    break;
                case Key.Z:
                    stateOfGame.RotateBlockCCW(); // move the block left (counter-clockwise)
                    break;
                case Key.C:
                    stateOfGame.HoldBlocks(); // hold the current block
                    break;
                case Key.Up:
                    stateOfGame.DroptheBlock(); // instantly drop the block down 
                    break;
                default:
                    return; // if another key is pressed do nothing
            }

            // redraw the game state after handling the key event
            Draw(stateOfGame);

        }

        // next method loads high scores when the game canvas is loaded
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHighScores(); // call the method to load high scores from file
        }

        // resets the game state and starts a new game when the 'Play Again' button is clicked
        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            stateOfGame = new StateOfGame(); // reset the game state
            GameOver.Visibility = Visibility.Hidden; // hide the game over screen
            await GameLoop(); // restart the game loop
        }

        //  updates the high scores list with a new score if it qualifies
        public void UpdateHighScores(int newScore)
        {
            // check if the score is greater than 0 and not already in the high scores list
            if (newScore > 0 && !highScores.Contains(newScore))
            {
                highScores.Add(newScore); // add the new score
                highScores.Sort((a, b) => b.CompareTo(a)); // sort the list in descending order

                // if there are more than 10 high scores, keep only the top 10
                if (highScores.Count > 10)
                {
                    highScores = highScores.Take(10).ToList();
                }

                //then update the high scores in the UI
                HighScoreList.ItemsSource = highScores.Select(score => $"Score: {score}").ToList();
            }
        }

        // this method is called when the game is over to update UI and high scores
        private void OnGameOver()
        {
            int finalScore = stateOfGame.Score * 100; // calculate the final score
            UpdateHighScores(finalScore); // update the high scores list
            SaveHighScores(); // save the high scores to a file
        }

        // saves the current high scores to a text file
        private void SaveHighScores()
        {
            string highScoresText = string.Join(Environment.NewLine, highScores);  // create a string with each score on a new line
            System.IO.File.WriteAllText(highScoresFilePath, highScoresText); // write the string to the file path
        }

        // this method loads high scores from a text file
        private void LoadHighScores()
        {
            // first check if the high scores file exists
            if (System.IO.File.Exists(highScoresFilePath))
            {
                string[] lines = System.IO.File.ReadAllLines(highScoresFilePath); // then read all lines from the file
                highScores = lines.Select(int.Parse).ToList(); // convert each line to an integer and add to the list
            }
            // at last update the high scores in the UI
            HighScoreList.ItemsSource = highScores.Select(score => $"Score: {score}").ToList();
        }
    }
}
