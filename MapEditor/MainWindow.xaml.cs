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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string defaultColor = "#FFF4F4F5";
        public string defaultSpriteSheetFile = "../spritesheet.png";

        // Default position of the player on the spritesheet
        public const int defaultPlayerSpritePosition = 5;
        public int numberPlayerOnMap { get; set; }

        // Cancel the exit of the map editor if you click on the red cross of the popup window
        public bool cancelExit { get; set; }
        // Basically cancel the creation of a new map of the opening of a map if you click on the red cross
        public bool cancelNextAction { get; set; }
        // Disable the exit button when you create or open a new map
        public bool changeExitButton { get; set; }
        // Determine if you can click on the map (threading system)
        public bool canClick { get; set; }

        // Useful variable to determine when to trigger the save popup
        public string lastSavePath { get; set; }
        public bool hasBeenModified { get; set; }
        // Map infos
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public string mapName { get; set; }
        public string mapAudioPath { get; set; }

        public enum SpecialTile
        {
            NONE,
            HEATZONE,
            NON_COLLIDABLE
        }

        public Brush specialTile { get; set; }
        public SpecialTile specialTileType { get; set; }

        public ImageBrush sprite { get; set; }
        // Because of the bug (usedSprites, listSprites, and sprite) we need an int to know where to get the sprite from
        public int spriteInt { get; set; }

        public List<ImageBrush> listSprites = new List<ImageBrush>();
        public Dictionary<int, ImageBrush> usedSprites = new Dictionary<int, ImageBrush>();

        // Those variables are the coordinate of the rectangle on a MouseDown event
        public int firstTileX { get; set; }
        public int firstTileY { get; set; }

        tile[,] globalMap;

        // Object that will be serialized for the JSON file creation
        public class MapInfos
        {
            public string name { get; set; }
            public string size { get; set; }
            public string audio { get; set; }
            public Dictionary<int, List<tile>> tileList { get; set; }
            public List<tile> heatZonesList { get; set; }
            public List<tile> otherTileList { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
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
                tileSelectionPanel.Children.Clear();

                loadButtonsFromFile();
                loadSpecialTiles();
                gridSplitter.Visibility = Visibility.Visible;
                selectedSpriteLabel.Visibility = Visibility.Visible;
                audioButton.Visibility = Visibility.Visible;
                saveButton.IsEnabled = true;
                lastSavePath = null;
                hasBeenModified = false;
                cancelNextAction = false;
                numberPlayerOnMap = 0;

                selectedSprite.Fill = listSprites[0];
                sprite = listSprites[0];
                spriteInt = 0;

                canClick = true;

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

            canClick = false;

            usedSprites.Clear();

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
                    tileSize = 32;

                    loadButtonsFromFile();

                    foreach (var list in loadedMap.tileList)
                    {
                        var tileIndex = list.Key;
                        var tileList = list.Value;

                        foreach (tile elem in tileList)
                        {
                            tile newTile = new tile();
                            newTile.coordx = elem.coordx;
                            newTile.coordy = elem.coordy;
                            newTile.tileSprite = listSprites[tileIndex];
                            newTile.collidable = elem.collidable;
                            newTile.properties = elem.properties;

                            if (!usedSprites.ContainsKey(tileIndex))
                                usedSprites.Add(tileIndex, listSprites[tileIndex]);
                            else if (usedSprites[tileIndex] != listSprites[tileIndex])
                                usedSprites[tileIndex] = listSprites[tileIndex];

                            newGlobalMap[elem.coordx, elem.coordy] = newTile;
                        }
                    }

                    foreach (tile heatZone in loadedMap.heatZonesList)
                    {
                        if (newGlobalMap[heatZone.coordx, heatZone.coordy] != null)
                            newGlobalMap[heatZone.coordx, heatZone.coordy].heatZone = true;
                        else
                        {
                            tile newTile = new tile();
                            newTile.coordx = heatZone.coordx;
                            newTile.coordy = heatZone.coordy;
                            newTile.heatZone = true;
                            newTile.properties = heatZone.properties;

                            newGlobalMap[heatZone.coordx, heatZone.coordy] = newTile;
                        }

                    }

                    foreach (tile otherTile in loadedMap.otherTileList)
                    {
                        tile newTile = new tile();
                        newTile.coordx = otherTile.coordx;
                        newTile.coordy = otherTile.coordy;
                        newTile.collidable = otherTile.collidable;
                        newTile.properties = otherTile.properties;

                        newGlobalMap[otherTile.coordx, otherTile.coordy] = newTile;
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

                // Reapply the sprites to the map
                for (int j = 0; j < mapHeight; j++)
                {
                    WrapPanel panel = new WrapPanel();
                    for (int i = 0; i < mapWidth; i++)
                    {
                        panel.Children.Add(new Rectangle { Tag = i + "/" + (mapHeight - 1 - j), Width = tileSize, Height = tileSize, Fill = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF4F4F5"), Stroke = new SolidColorBrush(Colors.Black), Margin = new Thickness(0, 2, 2, 0) });
                    }
                    mapGrid.Children.Add(panel);
                }

                foreach (WrapPanel panelChild in mapGrid.Children)
                {
                    //foreach (Rectangle rectangleChild in panelChild.Children)
                    //Old
                    for (int i = 0; i < panelChild.Children.Count; i++)
                    {
                        Rectangle rectangleChild = (Rectangle)panelChild.Children[i];

                        // Get the TAG of the Rectangle (tag contains coordinates)
                        String[] currentCoo = rectangleChild.Tag.ToString().Split('/');
                        int currentX = int.Parse(currentCoo.First());
                        int currentY = int.Parse(currentCoo.Last());

                        if (globalMap[currentX, currentY] != null)
                        {
                            if (globalMap[currentX, currentY].tileSprite != null)
                                rectangleChild.Fill = globalMap[currentX, currentY].tileSprite;
                            if (globalMap[currentX, currentY].collidable == false)
                            {
                                rectangleChild = setGivenSpecialTile(rectangleChild, currentX, currentY, SpecialTile.NON_COLLIDABLE);
                            }
                            if (globalMap[currentX, currentY].heatZone == true)
                            {
                                rectangleChild = setGivenSpecialTile(rectangleChild, currentX, currentY, SpecialTile.HEATZONE);
                            }
                        }
                    }
                }

                // Hide the message displayed at the opening of the map editor
                informations.Visibility = Visibility.Hidden;

                // Clear existing buttons
                tileSelectionPanel.Children.Clear();

                loadButtonsFromFile();
                loadSpecialTiles();
                gridSplitter.Visibility = Visibility.Visible;
                selectedSpriteLabel.Visibility = Visibility.Visible;
                audioButton.Visibility = Visibility.Visible;
                saveButton.IsEnabled = true;
                lastSavePath = openFilePopup.FileName;
                hasBeenModified = false;

                selectedSprite.Fill = listSprites[0];
                sprite = listSprites[0];
                spriteInt = 0;

                // Thread system to prevent miss clicking while opening a map
                System.Threading.Timer timer = null;
                timer = new System.Threading.Timer((obj) =>
                {
                    canClick = true;
                    timer.Dispose();
                }, null, 500, System.Threading.Timeout.Infinite);

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

                Dictionary<int, List<tile>> sortedTileList = new Dictionary<int, List<tile>>();
                List<tile> heatZoneTileList = new List<tile>();
                List<tile> otherTileList = new List<tile>();

                for (int j = 0; j < mapHeight; j++)
                {
                    for (int i = 0; i < mapWidth; i++)
                    {
                        if (globalMap[i, j] != null)
                        {
                            tile specialTile = new tile { coordx = i, coordy = j, collidable = globalMap[i, j].collidable, properties = globalMap[i, j].properties };

                            if (globalMap[i, j].heatZone == true)
                                heatZoneTileList.Add(specialTile);
                            else
                            {
                                if (globalMap[i, j].properties != null && globalMap[i, j].tileSprite == null)
                                    otherTileList.Add(specialTile);
                            }
                        }
                    }
                }

                for (int k = 0; k < listSprites.Count; k++)
                {
                    List<tile> singleSpriteTileList = new List<tile>();

                    for (int j = 0; j < mapHeight; j++)
                    {
                        for (int i = 0; i < mapWidth; i++)
                        {
                            if (globalMap[i, j] != null)
                            {
                                if (globalMap[i, j].tileSprite != null)
                                {
                                    if (globalMap[i, j].tileSprite.ImageSource == listSprites[k].ImageSource)
                                    {
                                        singleSpriteTileList.Add(new tile()
                                        {
                                            coordx = i,
                                            coordy = j,
                                            collidable = globalMap[i, j].collidable,
                                            properties = globalMap[i, j].properties
                                        });
                                    }
                                    else
                                    {
                                        if (usedSprites.ContainsKey(k))
                                        {
                                            if (globalMap[i, j].tileSprite == usedSprites[k])
                                            {
                                                singleSpriteTileList.Add(new tile()
                                                {
                                                    coordx = i,
                                                    coordy = j,
                                                    collidable = globalMap[i, j].collidable,
                                                    properties = globalMap[i, j].properties
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (singleSpriteTileList.Count != 0)
                        sortedTileList.Add(k, singleSpriteTileList);

                }

                MapInfos map = new MapInfos();

                map.name = mapName;
                map.size = mapWidth + "/" + mapHeight;
                map.audio = mapAudioPath;
                map.tileList = sortedTileList;
                map.heatZonesList = heatZoneTileList;
                map.otherTileList = otherTileList;

                string json = JsonConvert.SerializeObject(map, Formatting.Indented);
                File.WriteAllText(savePath, json);

                lastSavePath = savePath;
                hasBeenModified = false;
            }
        }

        // Load the buttons from a txt file
        private void loadButtonsFromFile()
        {
            if (File.Exists(defaultSpriteSheetFile))
            {
                WrapPanel panel = new WrapPanel();
                BitmapImage spriteSheet = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultSpriteSheetFile)));

                listSprites.Clear();

                for (int i = 0; i * 16 < spriteSheet.Width; i++)
                {
                    CroppedBitmap singleSprite = new CroppedBitmap(spriteSheet, new Int32Rect(i * 16, 0, 16, 16));

                    Rectangle spriteRectangle = new Rectangle { Width = 48, Height = 48, Stroke = new SolidColorBrush(Colors.Black), Margin = new Thickness(5, 5, 5, 5) };
                    spriteRectangle.Fill = new ImageBrush(singleSprite);
                    spriteRectangle.MouseLeftButtonDown += spriteButton_Click;
                    spriteRectangle.Tag = i;

                    listSprites.Add(new ImageBrush(singleSprite));

                    if (!usedSprites.ContainsKey(i))
                        usedSprites.Add(i, new ImageBrush(singleSprite));

                    panel.Children.Add(spriteRectangle);
                }

                tileSelectionPanel.Children.Add(panel);
            }
            else
            {
                NoConfigFilePopup popup = new NoConfigFilePopup();
                string[] configFilePath = defaultSpriteSheetFile.Split('/');

                popup.setContent(configFilePath.Last());
                popup.Owner = this;
                popup.ShowDialog();

                if (!string.IsNullOrEmpty(popup.fileName))
                {
                    defaultSpriteSheetFile = popup.fileName;
                    this.loadButtonsFromFile();
                }
            }

        }

        // Load special tiles like Heat zones
        private void loadSpecialTiles()
        {
            WrapPanel panel = new WrapPanel();

            Label heatZoneLabel = new Label { Content = "Heat Zone :", FontSize = 15, Height = 48, VerticalContentAlignment = VerticalAlignment.Center };
            Rectangle heatZoneRectangle = new Rectangle { Width = 48, Height = 48, StrokeThickness = 2, Stroke = new SolidColorBrush(Colors.Orange), Fill = new SolidColorBrush(Colors.White), Margin = new Thickness(5, 5, 5, 5), Name = SpecialTile.HEATZONE.ToString() };
            heatZoneRectangle.MouseLeftButtonDown += specialTile_Click;

            Label nonCollidableBlockLabel = new Label { Content = "Non-Collidable Block :", FontSize = 15, Height = 48, VerticalContentAlignment = VerticalAlignment.Center };
            Rectangle nonCollidableBlockRectangle = new Rectangle { Width = 48, Height = 48, StrokeThickness = 2, Stroke = new SolidColorBrush(Colors.DarkGray), Fill = new SolidColorBrush(Colors.White), Margin = new Thickness(5, 5, 5, 5), Name = SpecialTile.NON_COLLIDABLE.ToString() };
            nonCollidableBlockRectangle.MouseLeftButtonDown += specialTile_Click;

            panel.Children.Add(heatZoneLabel);
            panel.Children.Add(heatZoneRectangle);

            panel.Children.Add(nonCollidableBlockLabel);
            panel.Children.Add(nonCollidableBlockRectangle);

            tileSelectionPanel.Children.Add(panel);
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
            if (canClick == true)
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
                        globalMap[secondTileX, secondTileY] = new tile { collidable = true };

                    // Check if it's a special tile or not
                    if (specialTile != null)
                        ClickedRectangle = setSpecialTile(ClickedRectangle,secondTileX, secondTileY);
                    else
                        ClickedRectangle.Fill = setRectangleSprite(secondTileX, secondTileY);
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
                        //foreach (Rectangle rectangleChild in panelChild.Children)
                        //Au dessus c'est old mais on garde au cas ou
                        for (int i = 0; i < panelChild.Children.Count; i++)
                        {
                            // Get the rectangle as we don't use a foreach anymore because of C#
                            Rectangle rectangleChild = (Rectangle)panelChild.Children[i];

                            // Get the TAG of the Rectangle (tag contains coordinates)
                            String[] currentCoo = rectangleChild.Tag.ToString().Split('/');
                            int currentX = int.Parse(currentCoo.First());
                            int currentY = int.Parse(currentCoo.Last());

                            if (currentX >= xLimits.Item1 && currentX <= xLimits.Item2)
                            {
                                if (currentY >= yLimits.Item1 && currentY <= yLimits.Item2)
                                {
                                    if (globalMap[currentX, currentY] == null)
                                        globalMap[currentX, currentY] = new tile { collidable = true };

                                    // Check if it's a special tile or not
                                    if (specialTile != null)
                                        rectangleChild = setSpecialTile(rectangleChild, currentX, currentY);
                                    else
                                        rectangleChild.Fill = setRectangleSprite(currentX, currentY);
                                }
                            }
                        }
                    }
                }

                firstTileX = -1;
                firstTileY = -1;
                hasBeenModified = true;
            }
        }

        // Define the tile's sprite
        private Brush setRectangleSprite(int x, int y)
        {
            // If the tile we click on == the one we selected we need to unset it
            if (globalMap[x, y].tileSprite == sprite || globalMap[x, y].tileSprite == usedSprites[spriteInt])
            {
                // Check if the selected sprite is the one of a player
                if (sprite.ImageSource == listSprites[defaultPlayerSpritePosition].ImageSource)
                    numberPlayerOnMap--;

                globalMap[x, y] = null;
                return ((SolidColorBrush)(new BrushConverter().ConvertFrom(defaultColor)));
            }

            // Check if we are putting a player
            if (sprite.ImageSource == listSprites[defaultPlayerSpritePosition].ImageSource && numberPlayerOnMap > 0)
            {
                if (globalMap[x, y].tileSprite == null)
                    return ((SolidColorBrush)(new BrushConverter().ConvertFrom(defaultColor)));
                return (globalMap[x, y].tileSprite);
            }
            else if (sprite.ImageSource == listSprites[defaultPlayerSpritePosition].ImageSource)
                selectedSprite.IsEnabled = false;
            //numberPlayerOnMap++;

            if (globalMap[x, y].tileSprite == listSprites[defaultPlayerSpritePosition])
                if (sprite.ImageSource != listSprites[defaultPlayerSpritePosition].ImageSource)
                    numberPlayerOnMap--;

            globalMap[x, y].tileSprite = sprite;
            return (sprite);

        }

        // Set the special tile to the rectangle
        private Rectangle setSpecialTile(Rectangle rectangle, int x, int y)
        {
            if (specialTileType == SpecialTile.HEATZONE)
            {
                if (rectangle.Name == SpecialTile.HEATZONE.ToString())
                {
                    rectangle.Name = null;
                    rectangle.Stroke = new SolidColorBrush(Colors.Black);
                    rectangle.StrokeThickness = 1;
                    globalMap[x, y].heatZone = false;
                }
                else
                {
                    rectangle.Name = SpecialTile.HEATZONE.ToString();
                    rectangle.Stroke = new SolidColorBrush(Colors.Orange);
                    rectangle.StrokeThickness = 2;
                    globalMap[x, y].heatZone = true;
                }
            }

            if (specialTileType == SpecialTile.NON_COLLIDABLE)
            {
                if (rectangle.Name == SpecialTile.NON_COLLIDABLE.ToString())
                {
                    rectangle.Name = null;
                    rectangle.Stroke = new SolidColorBrush(Colors.Black);
                    rectangle.StrokeThickness = 1;
                    globalMap[x, y].collidable = true;
                }
                else
                {
                    rectangle.Name = SpecialTile.NON_COLLIDABLE.ToString();
                    rectangle.Stroke = new SolidColorBrush(Colors.DarkGray);
                    rectangle.StrokeThickness = 3;
                    globalMap[x, y].collidable = false;
                }
            }

            return (rectangle);
        }

        // Set the special tile to the rectangle
        private Rectangle setGivenSpecialTile(Rectangle rectangle, int x, int y, SpecialTile SpecialTile)
        {
            if (SpecialTile == SpecialTile.HEATZONE)
            {
                    rectangle.Name = SpecialTile.HEATZONE.ToString();
                    rectangle.Stroke = new SolidColorBrush(Colors.Orange);
                    rectangle.StrokeThickness = 2;
                    globalMap[x, y].heatZone = true;
            }

            if (SpecialTile == SpecialTile.NON_COLLIDABLE)
            {
                    rectangle.Name = SpecialTile.NON_COLLIDABLE.ToString();
                    rectangle.Stroke = new SolidColorBrush(Colors.DarkGray);
                    rectangle.StrokeThickness = 3;
                    globalMap[x, y].collidable = false;
            }

            return (rectangle);
        }

        // When you click on a sprite from the list
        private void spriteButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Shapes.Rectangle ClickedSprite = (System.Windows.Shapes.Rectangle)e.OriginalSource;

            spriteInt = (int)ClickedSprite.Tag;
            sprite = listSprites[spriteInt];//(ImageBrush)ClickedSprite.Fill;
            // Reset the special tile
            specialTile = null;
            selectedSprite.Fill = ClickedSprite.Fill;
            selectedSprite.Stroke = null;
            selectedSprite.StrokeThickness = 0;
        }

        // WHen you click on a special tile from the list
        private void specialTile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Shapes.Rectangle ClickedTile = (System.Windows.Shapes.Rectangle)e.OriginalSource;

            if (ClickedTile.Name == SpecialTile.HEATZONE.ToString())
            {
                specialTileType = SpecialTile.HEATZONE;
            }

            if (ClickedTile.Name == SpecialTile.NON_COLLIDABLE.ToString())
            {
                specialTileType = SpecialTile.NON_COLLIDABLE;
            }

            specialTile = ClickedTile.Fill;
            selectedSprite.Fill = ClickedTile.Fill;
            selectedSprite.Stroke = ClickedTile.Stroke;
            selectedSprite.StrokeThickness = ClickedTile.StrokeThickness;
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

        // Set the properties of a tile after right cliking it
        private void setTileProperties(object sender, MouseButtonEventArgs e)
        {
            Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;

            int x = 0, y = 0;
            String[] coo = ClickedRectangle.Tag.ToString().Split('/');
            if (coo != null && coo.Length != 0)
            {
                x = int.Parse(coo.First());
                y = int.Parse(coo.Last());
            }

            TileProperties tileProperties = new TileProperties();
            TilePropertiesWindow tilePropertiesWindow;

            if (globalMap[x, y] != null)
            {
                if (globalMap[x, y].properties == null)
                    tilePropertiesWindow = new TilePropertiesWindow(x, y, mapWidth, mapHeight, ClickedRectangle);
                else
                    tilePropertiesWindow = new TilePropertiesWindow(x, y, mapWidth, mapHeight, globalMap[x, y], ClickedRectangle);
            }
            else
            {
                globalMap[x, y] = new tile { coordx = x, coordy = y, collidable = false };
                tilePropertiesWindow = new TilePropertiesWindow(x, y, mapWidth, mapHeight, ClickedRectangle);
            }

            tilePropertiesWindow.Owner = this;
            tilePropertiesWindow.ShowDialog();

            if (tilePropertiesWindow.set == true)
                globalMap[x, y].properties = tilePropertiesWindow.tileProperties;
        }
    }
}
