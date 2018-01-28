using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// start a new game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void new_game(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
            mainWindow.Closing += new CancelEventHandler(show_menu);
        }

        /// <summary>
        /// load a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void load_game(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            if (mainWindow.load_Game())
            {
                mainWindow.Show();
                this.Hide();
                mainWindow.Closing += new CancelEventHandler(show_menu);
            }
        }

        /// <summary>
        /// exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_game(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// show the menu (used when we close the main window)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void show_menu(object sender, CancelEventArgs e)
        {
            this.Show();
        }
    }
}
