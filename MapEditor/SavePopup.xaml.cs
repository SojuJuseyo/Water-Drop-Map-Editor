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
            EXIT,
            NOTHING
        }

        public Action action { get; set; }
        // Manual way of doing magical stuff
        public Boolean isClosing { get; set; }

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
            isClosing = true;
            action = Action.EXIT;
            this.Close();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Je suis dans le no");
            isClosing = true;
            action = Action.EXIT;
            this.Close();
        }

        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            isClosing = true;
            action = Action.CHANGEPATH;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            isClosing = true;
            action = Action.SAVE;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosing == false)
                action = Action.NOTHING;
        }

        public void changeEventExitButton()
        {
            exitButton.Click += new RoutedEventHandler(noButton_Click);
        }
    }
}
