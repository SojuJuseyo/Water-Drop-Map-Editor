using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Logique d'interaction pour NoConfigFilePopup.xaml
    /// </summary>
    public partial class NoConfigFilePopup : Window
    {
        public string fileName { get; set; }

        public NoConfigFilePopup()
        {
            InitializeComponent();
        }

        public void setContent(string fileName)
        {
            errorMessage.Content = "Config file " + fileName + " not found.";
        }

        // Validate and open the selected settings file
        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Cancel the opening of a settings file
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            fileName = null;
            this.Close();
        }

        // Select a settings file to open
        private void browseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFilePopup = new OpenFileDialog();

            openFilePopup.Filter = "Text files (*.txt)|*.txt";
            openFilePopup.Title = "Open a settings file";
            if (openFilePopup.ShowDialog() == true)
            {
                selectedFile.Content = openFilePopup.SafeFileName;
                fileName = openFilePopup.FileName;
            }
        }
    }
}
