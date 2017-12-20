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
        public Color color { get; set; }
        public bool placed;

        public Pawn()
        {
            this.placed = false;
            this.color = Color.WHITE;
        }

        public Pawn(Color c) {
            this.color = c;
            this.placed = false;
        }
        public void reverse() {
            this.color = this.color == Color.BLACK ? Color.WHITE : Color.BLACK;
        }
    }
}
