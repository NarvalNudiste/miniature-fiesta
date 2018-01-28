using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello {
    [Serializable()]

    /// <summary>
    /// the class used to save the game
    /// </summary>
    class State {
        private int[,] board;
        private int currentPlayer;
        private System.TimeSpan whiteTimeSpanOffset;
        private System.TimeSpan blackTimeSpanOffset;
        public State(int[,] board, int currentPlayer, System.TimeSpan wTSO, System.TimeSpan bTSO) {
            this.board = board;
            this.currentPlayer = currentPlayer;
            this.whiteTimeSpanOffset = wTSO;
            this.blackTimeSpanOffset = bTSO;
        }
        public int[,] Board { get => board;}
        public int CurrentPlayer { get => currentPlayer;}
        public TimeSpan WhiteTimeSpanOffset { get => whiteTimeSpanOffset;}
        public TimeSpan BlackTimeSpanOffset { get => blackTimeSpanOffset;}
    }
}
