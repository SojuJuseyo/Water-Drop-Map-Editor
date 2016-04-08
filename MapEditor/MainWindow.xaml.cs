using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

/*
            BitmapImage kappaImage = new BitmapImage(new Uri("C:/Users/Thomas/Desktop/Sprites.png"));
            CroppedBitmap croppedKappa = new CroppedBitmap(kappaImage, new Int32Rect(0, 0, 16, 16));
            ImageBrush kappaBrush = new ImageBrush(croppedKappa);
            RectangleTest.Fill = kappaBrush;
*/

namespace MapEditor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string defaultColor = "#FFF4F4F5";
        public string defaultColorFile = "../settings.txt";

        // Cancel the exit of the map editor if you click on the red cross of the popup window
        public bool cancelExit { get; set; }
        // Basically cancel the creation of a new map of the opening of a map if you click on the red cross
        public bool cancelNextAction { get; set; }
        // Disable the exit button when you create or open a new map
        public bool changeExitButton { get; set; }

        // Useful variable to determine when to trigger the save popup
        public string lastSavePath { get; set; }
        public bool hasBeenModified { get; set; }
        // Map infos
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public string mapName { get; set; }
        public string mapAudioPath { get; set; }
        // To delete after (when there will be textures)
        public Color color { get; set; }
        // To change after
        public List<Color> usedColors = new List<Color>();

        // Those variables are the coordinate of the rectangle on a MouseDown event
        public int firstTileX { get; set; }
        public int firstTileY { get; set; }

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
            public string audio { get; set; }
            public Dictionary<Color, List<tile>> tileList { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        // Creation of a new map
        private void MenuFileNew_Click(object sender, RoutedEventArgs e)
        {
            if (hasBeenModified == true)
            {
                changeExitButton = true;
                editorClosing(this, new System.ComponentModel.CancelEventArgs());
                if (cancelNextAction == true)
                    return;
            }

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
                int tileSize = 32;
                
                if (mapWidth > 16 || mapHeight > 16)
                    gridSplitter.Margin = new Thickness(430, 27, 0, 0);
                else
                    gridSplitter.Margin = new Thickness(445, 27, 0, 0);
                    
                for (int j = 0; j < mapHeight; j++)
                {
                    WrapPanel panel = new WrapPanel();
                    for (int i = 0; i < mapWidth; i++)
                    {
                        panel.Children.Add(new Rectangle { Tag = i + "/" + (mapHeight - j - 1), Width = tileSize, Height = tileSize, Fill = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF4F4F5"), Stroke = new SolidColorBrush(Colors.Black), Margin = new Thickness(0, 2, 2, 0) });
                    }
                    mapGrid.Children.Add(panel);
                }

                // Clear existing buttons
                colorsPanel.Children.Clear();

                loadButtonsFromFile();
                gridSplitter.Visibility = Visibility.Visible;
                selectedColorLabel.Visibility = Visibility.Visible;
                audioButton.Visibility = Visibility.Visible;
                saveButton.IsEnabled = true;
                lastSavePath = null;
                hasBeenModified = false;
                cancelNextAction = false;
            }

            firstTileX = -1;
            firstTileY = -1;
        }

        // Open an existing JSON saved map
        private void MenuFileNew_Open(object sender, RoutedEventArgs e)
        {
            if (hasBeenModified == true)
            {
                changeExitButton = true;
                editorClosing(this, new System.ComponentModel.CancelEventArgs());
                if (cancelNextAction == true)
                    return;
            }

            System.Windows.Forms.OpenFileDialog openFilePopup = new System.Windows.Forms.OpenFileDialog();
            int newMapWidth, newMapHeight, tileSize = 0;
            string newMapName;

            openFilePopup.DefaultExt = ".json";
            openFilePopup.Filter = "JSON documents (.json)|*.json";
            openFilePopup.Title = "Open a map";
            openFilePopup.ShowDialog();

            if (openFilePopup.FileName != "")
            {
                tile[,] newGlobalMap;

                // Error handling
                try
                {
                    MapInfos loadedMap = JsonConvert.DeserializeObject<MapInfos>(File.ReadAllText(openFilePopup.FileName));

                    // Loading the informations from the deserialized object
                    String[] size = loadedMap.size.Split('/');

                    newMapWidth = int.Parse(size.First());
                    newMapHeight = int.Parse(size.Last());
                    newMapName = loadedMap.name;
                    mapAudioPath = loadedMap.audio;

                    newGlobalMap = new tile[newMapWidth, newMapHeight];

                    // Deciding the size of the rectangles (tiles)
                    if (newMapWidth > newMapHeight)
                        tileSize = (550 - newMapWidth * 2) / (newMapWidth);
                    else
                        tileSize = (550 - newMapHeight * 2) / (newMapHeight);

                    if (tileSize < 25)
                    {
                        tileSize = 25;
                        gridSplitter.Margin = new Thickness(430, 27, 0, 0);
                    }
                    else
                        gridSplitter.Margin = new Thickness(445, 27, 0, 0);

                    foreach (var list in loadedMap.tileList)
                    {
                        var tileColor = list.Key;
                        var tileList = list.Value;

                        foreach (tile elem in tileList)
                        {
                            tile newTile = new tile();
                            newTile.coordx = elem.coordx;
                            newTile.coordy = elem.coordy;
                            newTile.tileColor = tileColor;

                            newGlobalMap[elem.coordx, elem.coordy] = newTile;
                        }
                    }

                    // Re-set the values to the globalMap
                    foreach (var list in loadedMap.tileList)
                    {
                        var tileColor = list.Key;
                        var tileList = list.Value;

                        foreach (tile elem in tileList)
                        {
                            tile newTile = new tile();
                            newTile.coordx = elem.coordx;
                            newTile.coordy = elem.coordy;
                            newTile.tileColor = tileColor;

                            newGlobalMap[elem.coordx, elem.coordy] = newTile;
                        }
                    }

                }
                catch (Exception)
                {
                    GenericErrorPopup errorPopup = new GenericErrorPopup();

                    errorPopup.setErrorMessage("Error opening a map", "The map you're trying to open is corrupted.");
                    errorPopup.Owner = this;
                    errorPopup.ShowDialog();
                    return;
                }

                globalMap = newGlobalMap;
                mapName = newMapName;
                mapHeight = newMapHeight;
                mapWidth = newMapWidth;

                try {
                    // Setting the mapName as the title of the window
                    this.Title = newMapName;
                }
                catch (Exception) { }

                // Clear the potential already existing map
                mapGrid.Children.Clear();

                // Reapply the colors to the map
                for (int j = 0; j < mapHeight; j++)
                {
                    WrapPanel panel = new WrapPanel();
                    for (int i = 0; i < mapWidth; i++)
                    {
                        panel.Children.Add(new Rectangle { Tag = i + "/" + (mapHeight - 1 - j), Width = tileSize, Height = tileSize, Fill = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF4F4F5"), Stroke = new SolidColorBrush(Colors.Black), /*RadiusX = 10, RadiusY = 10,*/ Margin = new Thickness(0, 2, 2, 0) });
                    }
                    mapGrid.Children.Add(panel);
                }

                foreach (WrapPanel panelChild in mapGrid.Children)
                {
                    foreach (Rectangle rectangleChild in panelChild.Children)
                    {
                        // Get the TAG of the Rectangle (tag contains coordinates)
                        String[] currentCoo = rectangleChild.Tag.ToString().Split('/');
                        int currentX = int.Parse(currentCoo.First());
                        int currentY = int.Parse(currentCoo.Last());

                        if (newGlobalMap[currentX, currentY] != null)
                        {
                            if (usedColors.IndexOf(globalMap[currentX, currentY].tileColor) < 0)
                                usedColors.Add(globalMap[currentX, currentY].tileColor);
                            rectangleChild.Fill = new SolidColorBrush(globalMap[currentX, currentY].tileColor);
                        }
                    }
                }

                // Hide the message displayed at the opening of the map editor
                informations.Visibility = Visibility.Hidden;

                // Clear existing buttons
                colorsPanel.Children.Clear();

                loadButtonsFromFile();
                gridSplitter.Visibility = Visibility.Visible;
                selectedColorLabel.Visibility = Visibility.Visible;
                audioButton.Visibility = Visibility.Visible;
                saveButton.IsEnabled = true;
                lastSavePath = openFilePopup.FileName;
                hasBeenModified = false;

                firstTileX = -1;
                firstTileY = -1;
            }
        }

        // To save a map
        private void MenuFileNew_Save(object sender, RoutedEventArgs e)
        {
            // Popup to select the location of the file
            System.Windows.Forms.SaveFileDialog saveFilePopup = new System.Windows.Forms.SaveFileDialog();

            string savePath;

            if (String.IsNullOrEmpty(lastSavePath))
            {
                saveFilePopup.DefaultExt = ".json";
                saveFilePopup.Filter = "JSON documents (.json)|*.json";
                saveFilePopup.Title = "Save your map";
                saveFilePopup.FileName = removeSpecialCharacters(mapName);
                if (saveFilePopup.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    cancelExit = true;
                    return;
                }
            }

            if (saveFilePopup.FileName != "" || !String.IsNullOrEmpty(lastSavePath))
            {
                savePath = (saveFilePopup.FileName != "" ? saveFilePopup.FileName : lastSavePath);

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
                map.audio = mapAudioPath;
                map.tileList = sortedTileList;

                string json = JsonConvert.SerializeObject(map, Formatting.Indented);
                File.WriteAllText(savePath, json);

                lastSavePath = savePath;
                hasBeenModified = false;
            }
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
                            System.Windows.Controls.Button newButton = new System.Windows.Controls.Button { Content = extractedColor[0], Tag = extractedColor[1], Width = 50, Height = 50, Margin = new Thickness(5, 5, 5, 0) };
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

        // Remove special characters
        public string removeSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        // Check the validity of an string (hexadecimal or not)
        public bool isStringHexadecimal(string str)
        {
            return Regex.IsMatch(str, "^#(([0-9a-fA-F]{2}){3}|([0-9a-fA-F]){3})$");
        }

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
            hasBeenModified = true;
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
            System.Windows.Controls.Button ClickedButton = (System.Windows.Controls.Button)e.OriginalSource;

            color = (Color)ColorConverter.ConvertFromString(ClickedButton.Tag.ToString());
            selectedColor.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(ClickedButton.Tag.ToString());
        }

        // Display a menu to chose between creating a new map or loading an existing saved one
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainMenu mainMenu = new MainMenu();

            mainMenu.Owner = this;
            mainMenu.ShowDialog();

            switch (mainMenu.action)
            {
                case MainMenu.Action.CREATE:
                    {
                        newButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.MenuItem.ClickEvent));
                        break;
                    }
                case MainMenu.Action.OPEN:
                    {
                        openButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.MenuItem.ClickEvent));
                        break;
                    }
                case MainMenu.Action.EXIT:
                    {
                        this.Close();
                        break;
                    }
                case MainMenu.Action.NOTHING:
                    {
                        break;
                    }
            }
        }

        // Handle the exit of the map editor
        private void editorClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (hasBeenModified == true)
            {
                SavePopup savePopup = new SavePopup();

                savePopup.setCurrentLocation(lastSavePath);
                savePopup.Owner = this;

                // To change the button exit in a "No" button when you're creating or opening a map
                if (changeExitButton == true)
                {
                    savePopup.exitButton.Content = "No";
                    savePopup.changeEventExitButton();
                }
                changeExitButton = false;

                // In case you press the red cross when opening or creating a map
                cancelNextAction = false;

                savePopup.ShowDialog();

                switch (savePopup.action)
                {
                    case SavePopup.Action.CHANGEPATH:
                        {
                            lastSavePath = null;
                            saveButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.MenuItem.ClickEvent));
                            if (cancelExit == true)
                            {
                                cancelExit = false;
                                e.Cancel = true;
                            }
                            break;
                        }
                    case SavePopup.Action.SAVE:
                        {
                            saveButton.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.MenuItem.ClickEvent));
                            break;
                        }
                    case SavePopup.Action.NOTHING:
                        {
                            cancelNextAction = true;
                            e.Cancel = true;
                            break;
                        }
                }
            }
        }

        // Click on the audio button
        private void audioButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFilePopup = new System.Windows.Forms.OpenFileDialog();

            openFilePopup.DefaultExt = ".wav";
            openFilePopup.Filter = "All Supported Audio | *.wav";
            openFilePopup.Title = "Open an audio file";
            openFilePopup.ShowDialog();

            if (openFilePopup.FileName != "")
                mapAudioPath = openFilePopup.FileName;
        }
    }
}
