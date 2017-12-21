using System;
using System.Collections.Generic;
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
            currentPlayer = 1;
            for (int y = 3; y < 5; y++) {
                for (int x = 3; x < 5; x++) {
                    board[x][y] = x == y ? 0 : 1;
                }
            }
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

    }
}
