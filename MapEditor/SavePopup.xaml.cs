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
    /// Logique d'interaction pour SavePopup.xaml
    /// </summary>
    public partial class SavePopup : Window
    {
        public enum Action
        {
            CHANGEPATH,
            SAVE,
            EXIT
        }

        public Action action { get; set; }

        public SavePopup()
        {
            InitializeComponent();
        }

        public void setCurrentLocation(string location)
        {
            if (String.IsNullOrEmpty(location))
            {
                currentLocationTextBlock.Text = "No save path defined";
                saveButton.IsEnabled = false;
            }
            else
                currentLocationTextBlock.Text = "Save path : " + location;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            action = Action.EXIT;
            this.Close();
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            action = Action.CHANGEPATH;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            action = Action.SAVE;
            this.Close();
        }
    }
}
