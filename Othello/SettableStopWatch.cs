﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Othello {
    class SettableStopWatch : Stopwatch {
        public TimeSpan StartOffset { get; private set; }
        public SettableStopWatch(TimeSpan ts) {
            this.StartOffset = ts;
        }
        public void setOffset(TimeSpan ts) {
            this.StartOffset = ts;
        }
        public TimeSpan getTime() {
            return new TimeSpan(this.Elapsed.Hours + StartOffset.Hours, this.Elapsed.Minutes + StartOffset.Minutes, this.Elapsed.Seconds + StartOffset.Seconds);
        }
    }
}
