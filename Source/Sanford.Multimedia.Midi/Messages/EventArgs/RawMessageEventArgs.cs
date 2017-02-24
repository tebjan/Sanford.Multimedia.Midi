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

    /// <summary>
    /// Raw short message as int or byte array, useful when working with VST.
    /// </summary>
    /// <seealso cref="Sanford.Multimedia.Midi.MidiEventArgsBase" />
    public class RawMessageEventArgs : MidiEventArgsBase
    {
        readonly byte[] message;
        bool intMessageBuilt;
        int intMessage;

        public RawMessageEventArgs(int message)
        {
            this.intMessage = message;
            this.message = BuildByteMessage(intMessage);
        }

        public RawMessageEventArgs(byte status, byte data1, byte data2)
        {
            this.message = new byte[] { status, data1, data2 };
        }

        private static byte[] BuildByteMessage(int intMessage)
        {
            unchecked
            {
                return new byte[] { (byte)ShortMessage.UnpackStatus(intMessage),
                    (byte)ShortMessage.UnpackData1(intMessage),
                    (byte)ShortMessage.UnpackData2(intMessage) };
            }
        }

        private static int BuildIntMessage(byte[] message)
        {
            var intMessage = 0;
            intMessage = ShortMessage.PackStatus(intMessage, message[0]);
            intMessage = ShortMessage.PackData1(intMessage, message[1]);
            intMessage = ShortMessage.PackData2(intMessage, message[2]);
            return intMessage;
        }

        public byte Status { get { return message[0]; } }
        public byte Data1 { get { return message[1]; } }
        public byte Data2 { get { return message[2]; } }

        public byte[] Message
        {
            get
            {
                return message;
            }
        }

        public int IntMessage
        {
            get
            {
                if(!intMessageBuilt)
                {
                    intMessage = BuildIntMessage(message);
                    intMessageBuilt = true;
                }

                return intMessage;
            }
        }
    }
}
