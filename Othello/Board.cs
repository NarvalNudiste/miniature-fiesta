using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class Board
    {
        Pawn[,] boardState;
        int scoreWhite;
        int scoreBlack;
        Color currentPlayer;

        public Board()
        {
            boardState = new Pawn[8, 8];
            scoreWhite = 2;
            scoreBlack = 2;
            currentPlayer = Color.BLACK;
            boardState[3,3].placed = true;

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
