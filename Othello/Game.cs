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
        ArrayList possibleMoves;

        DispatcherTimer globTimer;

        Stopwatch timerWhite;
        Stopwatch timerBlack;

        public String timerWhiteVal {
            get { return string.Format("{0:00}:{1:00}:{2:00}", timerWhite.Elapsed.Hours, timerWhite.Elapsed.Minutes, timerWhite.Elapsed.Seconds); }
        }

        public String timerBlackVal
        {
            get { return string.Format("{0:00}:{1:00}:{2:00}", timerBlack.Elapsed.Hours, timerBlack.Elapsed.Minutes, timerBlack.Elapsed.Seconds); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private String name = "Dan";

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
        private bool CheckLine(int x, int y, int direction, int color, bool stockCurrentLocation, ArrayList ary = null, int[,] game = null, ArrayList ops = null) {
            if (game == null) {
                game = this.board;
            }
            switch (direction) {
                case 1: {
                        return CheckLine(x, y, x - 1, y + 1, -1, 1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 2: {
                        return CheckLine(x, y, x, y + 1, 0, 1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 3: {
                        return CheckLine(x, y, x + 1, y + 1, 1, 1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 4: {
                        return CheckLine(x, y, x - 1, y, -1, 0, color, stockCurrentLocation, ary, game, ops);
                    }
                case 6: {
                        return CheckLine(x, y, x + 1, y, 1, 0, color, stockCurrentLocation, ary, game, ops);
                    }
                case 7: {
                        return CheckLine(x, y, x - 1, y - 1, -1, -1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 8: {
                        return CheckLine(x, y, x, y - 1, 0, -1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 9: {
                        return CheckLine(x, y, x + 1, y - 1, 1, -1, color, stockCurrentLocation, ary, game, ops);
                    }
                default: {
                        return false;
                    }
            }
        }

        private bool CheckLine(int startingX, int startingY, int x, int y, int xInc, int yInc, int color, bool stockCurrentLocations, ArrayList ary = null, int[,] game = null, ArrayList ops = null) {
            if (game == null) {
                game = this.board;
            }
            int foeColor = color == 0 ? 1 : 0;
            bool firstPass = false;
            while (!Out(x, y)) {
                if (game[x,y] == -1)
                    return false;
                if (game[x,y] == color && firstPass == false) {
                    return false;
                }
                if (game[x,y] == foeColor) {
                    if (stockCurrentLocations) {
                        ary.Add(new Tuple<int, int>(x, y));
                    }
                    if (ops != null){
                        int[,] newEntry = new int[startingX, startingY];
                        if (!ops.Contains(newEntry)) {
                            ops.Add(new int[startingX, startingY]);
                        }
                    }
                    firstPass = true;
                }
                if (firstPass == true && game[x,y] == color) {
                    return true;
                }
                x += xInc;
                y += yInc;
            }
            return false;
        }

        private ArrayList Ops(int[,] game,int player) {
            ArrayList ops = new ArrayList();
            for (int y = 0; y < this.boardSize; y++) {
                for (int x = 0; x< this.boardSize; x++) {
                    for (int i = 0; i < 10; i++) {
                        if (i != 5) {
                            CheckLine(x, y, i, player, false, null, game, ops);
                        }
                    }
                }
            }
            return ops;
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
                for (int i = 0; i < 10; i++) {
                    if (i != 5) {
                        if (CheckLine(column, line, i, c, false, null, null, null)){
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
            for (int i = 0; i < 10; i++) {
                if (i != 5) {
                    if (CheckLine(column, line, i, c, false, null, null, null)) {
                        CheckLine(column, line, i, c, true, ary, null, null);
                    }
                }
            }
            if (ary.Count == 0) {
                return false;
            } else {
                foreach (Tuple<int, int> t in ary) {
                    board[t.Item1, t.Item2] = board[t.Item1, t.Item2] == 1 ? 0 : 1;
                    board[column, line] = c;
                }
                ary = null;
                return true;
            }
        }
        public int[,] Apply(int column, int line, bool isWhite, int[,] game) {
            int[,] newBoard;
            newBoard = (int[,])game.Clone();
            ArrayList ary = new ArrayList();
            int c = isWhite ? 0 : 1;
            for (int i = 0; i < 10; i++) {
                if (i != 5) {
                    if (CheckLine(column, line, i, c, false, null, newBoard, null)) {
                        CheckLine(column, line, i, c, true, ary, newBoard, null);
                    }
                }
            }
            foreach (Tuple<int, int> t in ary) {
                newBoard[t.Item1, t.Item2] = newBoard[t.Item1, t.Item2] == 1 ? 0 : 1;
                newBoard[column, line] = c;
                }
                ary = null;
                return newBoard;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn) {
            Tuple<Double, int, int> move = alphabeta(board, 5, 1, 0,whiteTurn?0:1);
            return new Tuple<int, int>(move.Item2, move.Item3);
        }

        public Tuple<Double,int,int> alphabeta(int[,] root,int depth, int minOrMax, Double parentValue, int player)
        {
            if (depth == 0 || Final(player,root))
            {
                //retourne -1 pour la position a jouer si on est au fond
                return new Tuple<Double,int,int>(Score(root,player), -1, -1);
            }
            //je crois pour test
            Double optVal = Double.MinValue;
            int[] optOp = null;
            foreach (int[] op in Ops(root,player))
            {
                int[,] newNode = Apply(op[0],op[1],player==0,root);
                Double val = alphabeta(newNode, depth - 1, -minOrMax, optVal,player==0?1:0).Item1;
                if (val*minOrMax>parentValue*minOrMax)
                {
                    optVal = val;
                    optOp = op;
                    if (optVal*minOrMax>parentValue*minOrMax)
                    {
                        break;
                    }
                }
            }
            return new Tuple<Double, int, int>(optVal,optOp[0],optOp[1]);
        }

        //https://github.com/kartikkukreja/blog-codes/blob/master/src/Heuristic%20Function%20for%20Reversi%20(Othello).cpp
        public Double Score(int[,] board, int player)
        {
            int my_color = player;
            int opp_color = player == 0 ? 1 : 0;
            int my_tiles = 0, opp_tiles = 0, i, j, k, my_front_tiles = 0, opp_front_tiles = 0, x, y;
            double p = 0, c = 0, l = 0, m = 0, f = 0, d = 0;

            int[] X1 = { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] Y1 = { 0, 1, 1, 1, 0, -1, -1, -1 };

            int[,]V = {{20, -3, 11, 8, 8, 11, -3, 20},
                       {-3, -7, -4, 1, 1, -4, -7, -3},
                       { 11, -4, 2, 2, 2, 2, -4, 11},
                       { 8, 1, 2, -3, -3, 2, 1, 8},
                       { 8, 1, 2, -3, -3, 2, 1, 8},
                       { 11, -4, 2, 2, 2, 2, -4, 11},
                       { -3, -7, -4, 1, 1, -4, -7, -3},
                       { 20, -3, 11, 8, 8, 11, -3, 20} };

            // Piece difference, frontier disks and disk squares
	        for(i=0; i<8; i++)
		        for(j=0; j<8; j++)  {
			        if(board[i,j] == my_color)  {
				        d += V[i,j];
				        my_tiles++;
			        } else if(board[i,j] == opp_color)  {
				        d -= V[i,j];
				        opp_tiles++;
			        }
			        if(board[i,j] != -1)   {
				        for(k=0; k<8; k++)  {
					        x = i + X1[k]; y = j + Y1[k];
					        if(x >= 0 && x< 8 && y >= 0 && y< 8 && board[x,y] == -1) {
						        if(board[i,j] == my_color)  my_front_tiles++;
						        else opp_front_tiles++;
						        break;
					        }
				        }
			        }
		        }
	        if(my_tiles > opp_tiles)
		        p = (100.0 * my_tiles)/(my_tiles + opp_tiles);
	        else if(my_tiles<opp_tiles)

                p = -(100.0 * opp_tiles)/(my_tiles + opp_tiles);
	        else p = 0;

	        if(my_front_tiles > opp_front_tiles)
		        f = -(100.0 * my_front_tiles)/(my_front_tiles + opp_front_tiles);
	        else if(my_front_tiles<opp_front_tiles)

                f = (100.0 * opp_front_tiles)/(my_front_tiles + opp_front_tiles);
	        else f = 0;

            // Corner occupancy
	        my_tiles = opp_tiles = 0;
	        if(board[0,0] == my_color) my_tiles++;
	        else if(board[0,0] == opp_color) opp_tiles++;
	        if(board[0,7] == my_color) my_tiles++;
	        else if(board[0,7] == opp_color) opp_tiles++;
	        if(board[7,0] == my_color) my_tiles++;
	        else if(board[7,0] == opp_color) opp_tiles++;
	        if(board[7,7] == my_color) my_tiles++;
	        else if(board[7,7] == opp_color) opp_tiles++;
	        c = 25 * (my_tiles - opp_tiles);

            // Corner closeness
	        my_tiles = opp_tiles = 0;
	        if(board[0,0] == -1)   {
		        if(board[0,1] == my_color) my_tiles++;
		        else if(board[0,1] == opp_color) opp_tiles++;
		        if(board[1,1] == my_color) my_tiles++;
		        else if(board[1,1] == opp_color) opp_tiles++;
		        if(board[1,0] == my_color) my_tiles++;
		        else if(board[1,0] == opp_color) opp_tiles++;
	        }
	        if(board[0,7] == -1)   {
		        if(board[0,6] == my_color) my_tiles++;
		        else if(board[0,6] == opp_color) opp_tiles++;
		        if(board[1,6] == my_color) my_tiles++;
		        else if(board[1,6] == opp_color) opp_tiles++;
		        if(board[1,7] == my_color) my_tiles++;
		        else if(board[1,7] == opp_color) opp_tiles++;
	        }
	        if(board[7,0] == -1)   {
		        if(board[7,1] == my_color) my_tiles++;
		        else if(board[7,1] == opp_color) opp_tiles++;
		        if(board[6,1] == my_color) my_tiles++;
		        else if(board[6,1] == opp_color) opp_tiles++;
		        if(board[6,0] == my_color) my_tiles++;
		        else if(board[6,0] == opp_color) opp_tiles++;
	        }
	        if(board[7,7] == -1)   {
		        if(board[6,7] == my_color) my_tiles++;
		        else if(board[6,7] == opp_color) opp_tiles++;
		        if(board[6,6] == my_color) my_tiles++;
		        else if(board[6,6] == opp_color) opp_tiles++;
		        if(board[7,6] == my_color) my_tiles++;
		        else if(board[7,6] == opp_color) opp_tiles++;
	        }
	        l = -12.5 * (my_tiles - opp_tiles);

            // Mobility
	        my_tiles = Ops(board,my_color).Count;
            opp_tiles = Ops(board, opp_color).Count;
	        if(my_tiles > opp_tiles)
		        m = (100.0 * my_tiles)/(my_tiles + opp_tiles);
	        else if(my_tiles<opp_tiles)

                m = -(100.0 * opp_tiles)/(my_tiles + opp_tiles);
	        else m = 0;

            // final weighted score
	        double score = (10 * p) + (801.724 * c) + (382.026 * l) + (78.922 * m) + (74.396 * f) + (10 * d);
            return score;
        }

        public int[,] GetBoard() {
            return board;
        }

        public bool isAnOptionAvailable(int color) {
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    for (int i = 0; i < 10; i++) {
                        if (i != 5 && board[x,y] == -1) {
                            if (color == 0) {
                              // Debug.WriteLine("WHITE : checking [" + x + ";" + y + "] : direction = " + i + " : " + CheckLine(x, y, i, color, false));
                            }
                            if (color == 1){
                            //  Debug.WriteLine("BLACK : checking [" + x + ";" + y + "] : direction = " + i + " : " + CheckLine(x, y, i, color, false));
                            }
                            if (CheckLine(x, y, i, color, false, null, null, null)) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool Final(int color, int[,] game) {
            if (isGameFinished(game)) {
                return true;
            } else {
                for (int y = 0; y < boardSize; y++) {
                    for (int x = 0; x < boardSize; x++) {
                        for (int i = 0; i < 10; i++) {
                            if (i != 5 && game[x, y] == -1) {
                                if (CheckLine(x, y, i, color, false, null, game, null)) {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
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
        
        public bool isGameFinished(int[,] game) {
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if (game[x, y] == -1) {
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
        }

        public void Evaluate() {
            String cplayer = this.getCurrentPlayer() == 0 ? "White" : "Black";
            Debug.WriteLine("Current player : " + cplayer);
            Debug.WriteLine("Can white play ? " + this.isAnOptionAvailable(0));
            Debug.WriteLine("Can black play ? " + this.isAnOptionAvailable(1));
        }
    }
}
