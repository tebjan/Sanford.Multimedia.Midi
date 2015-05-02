using System;
using System.Collections.Generic;
using System.Text;

namespace Sanford.Multimedia.Midi
{
    public abstract class MidiEventArgsBase : EventArgs
    {
        /// <summary>
        /// Delta samples when the event should be processed in the next audio buffer.
        /// Leave at 0 for realtime input to play as fast as possible.
        /// Set to the desired sample in the next buffer if you play a midi sequence synchronized to the audio callback
        /// </summary>
        public int DeltaFrames;
    }

    public class RawMessageEventArgs : MidiEventArgsBase
    {
        private readonly byte[] message;

        public RawMessageEventArgs(int message)
        {
            this.message = new byte[] { (byte)ShortMessage.UnpackStatus(message),
                    (byte)ShortMessage.UnpackData1(message),
                    (byte)ShortMessage.UnpackData2(message) };
        }
        
        public RawMessageEventArgs(byte status, byte data1, byte data2)
        {
            this.message = new byte[] { status, data1, data2 };
        }

        public byte[] Message
        {
            get
            {
                return message;
            }
        }
    }
}
