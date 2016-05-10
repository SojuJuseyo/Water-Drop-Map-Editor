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
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Logique d'interaction pour TilePropertiesWindow.xaml
    /// </summary>
    public partial class TilePropertiesWindow : Window
    {
        public TileProperties tileProperties = new TileProperties();

        private int maxWidth { get; set; }
        private int maxHeight { get; set; }

        public bool set { get; set; }

        public TilePropertiesWindow()
        {
            InitializeComponent();
        }

        public TilePropertiesWindow(int x, int y, int mapWidth, int mapHeight, Rectangle clickedRectangle)
        {
            InitializeComponent();

            mainLabel.Content = "Tile [" + x + "," + y + "] Properties";
            x1TextBox.Text = x.ToString();
            y1TextBox.Text = y.ToString();
            x2TextBox.Text = x.ToString();
            y2TextBox.Text = y.ToString();

            spritePreview.Fill = clickedRectangle.Fill;
            spritePreview.Stroke = clickedRectangle.Stroke;
            spritePreview.StrokeThickness = clickedRectangle.StrokeThickness;

            maxWidth = mapWidth;
            maxHeight = mapHeight;

            set = false;
        }

        // If the tile already has properties
        public TilePropertiesWindow(int x, int y, int mapWidth, int mapHeight, tile clickedTile, Rectangle clickedRectangle)
        {
            InitializeComponent();

            mainLabel.Content = "Tile [" + x + "," + y + "] Properties";
            textTextBox.Text = clickedTile.properties.text;
            x1TextBox.Text = x.ToString();
            y1TextBox.Text = y.ToString();
            x2TextBox.Text = clickedTile.properties.x2.ToString();
            y2TextBox.Text = clickedTile.properties.y2.ToString();

            spritePreview.Fill = clickedRectangle.Fill;
            spritePreview.Stroke = clickedRectangle.Stroke;
            spritePreview.StrokeThickness = clickedRectangle.StrokeThickness;

            maxWidth = mapWidth;
            maxHeight = mapHeight;

            set = false;
        }

        // After validation
        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            set = true;
            tileProperties.x2 = -1;
            tileProperties.y2 = -1;

            tileProperties.text = textTextBox.Text;

            if (!string.IsNullOrEmpty(x2TextBox.Text) && x2TextBox.Text.All(char.IsDigit))
            {
                tileProperties.x2 = int.Parse(x2TextBox.Text);
                if (tileProperties.x2 < 0 || tileProperties.x2 >= maxWidth)
                {
                    x2TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    tileProperties.x2 = -1;
                }
                else
                    x2TextBox.BorderBrush = new SolidColorBrush(Colors.DarkGreen);

            }
            else
                x2TextBox.BorderBrush = new SolidColorBrush(Colors.Red);

            if (!string.IsNullOrEmpty(y2TextBox.Text) && y2TextBox.Text.All(char.IsDigit))
            {
                tileProperties.y2 = int.Parse(y2TextBox.Text);
                if (tileProperties.y2 < 0 || tileProperties.y2 >= maxHeight)
                {
                    y2TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    tileProperties.y2 = -1;
                }
                else
                    y2TextBox.BorderBrush = new SolidColorBrush(Colors.DarkGreen);

            }
            else
                y2TextBox.BorderBrush = new SolidColorBrush(Colors.Red);

            if (tileProperties.x2 != -1 && tileProperties.y2 != -1)
                this.Close();
        }

        // When the user wants to cancel and close the popup.
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // To make sure the input only accept numbers.
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
