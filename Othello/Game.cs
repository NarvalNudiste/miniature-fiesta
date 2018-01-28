using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Othello
{
    class Game : System.ComponentModel.INotifyPropertyChanged 
    {
        int[,] board;
        private int currentPlayer;
        int boardSize = 8;
        ArrayList possibleMoves;
        public int lastNumberOfPawnDowned = 0;

        DispatcherTimer globTimer;
        SettableStopWatch timerWhite;
        SettableStopWatch timerBlack;
        

        public String timerWhiteVal {
            get { return string.Format("{0:00}:{1:00}:{2:00}", timerWhite.getTime().Hours, timerWhite.getTime().Minutes, timerWhite.getTime().Seconds); }
        }

        public String timerBlackVal
        {
            get { return string.Format("{0:00}:{1:00}:{2:00}", timerBlack.getTime().Hours, timerBlack.getTime().Minutes, timerBlack.getTime().Seconds); }
        }

        public int blackScore { get { return GetBlackScore(); } }

        public int whiteScore { get { return GetWhiteScore(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public int this[int x,int y]
        {
            get { return board[x,y]; }
        }
        public Game()
        {
            globTimer = new DispatcherTimer();
            timerWhite = new SettableStopWatch(new TimeSpan(0, 0, 0));
            timerBlack = new SettableStopWatch(new TimeSpan(0, 0, 0));

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
            possibleMoves = new ArrayList();
            //fill the array with starting pos
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if ((y == 3 || y == 4) && (x == 3 || x == 4))
                        board[x,y] = x == y ? 0 : 1;
                    else
                        board[x,y] = -1;
                }
            }
            timerBlack.Reset();
            timerWhite.Reset();
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
            switch (direction) {
                case 1: {
                        return CheckLine(x, y, x - 1, y + 1, -1, 1, color, stockCurrentLocation, ary);
                    }
                case 2: {
                        return CheckLine(x, y, x, y + 1, 0, 1, color, stockCurrentLocation, ary);
                    }
                case 3: {
                        return CheckLine(x, y, x + 1, y + 1, 1, 1, color, stockCurrentLocation, ary);
                    }
                case 4: {
                        return CheckLine(x, y, x - 1, y, -1, 0, color, stockCurrentLocation, ary);
                    }
                case 6: {
                        return CheckLine(x, y, x + 1, y, 1, 0, color, stockCurrentLocation, ary);
                    }
                case 7: {
                        return CheckLine(x, y, x - 1, y - 1, -1, -1, color, stockCurrentLocation, ary);
                    }
                case 8: {
                        return CheckLine(x, y, x, y - 1, 0, -1, color, stockCurrentLocation, ary);
                    }
                case 9: {
                        return CheckLine(x, y, x + 1, y - 1, 1, -1, color, stockCurrentLocation, ary);
                    }
                default: {
                        return false;
                    }
            }
        }

        private bool CheckLine(int startingX, int startingY, int x, int y, int xInc, int yInc, int color, bool stockCurrentLocations, ArrayList ary = null) {

            int foeColor = color == 0 ? 1 : 0;
            bool firstPass = false;
            while (!Out(x, y)) {
                if (board[x,y] == -1)
                    return false;
                if (board[x,y] == color && firstPass == false) {
                    return false;
                }
                if (board[x,y] == foeColor) {
                    if (stockCurrentLocations) {
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

        public bool IsPlayable(int column, int line, bool isWhite) {
            if (Out(column, line) || !Empty(column, line))
                return false;
            else {
                int c = isWhite ? 0 : 1;
                for (int i = 0; i < 10; i++) {
                    if (i != 5) {
                        if (CheckLine(column, line, i, c, false, null)){
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        public bool PlayMove(int column, int line, bool isWhite) {
            lastNumberOfPawnDowned = 0;
            ArrayList ary = new ArrayList();
            int c = isWhite ? 0 : 1;
            for (int i = 0; i < 10; i++) {
                if (i != 5) {
                    if (CheckLine(column, line, i, c, false, null)) {
                        CheckLine(column, line, i, c, true, ary);
                    }
                }
            }
            if (ary.Count == 0) {
                return false;
            } else {
                foreach (Tuple<int, int> t in ary) {
                    if (lastNumberOfPawnDowned < 6) {
                        lastNumberOfPawnDowned++;
                    }
                    board[t.Item1, t.Item2] = board[t.Item1, t.Item2] == 1 ? 0 : 1;
                    board[column, line] = c;
                }
                ary = null;
                OnPropertyChanged("blackScore");
                OnPropertyChanged("whiteScore");
                return true;
            }
        }
        

        public bool isAnOptionAvailable(int color) {
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    for (int i = 0; i < 10; i++) {
                        if (i != 5 && board[x,y] == -1) {
                            if (CheckLine(x, y, i, color, false, null)) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        public bool isGameFinished() {
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if (board[x,y] == -1) {
                        return false;
                    }
                }
            }
            return true;
        }
        

        public int getCurrentPlayer() {
            return currentPlayer;
        }

        public void ResetGame() {
            Initialize();
            OnPropertyChanged("blackScore");
            OnPropertyChanged("whiteScore");
        }

        public void Evaluate() {
            String cplayer = this.getCurrentPlayer() == 0 ? "White" : "Black";
            Debug.WriteLine("Current player : " + cplayer);
            Debug.WriteLine("Can white play ? " + this.isAnOptionAvailable(0));
            Debug.WriteLine("Can black play ? " + this.isAnOptionAvailable(1));
        }

        public void SaveGame(String filename) {
            State s = new State(this.board, this.currentPlayer, timerWhite.Elapsed, timerBlack.Elapsed);
            Stream TestFileStream = File.Create(filename);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(TestFileStream, s);
            TestFileStream.Close();
        }

        public void LoadBoard(String filename) {
            if (File.Exists(filename)) {
                Stream TestFileStream = File.OpenRead(filename);
                BinaryFormatter deserializer = new BinaryFormatter();
                State s = (Othello.State)deserializer.Deserialize(TestFileStream);
                TestFileStream.Close();

                if (s != null) {
                    this.board = s.Board;
                    this.currentPlayer = s.CurrentPlayer;
                    this.timerBlack = new SettableStopWatch(s.BlackTimeSpanOffset);
                    this.timerWhite = new SettableStopWatch(s.WhiteTimeSpanOffset);
                    if (currentPlayer == 1)
                    {
                        timerBlack.Start();
                    }
                    else if (currentPlayer == 0)
                    {
                        timerWhite.Start();
                    }
                    OnPropertyChanged("blackScore");
                    OnPropertyChanged("whiteScore");
                }
            }
        }

    }
}
