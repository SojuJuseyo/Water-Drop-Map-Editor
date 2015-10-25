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
    /// Logique d'interaction pour MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public enum Action
        {
            CREATE,
            OPEN,
            EXIT,
            NOTHING
        }

        public Action action { get; set; }
        // Manual way of doing magical stuff
        public Boolean isClosing { get; set; }

        public MainMenu()
        {
            InitializeComponent();
        }

        private void createMapButton_Click(object sender, RoutedEventArgs e)
        {
            isClosing = true;
            action = Action.CREATE;
            this.Close();
        }

        private void openMapButton_Click(object sender, RoutedEventArgs e)
        {
            isClosing = true;
            action = Action.OPEN;
            this.Close();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            isClosing = true;
            action = Action.EXIT;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosing == false)
                action = Action.NOTHING;
        }
    }
}
