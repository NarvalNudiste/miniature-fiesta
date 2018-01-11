using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Windows.Threading;

namespace Othello
{
    class Game : IPlayable.IPlayable , System.ComponentModel.INotifyPropertyChanged
    {
        int[,] board;
        private int currentPlayer;
        int boardSize = 8;
        private String name = "literallyunplayable";

        DispatcherTimer globTimer;

        Stopwatch timerWhite;
        Stopwatch timerBlack;

        public String timerWhiteVal {
            get { return timerWhite.Elapsed.ToString("hh:mm:ss"); }
        }

        public String timerBlackVal
        {
            get { return timerBlack.Elapsed.ToString("hh:mm:ss"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int this[int x,int y]
        {
            get { return board[x,y]; }
        }
        public Game()
        {
            globTimer = new DispatcherTimer();
            timerWhite = new Stopwatch();
            timerBlack = new Stopwatch();

            globTimer.Interval = TimeSpan.FromSeconds(1);
            globTimer.Tick += GlobTimerTick;
            globTimer.Start();
            Initialize();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GlobTimerTick(object sender, EventArgs e)
        {
            OnPropertyChanged("timerWhiteVal");
            OnPropertyChanged("timerBlackVal");
        }

        private void Initialize() {
            //init 2d array
                board = new int[boardSize,boardSize];
            //fill the array with starting pos
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if ((y == 3 || y == 4) && (x == 3 || x == 4))
                        board[x,y] = x == y ? 0 : 1;
                    else
                        board[x,y] = -1;
                }
            }
            currentPlayer = 1;
            timerBlack.Start();
        }
        private bool Out(int x, int y) {
            return (x < 0 || y < 0 || x >= boardSize || y >= boardSize) ? true : false;
        }
        private bool Empty(int x, int y) {
            return (board[x, y] == -1) ? true : false;
        }
        private int GetScore(int player) {
            int s = 0;
            foreach (int e in board) {
                if (e == player)
                    s++;
            }
            return s;
        }
        private bool CheckLine(int x, int y, int direction, int color, bool stockCurrentLocation, ArrayList ary = null) {
            Console.WriteLine("Checking direction " + direction);
            switch (direction) {
                case 1: {
                        return CheckLine(x-1, y+1, -1, 1, color, stockCurrentLocation, ary);
                    }
                case 2: {
                        return CheckLine(x, y+1, 0, 1, color, stockCurrentLocation, ary);
                    }
                case 3: {
                        return CheckLine(x+1, y+1, 1, 1, color, stockCurrentLocation, ary);
                    }
                case 4: {
                        return CheckLine(x-1, y, -1, 0, color, stockCurrentLocation, ary);
                    }
                case 6: {
                        return CheckLine(x+1, y, 1, 0, color, stockCurrentLocation, ary);
                    }
                case 7: {
                        return CheckLine(x-1, y-1, -1, -1, color, stockCurrentLocation, ary);
                    }
                case 8: {
                        return CheckLine(x, y-1, 0, -1, color, stockCurrentLocation, ary);
                    }
                case 9: {
                        return CheckLine(x+1, y-1, 1, -1, color, stockCurrentLocation, ary);
                    }
                default: {
                        return false;
                    }
            }
        }
        private bool CheckLine(int x, int y, int xInc, int yInc, int color, bool stockCurrentLocations, ArrayList ary = null) {
            //TOFIX
            int foeColor = color == 0 ? 1 : 0;
            bool firstPass = false;
            while (!Out(x, y)) {
                if (board[x,y] == -1 && firstPass == false)
                    return false;
                if (board[x,y] == foeColor) {
                    Debug.WriteLine("CC at " + x + ";" + y);
                    if (stockCurrentLocations) {
                        Debug.WriteLine("Appending values in array");
                        ary.Add(new Tuple<int, int>(x, y));
                    }
                    firstPass = true;
                }
                if (firstPass == true && board[x,y] == color) {
                    return true;
                }
                x += xInc;
                y += yInc;
            }
            return false;
        }
        public bool isCurrentPlayerWhite()
        {
            return currentPlayer == 0;
        }
        public void changePlayer()
        {
            currentPlayer = currentPlayer == 1 ? 0 : 1;
            if(currentPlayer ==1)
            {
                timerWhite.Stop();
                timerBlack.Start();
            }
            else if(currentPlayer ==0)
            {
                timerBlack.Stop();
                timerWhite.Start();
            }
        }
        public int GetBlackScore() {
            return GetScore(1);
        }
        public int GetWhiteScore() {
            return GetScore(0);
        }
        public string GetName() {
            return name;
        }
        public bool IsPlayable(int column, int line, bool isWhite) {
            if (Out(column, line) || !Empty(column, line))
                return false;
            else {
                int c = isWhite ? 0 : 1;
                for (int i = 0; i < 9; i++) {
                    if (i != 5) {
                        if (CheckLine(column, line, i, c, false)){
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public bool PlayMove(int column, int line, bool isWhite) {
            ArrayList ary = new ArrayList();
            int c = isWhite ? 0 : 1;
            for (int i = 0; i < 9; i++) {
                if (i != 5) {
                    CheckLine(column, line, i, c, true, ary);
                }
            }
            if (ary.Count == 0) {
                Debug.WriteLine("Null");
                return false;
            } else {
                foreach (Tuple<int, int> t in ary) {
                    Debug.WriteLine("Switching board color at " + t.Item1 + ";" + t.Item2);
                    board[t.Item1, t.Item2] = board[t.Item1, t.Item2] == 1 ? 0 : 1;
                    board[column, line] = c;
                }
                ary = null;
                return true;
            }
        }
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn) {
            throw new NotImplementedException();
        }
        public int[,] GetBoard() {
            return board;
        }
        public void PrintBoard() {
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if (board[x,y] == -1) {
                        Debug.Write("x ");
                    } else {
                        Debug.Write(board[x, y] + " ");
                    }
                }
                Debug.Write("\n");
            }
        }
    }
}
