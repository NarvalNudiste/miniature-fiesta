using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello {
    public enum Color {
        WHITE = 0,
        BLACK = 1
    }
    class Pawn {
        private int x;
        private int y;
        private Color color;
        public Pawn(int x, int y, Color c) {
            this.x = x; 
            this.y = y;
            this.color = c;
        }
        public void reverse() {
            this.color = this.color == Color.BLACK ? Color.WHITE : Color.BLACK;
        }
    }
}
