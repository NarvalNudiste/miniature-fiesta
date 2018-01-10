using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        Image imageBlack;
        String srcBlack;
        Image imageWhite;
        String srcWhite;
        Image ImageNull;
        Game game;


        public MainWindow() {
            InitializeComponent();
            game = new Game();

            imageBlack = new Image();
            ImageNull = new Image();
            imageWhite = new Image();
            srcBlack = System.AppDomain.CurrentDomain.BaseDirectory + @"testB.png";
            srcWhite = System.AppDomain.CurrentDomain.BaseDirectory + @"testW.png";
            imageBlack.Source = new BitmapImage(new Uri(srcBlack));
            imageWhite.Source = new BitmapImage(new Uri(srcWhite));

            InitializeGrid();
        }

        private void InitializeGrid()
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
                    btn.MouseEnter += grid_Item_Enter_Over;
                    btn.MouseLeave += grid_Item_Left_Over;
                    if(game[j,i] == 0)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcWhite));
                        btn.Content = img;
                    }
                    else if(game[j, i] == 1)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcBlack));
                        btn.Content = img;
                    }
                    PlayGrid.Children.Add(bord);
                    PlayGrid.Children.Add(btn);
                }
            }
        }

        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/dc9afbe7-784d-42cd-8065-6fd1558e8bd9/grid-child-elements-accessing-using-c-rowcolumn?forum=wpf
        Button GetGridButton(Grid g, int r, int c)
        {
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c && e is Button)
                    return e as Button;
            }
            return null;
        }

        private void refreshGrid()
        {
            int nbRow = PlayGrid.RowDefinitions.Count();
            int nbColumn = PlayGrid.ColumnDefinitions.Count();

            for (int i = 0; i < nbRow; i++)
            {
                for (int j = 0; j < nbColumn; j++)
                {
                    Button btn = GetGridButton(PlayGrid, i, j);
                    if (game[j, i] == 0)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcWhite));
                        btn.Content = img;
                    }
                    else if (game[j, i] == 1)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcBlack));
                        btn.Content = img;
                    }
                }
            }
        }

        private void grid_Item_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int x = (int)btn.GetValue(Grid.RowProperty);
            int y = (int)btn.GetValue(Grid.ColumnProperty);
            if(game.IsPlayable(x,y, game.isCurrentPlayerWhite()))
            {
                if (game.PlayMove(x, y, game.isCurrentPlayerWhite()))
                {
                    refreshGrid();
                }
                game.changePlayer();
                game.PrintBoard();
            }
            //MessageBox.Show("row" + x.ToString() + "column" + y.ToString());
        }

        private void grid_Item_Enter_Over(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int x = (int)btn.GetValue(Grid.RowProperty);
            int y = (int)btn.GetValue(Grid.ColumnProperty);
            if (game.IsPlayable(x, y, game.isCurrentPlayerWhite()))
            {
                if(game.isCurrentPlayerWhite())
                {
                    btn.Content = imageWhite;
                }
                else
                {
                    btn.Content = imageBlack;
                }
            }
        }       

        private void grid_Item_Left_Over(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int x = (int)btn.GetValue(Grid.RowProperty);
            int y = (int)btn.GetValue(Grid.ColumnProperty);
            if (game[x, y] == -1)
                btn.Content = ImageNull;
        }
    }
}
