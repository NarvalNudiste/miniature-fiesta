using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Game : IPlayable.IPlayable
    {
        int[,] board;
        public int currentPlayer;
        int boardSize = 8;
        private String name = "literralyunplayable";

        public int this[int x,int y]
        {
            get { return board[x,y]; }
        }

        public Game()
        {
            Initialize();
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
        }
        private bool Out(int x, int y) {
            return (x < 0 || y < 0 || x > boardSize || y > boardSize) ? true : false;
        }
        private int GetScore(int player) {
            int s = 0;
            foreach (int e in board) {
                if (e == player)
                    s++;
            }
            return s;
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
            throw new NotImplementedException();
        }

        public bool PlayMove(int column, int line, bool isWhite) {
            throw new NotImplementedException();
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn) {
            throw new NotImplementedException();
        }

        public int[,] GetBoard() {
            return board;
        }
    }
}
