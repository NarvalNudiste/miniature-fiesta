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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            InitializeGrid();
        }

        public void InitializeGrid()
        {
            int nbRow = PlayGrid.RowDefinitions.Count();
            int nbColumn = PlayGrid.ColumnDefinitions.Count();

            for(int i = 0; i<nbRow; i++)
            {
                for(int j = 0; j<nbColumn; j++)
                {
                    Border bord = new Border();
                    Button btn = new Button();
                    Grid.SetColumn(btn, j);
                    Grid.SetRow(btn, i);
                    Grid.SetColumn(bord, j);
                    Grid.SetRow(bord, i);
                    bord.BorderThickness = new Thickness(2.0);
                    bord.BorderBrush = Brushes.Black;
                    btn.Click += grid_Item_Click;
                    PlayGrid.Children.Add(bord);
                    PlayGrid.Children.Add(btn);
                }
            }
        }

        private void grid_Item_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int x = (int)btn.GetValue(Grid.RowProperty);
            int y = (int)btn.GetValue(Grid.ColumnProperty);
            MessageBox.Show("row" + x.ToString() + "column" + y.ToString());
        }
    }
}
