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
    /// Logique d'interaction pour NewMap.xaml
    /// </summary>
    public partial class NewMap : Window
    {
        public string mapName { get; set; }
        public int xSize { get; set; }
        public int ySize { get; set; }

        public NewMap()
        {
            InitializeComponent();
        }

        // When the user validates the popup.
        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(mapNameTextBox.Text))
                mapName = mapNameTextBox.Text;
            else
                mapNameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            if (!string.IsNullOrEmpty(xSizeTextBox.Text))
            {
                xSize = int.Parse(xSizeTextBox.Text);
                if (xSize <= 0 || xSize > 50)
                    xSizeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            if (!string.IsNullOrEmpty(ySizeTextBox.Text))
            {
                ySize = int.Parse(ySizeTextBox.Text);
                if (ySize <= 0 || ySize > 50)
                    ySizeTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }

            if (!string.IsNullOrEmpty(mapName) && xSize != 0 && ySize != 0)
                this.Close();
        }

        // When the user wants to cancel and close the popup.
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            mapName = null;
            xSize = 0;
            ySize = 0;
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
