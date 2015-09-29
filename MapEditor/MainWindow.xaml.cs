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

namespace MapEditor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        //To delete after (when there will be textures)
        public Color color { get; set; }

        public struct tile
        {
            public Color tileColor;
        }
        tile[,] globalMapee;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Creation of a new map.
        private void MenuFileNew_Click(object sender, RoutedEventArgs e)
        {
            NewMap createNewMapWindow = new NewMap();
            createNewMapWindow.Owner = this;
            createNewMapWindow.ShowDialog();

            // After the popup is closed.
            mapGrid.Children.Clear();
            ////////////
            //TO REMOVE
            testTextBox.Text = createNewMapWindow.mapName + createNewMapWindow.xSize + createNewMapWindow.ySize;
            ///////////
            mapWidth = createNewMapWindow.xSize;
            mapHeight = createNewMapWindow.ySize;
            globalMap = new tile[mapWidth, mapHeight];

            //Deciding the size of the rectangles (tiles)
            int tileSize = 0;
            if (mapWidth > mapHeight)
                tileSize = (550 - mapWidth * 2) / (mapWidth);
            else
                tileSize = (550 - mapHeight * 2) / (mapHeight);                

            for (int j = 0; j < mapHeight; j++)
            {
                WrapPanel panel = new WrapPanel();
                for (int i = 0; i < mapWidth; i++)
                {
                    panel.Children.Add(new Rectangle { Tag = i + "/" + j, Width = tileSize, Height = tileSize, Fill = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF4F4F5"), Stroke = new SolidColorBrush(Colors.Black), RadiusX = 10, RadiusY = 10, Margin = new Thickness(0, 2, 2, 0) });
                }
                mapGrid.Children.Add(panel);
            }
        }

        private void redButton_Click(object sender, RoutedEventArgs e)
        {
            color = Colors.Red;
        }

        private void blueButton_Click(object sender, RoutedEventArgs e)
        {
            color = Colors.Blue;
        }

        private void greenButton_Click(object sender, RoutedEventArgs e)
        {
            color = Colors.Green;
        }

        private void setTileTexture(object sender, MouseButtonEventArgs e)
        {
            Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;

            // Get the TAG of the Rectangle (tag contains coordinates)
            String[] coo = ClickedRectangle.Tag.ToString().Split('/');
            int x = int.Parse(coo[0]);
            int y = int.Parse(coo[1]);

            ClickedRectangle.Fill = new SolidColorBrush(color);
            globalMap[x, y].tileColor = color;
            //displayMap();
        }

        // TEST AND DEBUG FUNCTION
        private void displayMap()
        {
            testTextBox.Text = "";
            for (int j = 0; j < mapHeight; j++)
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    testTextBox.Text += '\n' + "[" + i + "," + j + "]" + globalMap[i, j].tileColor.ToString();
                }
            }
        }
    }
}
