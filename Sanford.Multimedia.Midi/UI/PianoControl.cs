#region License

/* Copyright (c) 2006 Leslie Sanford
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;
using System.Collections;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Sanford.Multimedia.Midi.UI
{
    public partial class PianoControl : Control
    {
        private enum KeyType
        {
            White,
            Black
        }

        private readonly static Hashtable keyTable = new Hashtable();

        private static readonly KeyType[] KeyTypeTable = 
            {
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White,
                KeyType.White, KeyType.Black, KeyType.White, KeyType.Black, KeyType.White, KeyType.White, KeyType.Black, KeyType.White
            };

        private delegate void NoteMessageCallback(ChannelMessage message);

        private const int DefaultLowNoteID = 21;

        private const int DefaultHighNoteID = 109;

        private const double BlackKeyScale = 0.666666666;

        private SynchronizationContext context;

        private int lowNoteID = DefaultLowNoteID;

        private int highNoteID = DefaultHighNoteID;

        private int octaveOffset = 5;

        private Color noteOnColor = Color.SkyBlue;

        private PianoKey[] keys = null;

        private int whiteKeyCount;

        private NoteMessageCallback noteOnCallback;

        private NoteMessageCallback noteOffCallback;

        public event EventHandler<PianoKeyEventArgs> PianoKeyDown;

        public event EventHandler<PianoKeyEventArgs> PianoKeyUp;

        static PianoControl()
        {
            keyTable.Add(Keys.A, 0);
            keyTable.Add(Keys.W, 1);
            keyTable.Add(Keys.S, 2);
            keyTable.Add(Keys.E, 3);
            keyTable.Add(Keys.D, 4);
            keyTable.Add(Keys.F, 5);
            keyTable.Add(Keys.T, 6);
            keyTable.Add(Keys.G, 7);
            keyTable.Add(Keys.Y, 8);
            keyTable.Add(Keys.H, 9);
            keyTable.Add(Keys.U, 10);
            keyTable.Add(Keys.J, 11);
            keyTable.Add(Keys.K, 12);
            keyTable.Add(Keys.O, 13);
            keyTable.Add(Keys.L, 14);
            keyTable.Add(Keys.P, 15);
        }

        public PianoControl()
        {
            CreatePianoKeys();
            InitializePianoKeys();

            context = SynchronizationContext.Current;

            noteOnCallback = delegate(ChannelMessage message)
            {
                if(message.Data2 > 0)
                {
                    keys[message.Data1 - lowNoteID].PressPianoKey();
                }
                else
                {
                    keys[message.Data1 - lowNoteID].ReleasePianoKey();
                }
            };

            noteOffCallback = delegate(ChannelMessage message)
            {
                keys[message.Data1 - lowNoteID].ReleasePianoKey();
            };
        }

        private void CreatePianoKeys()
        {
            // If piano keys have already been created.
            if(keys != null)
            {
                // Remove and dispose of current piano keys.
                foreach(PianoKey key in keys)
                {
                    Controls.Remove(key);
                    key.Dispose();
                }
            }

            keys = new PianoKey[HighNoteID - LowNoteID];

            whiteKeyCount = 0;

            for(int i = 0; i < keys.Length; i++)
            {
                keys[i] = new PianoKey(this);
                keys[i].NoteID = i + LowNoteID;

                if(KeyTypeTable[keys[i].NoteID] == KeyType.White)
                {
                    whiteKeyCount++;
                }
                else
                {
                    keys[i].NoteOffColor = Color.Black;
                    keys[i].BringToFront();
                }

                keys[i].NoteOnColor = NoteOnColor;

                Controls.Add(keys[i]);
            }
        }

        private void InitializePianoKeys()
        {
            #region Guard

            if(keys.Length == 0)
            {
                return;
            }

            #endregion

            int whiteKeyWidth = Width / whiteKeyCount;
            int blackKeyWidth = (int)(whiteKeyWidth * BlackKeyScale);
            int blackKeyHeight = (int)(Height * BlackKeyScale);
            int offset = whiteKeyWidth - blackKeyWidth / 2;
            int n = 0;
            int w = 0;

            while(n < keys.Length)
            {
                if(KeyTypeTable[keys[n].NoteID] == KeyType.White)
                {
                    keys[n].Height = Height;
                    keys[n].Width = whiteKeyWidth;
                    keys[n].Location = new Point(w * whiteKeyWidth, 0);
                    w++;
                    n++;
                }
                else
                {
                    keys[n].Height = blackKeyHeight;
                    keys[n].Width = blackKeyWidth;
                    keys[n].Location = new Point(offset + (w - 1) * whiteKeyWidth);
                    keys[n].BringToFront();
                    n++;
                }
            }
        }

        public void Send(ChannelMessage message)
        {
            if(message.Command == ChannelCommand.NoteOn &&
                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
            {
                if(InvokeRequired)
                {
                    BeginInvoke(noteOnCallback, message);
                }
                else
                {
                    noteOnCallback(message);
                }
            }
            else if(message.Command == ChannelCommand.NoteOff &&
                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
            {
                if(InvokeRequired)
                {
                    BeginInvoke(noteOffCallback, message);
                }
                else
                {
                    noteOffCallback(message);
                }
            }
        }

        public void PressPianoKey(int noteID)
        {
            #region Require

            if(noteID < lowNoteID || noteID > highNoteID)
            {
                throw new ArgumentOutOfRangeException();
            }

            #endregion

            keys[noteID - lowNoteID].PressPianoKey();
        }

        public void ReleasePianoKey(int noteID)
        {
            #region Require

            if(noteID < lowNoteID || noteID > highNoteID)
            {
                throw new ArgumentOutOfRangeException();
            }

            #endregion

            keys[noteID - lowNoteID].ReleasePianoKey();
        }

        public void PressPianoKey(Keys k)
        {
            if(!Focused)
            {
                return;
            }

            if(keyTable.Contains(k))
            {
                int noteID = (int)keyTable[k] + 12 * octaveOffset;

                if(noteID >= LowNoteID && noteID <= HighNoteID)
                {
                    if(!keys[noteID - lowNoteID].IsPianoKeyPressed)
                    {
                        keys[noteID - lowNoteID].PressPianoKey();
                    }
                }
            }
            else
            {
                if(k == Keys.D0)
                {
                    octaveOffset = 0;
                }
                else if(k == Keys.D1)
                {
                    octaveOffset = 1;
                }
                else if(k == Keys.D2)
                {
                    octaveOffset = 2;
                }
                else if(k == Keys.D3)
                {
                    octaveOffset = 3;
                }
                else if(k == Keys.D4)
                {
                    octaveOffset = 4;
                }
                else if(k == Keys.D5)
                {
                    octaveOffset = 5;
                }
                else if(k == Keys.D6)
                {
                    octaveOffset = 6;
                }
                else if(k == Keys.D7)
                {
                    octaveOffset = 7;
                }
                else if(k == Keys.D8)
                {
                    octaveOffset = 8;
                }
                else if(k == Keys.D9)
                {
                    octaveOffset = 9;
                }
            }
        }

        public void ReleasePianoKey(Keys k)
        {
            #region Guard

            if(!keyTable.Contains(k))
            {
                return;
            }

            #endregion            

            int noteID = (int)keyTable[k] + 12 * octaveOffset;

            if(noteID >= LowNoteID && noteID <= HighNoteID)
            {
                keys[noteID - lowNoteID].ReleasePianoKey();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            InitializePianoKeys();

            base.OnResize(e);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach(PianoKey key in keys)
                {
                    key.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected virtual void OnPianoKeyDown(PianoKeyEventArgs e)
        {
            EventHandler<PianoKeyEventArgs> handler = PianoKeyDown;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPianoKeyUp(PianoKeyEventArgs e)
        {
            EventHandler<PianoKeyEventArgs> handler = PianoKeyUp;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        public int LowNoteID
        {
            get
            {
                return lowNoteID;
            }
            set
            {
                #region Require

                if(value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("LowNoteID", value,
                        "Low note ID out of range.");
                }

                #endregion

                #region Guard

                if(value == lowNoteID)
                {
                    return;
                }

                #endregion

                lowNoteID = value;

                if(lowNoteID > highNoteID)
                {
                    highNoteID = lowNoteID;
                }

                CreatePianoKeys();
                InitializePianoKeys();
            }
        }

        public int HighNoteID
        {
            get
            {
                return highNoteID;
            }
            set
            {
                #region Require

                if(value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("HighNoteID", value,
                        "High note ID out of range.");
                }

                #endregion

                #region Guard

                if(value == highNoteID)
                {
                    return;
                }

                #endregion

                highNoteID = value;

                if(highNoteID < lowNoteID)
                {
                    lowNoteID = highNoteID;
                }

                CreatePianoKeys();
                InitializePianoKeys();
            }
        }

        public Color NoteOnColor
        {
            get
            {
                return noteOnColor;
            }
            set
            {
                #region Guard

                if(value == noteOnColor)
                {
                    return;
                }

                #endregion

                noteOnColor = value;

                foreach(PianoKey key in keys)
                {
                    key.NoteOnColor = noteOnColor;
                }
            }
        }
    }
}
