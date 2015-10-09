using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string mapName { get; set; }
        // To delete after (when there will be textures)
        public Color color { get; set; }

        public class tile
        {
            public Color tileColor { get; set; }
            public int coordx { get; set; }
            public int coordy { get; set; }
        }

        tile[,] globalMap;

        // Object that will be serialized for the JSON file creation
        public class MapInfos
        {
            public string Name { get; set; }
            public string Size { get; set; }
            public List<tile> Tiles { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        // Creation of a new map
        private void MenuFileNew_Click(object sender, RoutedEventArgs e)
        {
            NewMap createNewMapWindow = new NewMap();
            createNewMapWindow.Owner = this;
            createNewMapWindow.ShowDialog();

            // After the popup is closed
            mapGrid.Children.Clear();
            mapWidth = createNewMapWindow.xSize;
            mapHeight = createNewMapWindow.ySize;
            mapName = createNewMapWindow.mapName;
            globalMap = new tile[mapWidth, mapHeight];

            if (mapWidth > 0 && mapHeight > 0)
            {
                // Setting the mapName as the title of the window
                this.Title = mapName;
                // Deciding the size of the rectangles (tiles)
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
        }

        // Those variables are the coordinate of the rectangle on a MouseDown event
        public int firstTileX { get; set; }
        public int firstTileY { get; set; }

        // Get the coordinates of the tile selected (or the first tile if multiple tiles are selected)
        private void setTileTexture(object sender, MouseButtonEventArgs e)
        {
            Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;

            // Get the TAG of the Rectangle (tag contains coordinates)
            String[] coo = ClickedRectangle.Tag.ToString().Split('/');
            firstTileX = int.Parse(coo[0]);
            firstTileY = int.Parse(coo[1]);
        }

        // To save a map
        private void MenuFileNew_Save(object sender, RoutedEventArgs e)
        {
            // Popup to select the location of the file
            SaveFileDialog saveFilePopup = new SaveFileDialog();

            saveFilePopup.DefaultExt = ".json";
            saveFilePopup.Filter = "JSON documents (.json)|*.json";
            saveFilePopup.Title = "Save your map";
            saveFilePopup.FileName = removeSpecialCharacters(mapName);
            saveFilePopup.ShowDialog();

            if (saveFilePopup.FileName != "")
            {
                List<tile> tileList = new List<tile>();
                for (int j = 0; j < mapHeight; j++)
                {
                    for (int i = 0; i < mapWidth; i++)
                    {
                        if (globalMap[i, j] != null)
                        {
                            tileList.Add(new tile()
                            {
                                tileColor = globalMap[i, j].tileColor,
                                coordx = i,
                                coordy = j
                            });
                        }
                    }

                }

                MapInfos map = new MapInfos();

                map.Name = mapName;
                map.Size = mapWidth + "/" + mapHeight;
                map.Tiles = tileList;

                string json = JsonConvert.SerializeObject(map, Formatting.Indented);
                System.IO.File.WriteAllText(saveFilePopup.FileName, json);
            }
        }

        public string removeSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
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

        // Apply the texture to the tile(s)
        private void setTileTextureApply(object sender, MouseButtonEventArgs e)
        {
            Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;

            // Get the TAG of the Rectangle (tag contains coordinates)
            String[] coo = ClickedRectangle.Tag.ToString().Split('/');
            int secondTileX = int.Parse(coo[0]);
            int secondTileY = int.Parse(coo[1]);

            // Define the smallest X and the biggest. Same for Y.
            Tuple<int, int> xLimits;
            Tuple<int, int> yLimits;

            if (firstTileX > secondTileX)
                xLimits = new Tuple<int, int>(secondTileX, firstTileX);
            else
                xLimits = new Tuple<int, int>(firstTileX, secondTileX);

            if (firstTileY > secondTileY)
                yLimits = new Tuple<int, int>(secondTileY, firstTileY);
            else
                yLimits = new Tuple<int, int>(firstTileY, secondTileY);

            foreach (WrapPanel panelChild in mapGrid.Children)
            {
                foreach (Rectangle rectangleChild in panelChild.Children)
                {
                    // Get the TAG of the Rectangle (tag contains coordinates)
                    String[] currentCoo = rectangleChild.Tag.ToString().Split('/');
                    int currentX = int.Parse(currentCoo[0]);
                    int currentY = int.Parse(currentCoo[1]);

                    if (currentX >= xLimits.Item1 && currentX <= xLimits.Item2)
                    {
                        if (currentY >= yLimits.Item1 && currentY <= yLimits.Item2)
                        {
                            rectangleChild.Fill = new SolidColorBrush(color);
                            if (globalMap[currentX, currentY] == null)
                                globalMap[currentX, currentY] = new tile();
                            globalMap[currentX, currentY].tileColor = color;
                        }
                    }
                }
            }
        }
    }
}
