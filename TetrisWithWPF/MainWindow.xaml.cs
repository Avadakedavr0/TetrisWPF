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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile1.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile2.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile3.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile4.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile5.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile6.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Tile7.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
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
        private readonly int maxDelay = 800;
        private readonly int minDelay = 300;
        private readonly int delayDecrease = 30;
        private List<int> highScores = new List<int>();
        private string highScoresFilePath = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "TetrisWithWPF_HighScores.txt");
        private StateOfGame stateOfGame = new StateOfGame();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(stateOfGame.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] localImageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 15); // to make you see that the top row actually is full when it is game over
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    localImageControls[r, c] = imageControl;
                }
            }
            return localImageControls;
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartScreen.Visibility = Visibility.Collapsed; // hide the start screen
            StartGame(); // call the method to start the game
        }

        private async void StartGame()
        {
            await GameLoop(); // start the game loop
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1; // reset it to original
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Blocks blocks)
        {
            foreach (PositionOffBlocks p in blocks.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1; // reset it to original
                imageControls[p.Row, p.Column].Source = tileImages[blocks.Id];
            }
        }

        private void DrawNextBlock(QueueBlocks queueBlocks)
        {
            Blocks next = queueBlocks.NextBlock;
            Next.Source = blockImages[next.Id];
        }

        private void DrawGhostBlock(Blocks blocks)
        {
            int dropDistance = stateOfGame.BlockDropDistance();
            foreach (PositionOffBlocks p in blocks.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.2;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[blocks.Id];
            }
        }

        private void Draw(StateOfGame stateOfGame)
        {
            DrawGrid(stateOfGame.GameGrid);
            DrawGhostBlock(stateOfGame.CurrentBlock);
            DrawBlock(stateOfGame.CurrentBlock);
            DrawNextBlock(stateOfGame.QueueBlocks);
            DrawHeldBlock(stateOfGame.HeldBlocks);
            ScoreText.Text = $"Score: {stateOfGame.Score * 100}"; // with each cleared line you get 100 points
        }

        private async Task GameLoop()
        {
            Draw(stateOfGame);

            while (!stateOfGame.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (stateOfGame.Score / 100) * delayDecrease);
                await Task.Delay(delay);
                stateOfGame.MoveBlockDown();
                Draw(stateOfGame);
            }

            OnGameOver();
            GameOver.Visibility = Visibility.Visible;
            TheFinalScoreText.Text = $"Total score: {stateOfGame.Score * 100}";
        }

        private void DrawHeldBlock(Blocks heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (stateOfGame.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    stateOfGame.MoveBlockLeft();
                    break;
                case Key.Right:
                    stateOfGame.MoveBlockRight();
                    break;
                case Key.Down:
                    stateOfGame.MoveBlockDown();
                    break;
                case Key.X:
                    stateOfGame.RotateBlockCW();
                    break;
                case Key.Z:
                    stateOfGame.RotateBlockCCW();
                    break;
                case Key.C:
                    stateOfGame.HoldBlocks();
                    break;
                case Key.Up:
                    stateOfGame.DroptheBlock();
                    break;
                default:
                    return;
            }

            Draw(stateOfGame);

        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            LoadHighScores();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            stateOfGame = new StateOfGame();
            GameOver.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        public void UpdateHighScores(int newScore)
        {
            highScores.Add(newScore);
            highScores.Sort((a, b) => b.CompareTo(a));
            highScores = highScores.Take(10).ToList();
            HighScoreList.ItemsSource = highScores.Select(score => $"Score: {score}").ToList();
        }

        private void OnGameOver()
        {
            int finalScore = stateOfGame.Score * 100;
            UpdateHighScores(finalScore);
            SaveHighScores();
        }
        private void SaveHighScores()
        {
            string highScoresText = string.Join(Environment.NewLine, highScores);
            System.IO.File.WriteAllText(highScoresFilePath, highScoresText);
        }

        private void LoadHighScores()
        {
            if (System.IO.File.Exists(highScoresFilePath))
            {
                string[] lines = System.IO.File.ReadAllLines(highScoresFilePath);
                highScores = lines.Select(int.Parse).ToList();
            }

            HighScoreList.ItemsSource = highScores.Select(score => $"Score: {score}").ToList();
        }
    }
}
