using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Othello {
    class SoundManager {
        private MediaPlayer introPlayer;
        private MediaPlayer loopPlayer;
        //private MediaPlayer loopPlayer;
        private MediaPlayer soundPlayer;
        Uri introPath = new Uri("sound/intro.wav", UriKind.Relative);
        Uri loopPath = new Uri("sound/loop.wav", UriKind.Relative);
        public SoundManager() {
            loopPlayer = new MediaPlayer();
            introPlayer = new MediaPlayer();

            introPlayer.Open(introPath);
            loopPlayer.Open(loopPath);
            introPlayer.MediaEnded += IntroPlayer_MediaEnded;
            loopPlayer.MediaEnded += LoopPlayer_MediaEnded;
            introPlayer.Play();
        }



        private void LoopPlayer_MediaEnded(object sender, EventArgs e) {
            loopPlayer.Position = TimeSpan.Zero;
            loopPlayer.Play();
        }

        private void IntroPlayer_MediaEnded(object sender, EventArgs e) {
            loopPlayer.Play();
        }

        public void Play(bool isWhite, int pawnNumber) {
            soundPlayer = new MediaPlayer();
            string color = isWhite ? "w" : "b";
            Uri path = new Uri("sound/" + color + (pawnNumber - 1) + ".wav", UriKind.Relative);
            soundPlayer.Open(path);
            soundPlayer.Play();
        }
    }
}
