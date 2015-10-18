﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public const string defaultColor = "#FFF4F4F5";
        public string defaultColorFile = "../settings.txt";

        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public string mapName { get; set; }
        // To delete after (when there will be textures)
        public Color color { get; set; }
        // To change after
        public List<Color> usedColors = new List<Color>(); 

        public class tile
        {
            [JsonIgnore]
            public Color tileColor { get; set; }
            public int coordx { get; set; }
            public int coordy { get; set; }
        }

        tile[,] globalMap;

        // Object that will be serialized for the JSON file creation
        public class MapInfos
        {
            public string name { get; set; }
            public string size { get; set; }
            public Dictionary<Color, List<tile>> tileList { get; set; }
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

            if (createNewMapWindow.xSize > 0 && createNewMapWindow.ySize > 0)
            {
                informations.Visibility = Visibility.Hidden;

                // After the popup is closed
                mapWidth = createNewMapWindow.xSize;
                mapHeight = createNewMapWindow.ySize;
                mapName = createNewMapWindow.mapName;
                mapGrid.Children.Clear();
                globalMap = new tile[mapWidth, mapHeight];
                
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

                // Clear existing buttons
                colorsPanel.Children.Clear();

                loadButtonsFromFile();
                gridSplitter.Visibility = Visibility.Visible;
                selectedColorLabel.Visibility = Visibility.Visible;
                saveButton.IsEnabled = true;
            }

            firstTileX = -1;
            firstTileY = -1;
        }

        // Load the buttons from a txt file
        private void loadButtonsFromFile()
        {
            int i = 0;
            string line;

            if (File.Exists(defaultColorFile))
            {
                StreamReader file = new StreamReader(defaultColorFile);

                WrapPanel panel = new WrapPanel();
                while ((line = file.ReadLine()) != null)
                {
                    String[] extractedColor = line.Split('|');
                    if (extractedColor.Length == 2)
                    {
                        if (isStringHexadecimal(extractedColor[1]))
                        {
                            Button newButton = new Button { Content = extractedColor[0], Tag = extractedColor[1], Width = 50, Height = 50, Margin = new Thickness(5, 5, 5, 0) };
                            newButton.Click += coloredButton_Click;
                            panel.Children.Add(newButton);
                        }
                    }
                    i++;
                }
                colorsPanel.Children.Add(panel);

                file.Close();
            }
            else
            {
                NoConfigFilePopup popup = new NoConfigFilePopup();
                string[] configFilePath = defaultColorFile.Split('/');

                popup.setContent(configFilePath.Last());
                popup.Owner = this;
                popup.ShowDialog();

                if (!string.IsNullOrEmpty(popup.fileName))
                {
                    defaultColorFile = popup.fileName;
                    this.loadButtonsFromFile();
                }
            }
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
                Dictionary<Color, List<tile>> sortedTileList = new Dictionary<Color, List<tile>>();

                foreach (Color tileColor in usedColors)
                {
                    List<tile> singleColorTileList = new List<tile>();

                    for (int j = 0; j < mapHeight; j++)
                    {
                        for (int i = 0; i < mapWidth; i++)
                        {
                            if (globalMap[i, j] != null)
                            {
                                if (globalMap[i, j].tileColor == tileColor)
                                {
                                    singleColorTileList.Add(new tile()
                                    {
                                        coordx = i,
                                        coordy = j
                                    });
                                }
                            }
                        }
                    }

                    sortedTileList.Add(tileColor, singleColorTileList);
                }

                MapInfos map = new MapInfos();

                map.name = mapName;
                map.size = mapWidth + "/" + mapHeight;
                map.tileList = sortedTileList;

                string json = JsonConvert.SerializeObject(map, Formatting.Indented);
                System.IO.File.WriteAllText(saveFilePopup.FileName, json);
            }
        }

        // Remove special characters
        public string removeSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        // Check the validity of an string (hexadecimal or not)
        public bool isStringHexadecimal(string test)
        {
            return Regex.IsMatch(test, "^#(([0-9a-fA-F]{2}){3}|([0-9a-fA-F]){3})$");
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
            if (coo != null && coo.Length != 0)
            {
                firstTileX = int.Parse(coo.First());
                firstTileY = int.Parse(coo.Last());
            }
        }

        // Apply the texture to the tile(s)
        private void setTileTextureApply(object sender, MouseButtonEventArgs e)
        {
            Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;

            // Get the TAG of the Rectangle (tag contains coordinates)
            String[] coo = ClickedRectangle.Tag.ToString().Split('/');
            int secondTileX = int.Parse(coo.First());
            int secondTileY = int.Parse(coo.Last());

            // Shortly : MouseDown outside of a rectangle and MouseUp on one
            if (firstTileX == -1 && firstTileY == -1)
            {
                if (globalMap[secondTileX, secondTileY] == null)
                    globalMap[secondTileX, secondTileY] = new tile();
                ClickedRectangle.Fill = setRectangleColor(secondTileX, secondTileY);
                // If the color was never used before, add it to the list of usedColors
                if (usedColors.IndexOf(color) < 0)
                    usedColors.Add(color);
            }
            else
            {
                // Define the smallest X and the biggest. Same for Y
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
                        int currentX = int.Parse(currentCoo.First());
                        int currentY = int.Parse(currentCoo.Last());

                        if (currentX >= xLimits.Item1 && currentX <= xLimits.Item2)
                        {
                            if (currentY >= yLimits.Item1 && currentY <= yLimits.Item2)
                            {
                                if (globalMap[currentX, currentY] == null)
                                    globalMap[currentX, currentY] = new tile();
                                rectangleChild.Fill = setRectangleColor(currentX, currentY);
                                // If the color was never used before, add it to the list of usedColors
                                if (usedColors.IndexOf(color) < 0)
                                    usedColors.Add(color);
                            }
                        }
                    }
                }
            }

            firstTileX = -1;
            firstTileY = -1;
        }

        // Define the tile's color
        private SolidColorBrush setRectangleColor(int x, int y)
        {
            if (globalMap[x, y].tileColor == color)
            {
                globalMap[x, y] = null;
                return (SolidColorBrush)(new BrushConverter().ConvertFrom(defaultColor));
            }
            globalMap[x, y].tileColor = color;
            return (new SolidColorBrush(color));
        }

        private void coloredButton_Click(object sender, RoutedEventArgs e)
        {
            Button ClickedButton = (Button)e.OriginalSource;

            color = (Color)ColorConverter.ConvertFromString(ClickedButton.Tag.ToString());
            selectedColor.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(ClickedButton.Tag.ToString());
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new MainMenu();

            mainMenu.Owner = this;
            mainMenu.ShowDialog();

            switch (mainMenu.action)
            {
                case MainMenu.Action.CREATE:
                    {
                        newButton.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                        break;
                    }
                case MainMenu.Action.OPEN:
                    {
                        break;
                    }
                case MainMenu.Action.EXIT:
                    {
                        this.Close();
                        break;
                    }
            }
        }
    }
}
