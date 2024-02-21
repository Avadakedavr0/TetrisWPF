using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            new BitmapImage(new Uri("Assets/Tile7.png", UriKind.Relative)),
        };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
