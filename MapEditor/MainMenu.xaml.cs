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
            EXIT
        }

        public Action action { get; set; }

        public MainMenu()
        {
            InitializeComponent();
        }

        private void createMapButton_Click(object sender, RoutedEventArgs e)
        {
            action = Action.CREATE;
            this.Close();
        }

        private void openMapButton_Click(object sender, RoutedEventArgs e)
        {
            action = Action.OPEN;
            this.Close();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            action = Action.EXIT;
            this.Close();
        }
    }
}
