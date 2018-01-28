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
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;


namespace Othello {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Image imageBlack;
        String srcBlack;
        Image imageWhite;
        String srcWhite;
        Game game;
        SoundManager smanager;

        public MainWindow() {
            InitializeComponent();
            game = new Game();
            DataContext = game;
            smanager = new SoundManager();

            imageBlack = new Image();
            imageWhite = new Image();
            srcBlack = System.AppDomain.CurrentDomain.BaseDirectory + @"pawn_white_14x14.png";
            srcWhite = System.AppDomain.CurrentDomain.BaseDirectory + @"pawn_black_14x14.png";

            this.SizeChanged += changeSize;

            RenderOptions.SetBitmapScalingMode(imageBlack, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetBitmapScalingMode(imageWhite, BitmapScalingMode.NearestNeighbor);

            imageBlack.Source = new BitmapImage(new Uri(srcBlack));
            imageWhite.Source = new BitmapImage(new Uri(srcWhite));
            InitializeGrid();
        }

        /// <summary>
        /// initialize the grid which affiche the board
        /// </summary>
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
                    bord.BorderThickness = new Thickness(0.0);
                    bord.BorderBrush = Brushes.Black;
                    btn.Click += grid_Item_Click;
                    btn.MouseEnter += grid_Item_Enter_Over;
                    btn.MouseLeave += grid_Item_Left_Over;
                    btn.Style = (Style)FindResource("CustomButtonStyle");
                    if (game[j,i] == 0)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcWhite));
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
                        btn.Content = img;
                    }
                    else if(game[j, i] == 1)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcBlack));
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
                        btn.Content = img;
                    }
                    PlayGrid.Children.Add(bord);
                    PlayGrid.Children.Add(btn);
                }
            }

        }

        /// <summary>
        /// load a game
        /// </summary>
        /// <returns>true if the game has been load else false</returns>
        public bool load_Game()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Save file (.save) | *.save";
            dlg.DefaultExt = "save";
            dlg.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                game.LoadBoard(filename);
                refreshGrid();
                return true;
            }
            return false;
        }

        /// <summary>
        /// the function call when you click on the load button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void load_Btn_click(object sender, RoutedEventArgs e) {
            load_Game();
        }

        /// <summary>
        /// the function called when you click on the button save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Btn_click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Save file (.save) | *.save";
            saveFileDialog.FileName = "Save1.save";
            saveFileDialog.DefaultExt = "save";
            saveFileDialog.ValidateNames = true;
            saveFileDialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == true) {
                // Open document 
                string filename = saveFileDialog.FileName;
                game.SaveGame(filename);
            }

        }

        /// <summary>
        /// get the button caontained in a given position in the grid
        /// </summary>
        /// <param name="g">grid</param>
        /// <param name="r">row</param>
        /// <param name="c">column</param>
        /// <returns>the button</returns>
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

        /// <summary>
        /// refresh the display of the grid
        /// </summary>
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
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
                        btn.Content = img;
                    }
                    else if (game[j, i] == 1)
                    {
                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(srcBlack));
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.NearestNeighbor);
                        btn.Content = img;
                    }
                    else if(game[j,i] == -1)
                    {
                        btn.Content = null;
                    }
                }
            }
        }

        /// <summary>
        /// the function called when we click on a button in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Item_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int y = (int)btn.GetValue(Grid.RowProperty);
            int x = (int)btn.GetValue(Grid.ColumnProperty);
            if(game.IsPlayable(x,y, game.isCurrentPlayerWhite()))
            {
                if (game.PlayMove(x, y, game.isCurrentPlayerWhite()))
                {
                    smanager.Play(game.isCurrentPlayerWhite(), game.lastNumberOfPawnDowned);
                    refreshGrid();
                }
                //game.Evaluate();
                if (game.isGameFinished()) {
                    Debug.WriteLine("Game Finished");
                    if(game.whiteScore>game.blackScore)
                    {
                        MessageBox.Show("Player White win");
                    }
                    else if(game.whiteScore < game.blackScore)
                    {
                        MessageBox.Show("Player Black win");
                    }
                    else
                    {
                        MessageBox.Show("Draw");
                    }
                    
                    game.ResetGame();
                    refreshGrid();
                } else {
                    if (!game.isAnOptionAvailable(0) && !game.isAnOptionAvailable(1)) {
                        Debug.WriteLine("Deadlock, resetting the game");
                        game.ResetGame();
                        refreshGrid();
                    } else if (!game.isAnOptionAvailable(game.getCurrentPlayer() == 0 ? 1 : 0)) {
                        String playerSkipped = game.getCurrentPlayer() == 0 ? "black" : "white";
                        Debug.WriteLine("No option available, skipping " + playerSkipped + " turn");
                    } else {
                        game.changePlayer();
                        refreshGrid();
                    }
                }
            }
        }

        /// <summary>
        /// the function called when we mouseOver a item in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Item_Enter_Over(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int y = (int)btn.GetValue(Grid.RowProperty);
            int x = (int)btn.GetValue(Grid.ColumnProperty);
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

        /// <summary>
        /// the function called when we quit the mouseOver of an item in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_Item_Left_Over(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int y = (int)btn.GetValue(Grid.RowProperty);
            int x = (int)btn.GetValue(Grid.ColumnProperty);
            if (game[x, y] == -1)
            {
                btn.Content = null;
            }
        }


        /// <summary>
        /// the function called to resize the grid to a square we we resize the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeSize(object sender, RoutedEventArgs e)
        {
            double height = Structure.ActualHeight - (PlayGrid.Margin.Top + PlayGrid.Margin.Bottom);
            double width = Structure.ColumnDefinitions[1].ActualWidth - (PlayGrid.Margin.Left + PlayGrid.Margin.Top);
            double squareSize = Math.Min(height, width);
            PlayGrid.Height = squareSize;
            PlayGrid.Width = squareSize;
        }

    }
}
