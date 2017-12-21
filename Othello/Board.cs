using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Board
    {
        Pawn[,] board;
        int scoreWhite;
        int scoreBlack;
        Color currentPlayer;

        public Board()
        {
            init();
            Console.WriteLine("init");
        }

        private void init() {
            board = new Pawn[8, 8];
            scoreWhite = 2;
            scoreBlack = 2;
            currentPlayer = Color.BLACK;
            init();
            for (int y = 3; y < 5; y++) {
                for (int x = 3; x < 5; x++) {
                    board[x,y].placed = true;
                    board[x,y].color = x == y ? Color.WHITE : Color.BLACK;
                }
            }
        }

        public bool isLegal(int x,int y)
        {
            return true;
        }

        public void placePawn(int x, int y)
        {

        }

        public void countScore()
        {

        }
    }
}
