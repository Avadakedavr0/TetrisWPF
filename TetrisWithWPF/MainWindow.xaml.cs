using System;
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

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Blocks blocks)
        {
            foreach (PositionOffBlocks p in blocks.TilePositions())
            {
                imageControls[p.Row, p.Column].Source = tileImages[blocks.Id];
            }
        }

        private void DrawNextBlock(QueueBlocks queueBlocks)
        {
            Blocks next = queueBlocks.NextBlock;
            Next.Source = blockImages[next.Id];
        }

        private void Draw(StateOfGame stateOfGame)
        {
            DrawGrid(stateOfGame.GameGrid);
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
                await Task.Delay(500);
                stateOfGame.MoveBlockDown();
                Draw(stateOfGame);
            }

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
                case Key.Up:
                    stateOfGame.RotateBlockCW();
                    break;
                case Key.Z:
                    stateOfGame.RotateBlockCCW();
                    break;
                case Key.C:
                    stateOfGame.HoldBlocks();
                    break;
                case Key.Space:
                    stateOfGame.DroptheBlock();
                    break;
                default:
                    return;
            }

            Draw(stateOfGame);

        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            stateOfGame = new StateOfGame();
            GameOver.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
