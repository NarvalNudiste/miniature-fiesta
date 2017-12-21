using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Game
    {
        int[][] board;
        int currentPlayer;
        int boardSize = 8;
        public Game()
        {
            Initialize();
        }

        private void Initialize() {
            //init 2d array
                board = new int[boardSize][];
                for (int i = 0; i < boardSize; i++) {
                    board[i] = new int[boardSize];
                }
            //fill the array with starting pos
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if ((y == 3 || y == 4) && (x == 3 || x == 4))
                        board[x][y] = x == y ? 0 : 1;
                    else
                        board[x][y] = -1;
                }
            }
            currentPlayer = 1;
        }
        
        private int GetScore(int player) {
            int s = 0;
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if (board[x][y] == player)
                        s++;
                }
            }
            return s;
        }

        private bool Out(int x, int y) {
            return (x < 0 || y < 0 || x > boardSize || y > boardSize) ? true : false;
        }

    }
}
