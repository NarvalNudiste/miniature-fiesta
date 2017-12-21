using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Board
    {
        int[][] board;
        int currentPlayer;
        int boardSize = 8;
        public Board()
        {
            Initialize();
        }

        private void Initialize() {
            try {
                board = new int[boardSize][];
                for (int i = 0; i < boardSize; i++) {
                    board[i] = new int[boardSize];
                }

                for (int y = 0; y < boardSize; y++) {
                    for (int x = 0; x < boardSize; x++) {
                        board[x][y] = new int();
                    }
                }

            } catch (System.StackOverflowException soe) {
                Console.WriteLine(soe.StackTrace);
            }

            currentPlayer = 1;
            for (int y = 3; y < 5; y++) {
                for (int x = 3; x < 5; x++) {
                    board[x][y] = x == y ? 0 : 1;
                }
            }
        }
        
        int GetScore(int player) {
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
