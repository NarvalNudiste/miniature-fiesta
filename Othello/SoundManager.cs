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
        /// <summary>
        /// Ctor, loads files and play the song intro 
        /// </summary>
        public SoundManager() {
            loopPlayer = new MediaPlayer();
            introPlayer = new MediaPlayer();

            introPlayer.Open(introPath);
            loopPlayer.Open(loopPath);
            introPlayer.MediaEnded += IntroPlayer_MediaEnded;
            loopPlayer.MediaEnded += LoopPlayer_MediaEnded;
            introPlayer.Play();
        }


        /// <summary>
        /// Callback used to play the main music part in loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopPlayer_MediaEnded(object sender, EventArgs e) {
            loopPlayer.Position = TimeSpan.Zero;
            loopPlayer.Play();
        }
        /// <summary>
        /// Callback used to play the main music part after the intro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntroPlayer_MediaEnded(object sender, EventArgs e) {
            loopPlayer.Play();
        }
        /// <summary>
        /// Play a sound when a pawn is placed, sounds better the more pawns were switched this turn
        /// </summary>
        /// <param name="isWhite"></param>
        /// <param name="pawnNumber"></param>
        public void Play(bool isWhite, int pawnNumber) {
            soundPlayer = new MediaPlayer();
            string color = isWhite ? "w" : "b";
            Uri path = new Uri("sound/" + color + (pawnNumber - 1) + ".wav", UriKind.Relative);
            soundPlayer.Open(path);
            soundPlayer.Play();
        }
    }
}
