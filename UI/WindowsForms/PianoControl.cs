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
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using Sanford.Multimedia.Timers;
using Sanford.Threading;
using Sanford.Collections.Generic;
using Sanford.Collections.Immutable;

namespace Sanford.Multimedia.Midi.UI.Windows
{
    public partial class PianoControl : Control
    {
        private enum KeyType
        {
            White,
            Black
        }

        // 1st code to be called upon boot: This initializes the new static Hashtable with the local name 'keyTable'.
        private readonly static Hashtable keyTable = new Hashtable();

        // 2nd code to be called upon boot: This initializes the function KeyTypeTable as a static and read only type of code with 'KeyType[]' as its local name.
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

        // This delegates the void type NoteMessageCallback, it will use ChannelMessage and is given the local name 'message'.
        private delegate void NoteMessageCallback(ChannelMessage message);

        // DefaultLowNoteID is configured with a constant integer value of 21.
        private const int DefaultLowNoteID = 21;

        // DefaultHighNoteID is configured with a constant integer value of 109.
        private const int DefaultHighNoteID = 109;

        // BlackKeyScale is configured with a constant floating point double value of 0.666666666.
        private const double BlackKeyScale = 0.666666666;

        // SynchronizationContext is given the a local name of 'context'. When this is called, it will perform a context synchronization.
        private SynchronizationContext context;

        // 4th code to be called upon boot: This loads the integer DefaultLowNoteID with 'lowNoteID' as the local name.
        private int lowNoteID = DefaultLowNoteID;

        // 5th code to be called upon boot: This loads the integer DefaultHighNoteID with 'highNoteID' as the local name.
        private int highNoteID = DefaultHighNoteID;

        // 6th code to be called upon boot: This loads the integer octaveOffset configured with a value of 5.
        private int octaveOffset = 5;

        // 7th code to be called upon boot: Color is given the local name 'noteOnColor' and configured with a SkyBlue color.
        private Color noteOnColor = Color.SkyBlue;

        // 8th code to be called upon boot: PianoKey[] is given 'keys' as the local name, with it being configured as null.
        private PianoKey[] keys = null;

        // Loads the whiteKeyCount integer, so the keys are counted.
        private int whiteKeyCount;

        // NoteMessageCallback is given 'noteOnCallback' as the local name, so it can be used for 'on' key notes when the piano keys are pressed down.
        private NoteMessageCallback noteOnCallback;

        // NoteMessageCallback is given 'noteOffCallback' as the local name, so it can be used for 'off' key notes when the piano keys are released.
        private NoteMessageCallback noteOffCallback;

        // The EventHandler here uses PianoKeyEventArgs, and is given the local name 'PianoKeyDown', so it can be called later for when the piano key is pressed down.
        public event EventHandler<PianoKeyEventArgs> PianoKeyDown;

        // The EventHandler here uses PianoKeyEventArgs, and is given the local name 'PianoKeyUp', so it can be called later for when the piano key is released.
        public event EventHandler<PianoKeyEventArgs> PianoKeyUp;

        // 3rd code to be called upon boot: These keys are loaded statically, one by one upon loading, this is for the PC keyboard keys.
        static PianoControl()
        {
            /*********************************************************\
             * Todo:
             * 
             * 1. Reorganise the keyboard keys
             * 2. Rearrange them in a way that is more user friendly.
             *
            \*********************************************************/

            keyTable.Add(Keys.ControlKey, -39);
            keyTable.Add(Keys.Escape, -38);
            keyTable.Add(Keys.CapsLock, -37);
            keyTable.Add(Keys.Menu, -36);
            keyTable.Add(Keys.ShiftKey, -35);
            keyTable.Add(Keys.Space, -34);
            keyTable.Add(Keys.Apps, -33);
            keyTable.Add(Keys.Delete, -32);
            keyTable.Add(Keys.Home, -31);
            keyTable.Add(Keys.End, -30);
            keyTable.Add(Keys.PageUp, -29);
            keyTable.Add(Keys.PageDown, -28);
            keyTable.Add(Keys.NumPad7, -27);
            keyTable.Add(Keys.NumPad4, -26);
            keyTable.Add(Keys.NumPad1, -25);
            keyTable.Add(Keys.Divide, -24);
            keyTable.Add(Keys.NumPad8, -23);
            keyTable.Add(Keys.NumPad5, -22);
            keyTable.Add(Keys.NumPad2, -21);
            keyTable.Add(Keys.NumPad0, -20);
            keyTable.Add(Keys.Multiply, -19);
            keyTable.Add(Keys.NumPad9, -18);
            keyTable.Add(Keys.NumPad6, -17);
            keyTable.Add(Keys.NumPad3, -16);
            keyTable.Add(Keys.Decimal, -15);
            keyTable.Add(Keys.Subtract, -14);
            keyTable.Add(Keys.Add, -13);
            keyTable.Add(Keys.F1, -12);
            keyTable.Add(Keys.F2, -11);
            keyTable.Add(Keys.F3, -10);
            keyTable.Add(Keys.F4, -9);
            keyTable.Add(Keys.F5, -8);
            keyTable.Add(Keys.F6, -7);
            keyTable.Add(Keys.F7, -6);
            keyTable.Add(Keys.F8, -5);
            keyTable.Add(Keys.F9, -4);
            keyTable.Add(Keys.F10, -3);
            keyTable.Add(Keys.F11, -2);
            keyTable.Add(Keys.F12, -1);
            keyTable.Add(Keys.Oemtilde, 0);
            keyTable.Add(Keys.D1, 1);
            keyTable.Add(Keys.Q, 2);
            keyTable.Add(Keys.A, 3);
            keyTable.Add(Keys.Z, 4);
            keyTable.Add(Keys.D2, 5);
            keyTable.Add(Keys.W, 6);
            keyTable.Add(Keys.S, 7);
            keyTable.Add(Keys.X, 8);
            keyTable.Add(Keys.D3, 9);
            keyTable.Add(Keys.E, 10);
            keyTable.Add(Keys.D, 11);
            keyTable.Add(Keys.C, 12);
            keyTable.Add(Keys.D4, 13);
            keyTable.Add(Keys.R, 14);
            keyTable.Add(Keys.F, 15);
            keyTable.Add(Keys.V, 16);
            keyTable.Add(Keys.D5, 17);
            keyTable.Add(Keys.T, 18);
            keyTable.Add(Keys.G, 19);
            keyTable.Add(Keys.B, 20);
            keyTable.Add(Keys.D6, 21);
            keyTable.Add(Keys.Y, 22);
            keyTable.Add(Keys.H, 23);
            keyTable.Add(Keys.N, 24);
            keyTable.Add(Keys.D7, 25);
            keyTable.Add(Keys.U, 26);
            keyTable.Add(Keys.J, 27);
            keyTable.Add(Keys.M, 28);
            keyTable.Add(Keys.D8, 29);
            keyTable.Add(Keys.I, 30);
            keyTable.Add(Keys.K, 31);
            keyTable.Add(Keys.Oemcomma, 32);
            keyTable.Add(Keys.D9, 33);
            keyTable.Add(Keys.O, 34);
            keyTable.Add(Keys.L, 35);
            keyTable.Add(Keys.OemPeriod, 36);
            keyTable.Add(Keys.D0, 37);
            keyTable.Add(Keys.P, 38);
            keyTable.Add(Keys.OemSemicolon, 39);
            keyTable.Add(Keys.OemQuestion, 40);
            keyTable.Add(Keys.OemMinus, 41);
            keyTable.Add(Keys.OemOpenBrackets, 42);
            keyTable.Add(Keys.OemQuotes, 43);
            keyTable.Add(Keys.Oemplus, 44);
            keyTable.Add(Keys.OemCloseBrackets, 45);
            keyTable.Add(Keys.Back, 46);
            keyTable.Add(Keys.OemPipe, 47);
            keyTable.Add(Keys.Enter, 48);
            keyTable.Add(Keys.RControlKey, 49);

        }

        // This group of code is named PianoControl.
        public PianoControl()
        {
            // 9th code to be called upon boot: This creates the piano keys.
            CreatePianoKeys();

            // 2nd to load in this group: This initializes the piano keys.
            InitializePianoKeys();

            // 3rd to load in this group: The locally named 'context' is configured with SynchronizationContext.Current.
            context = SynchronizationContext.Current; 

            // 4th to load in this group: Locally named 'noteOnCallback' is now configured with a delegate with ChannelMessage locally named as 'message'.
            noteOnCallback = delegate(ChannelMessage message)
            {
                if(message.Data2 > 0) // This will determine if the locally named 'message' configured with Data2 is more than 0, the code inside here will activate.
                {
                    keys[message.Data1 - lowNoteID].PressPianoKey(); // When the code is activated, the locally named 'keys' will be called, locally named 'message' is assigned with Data1 to negate lowNoteID in an encapsulated method, to work with using PressPianoKey configuration.
                }
                else // If it's not more than 0, it will use the code in here.
                {
                    keys[message.Data1 - lowNoteID].ReleasePianoKey(); // When the code is activated, the locally named 'keys' will be called, locally named 'message' is assigned with Data1 to negate lowNoteID in an encapsulated method, to work with using ReleasePianoKey configuration.
                }
            };

            // 5th to load in this group: The locally named 'noteOffCallback' is configured with a delegate with ChannelMessage locally named as 'message
            noteOffCallback = delegate(ChannelMessage message)
            {
                keys[message.Data1 - lowNoteID].ReleasePianoKey(); // When the code is activated, the locally named 'keys' will be called, locally named 'message' is assigned with Data1 to negate lowNoteID in an encapsulated method, to work with using ReleasePianoKey configuration.
            };
        }

        private void CreatePianoKeys()
        {
            // 10th code to be called upon boot: If piano keys have already been created, it will ignore creating a piano key.
            if (keys != null)
            {
                // Remove and dispose of current piano keys.
                foreach(PianoKey key in keys)
                {
                    Controls.Remove(key);
                    key.Dispose();
                }
            }

            // 11th code to be called upon boot: The locally named 'keys' is configured with a new PianoKey that has a value of what HighNoteID has, minus the amount equal to what LowNoteID has.
            keys = new PianoKey[HighNoteID - LowNoteID];

            // 14th code to be called upon boot: The whiteKeyCount is configured with a value of 0.
            whiteKeyCount = 0;

            // 15th code to be called upon boot: For an integer with a local name of 'i' to have a value equal to 0, then 'i' having a value less than keys.Length, then it will be asked the value of 'i' and then incremented.
            for (int i = 0; i < keys.Length; i++)
            {
                // 16th, 32nd, 46th, 60th code to be called upon boot: Locally named 'keys' with whatever the value of 'i' is, will be equal to whatever the new 'PianoKey' value is, within this 'for' statement.
                keys[i] = new PianoKey(this);
                
                // 22nd, 38th, 52nd code to be called upon boot: 
                keys[i].NoteID = i + LowNoteID;

                // 25th, 41st, 55th code to be called upon boot: 
                if (KeyTypeTable[keys[i].NoteID] == KeyType.White)
                {
                    // 27th, 57th code to be called upon boot: 
                    whiteKeyCount++;
                }
                else
                {
                    // 41st code to be called upon boot: Informs that the black piano key (rectangle) to be filled with black.
                    keys[i].NoteOffColor = Color.Black;

                    // 43rd code to be called upon boot: Informs the black piano key to be drawn in front of the white piano key graphics.
                    keys[i].BringToFront();
                }

                // 28th, 44th, 58th code to be called upon boot: 
                keys[i].NoteOnColor = NoteOnColor;

                // 31st code to be called upon boot: 
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

            int widthsum = 0; // Sum of white keys' width
            int LastWhiteWidth = 0; // Last white key width
            int remainder = Width % whiteKeyCount; // The remaining pixels
            int counter = 1;
            double step = remainder != 0 ? whiteKeyCount / (double)remainder : 0; // The ternary operator prevents a division by zero

            while (n < keys.Length)
            {
                if (KeyTypeTable[keys[n].NoteID] == KeyType.White)
                {

                    keys[n].Height = Height;
                    keys[n].Width = whiteKeyWidth;

                    if (remainder != 0 && counter <= whiteKeyCount && Convert.ToInt32(step * counter) == w)
                    {
                        counter++;
                        keys[n].Width++;
                    }
                    // See the Location property of black keys to understand
                    widthsum += LastWhiteWidth;
                    LastWhiteWidth = keys[n].Width;
                    keys[n].Location = new Point(widthsum, 0);

                    w++;
                    //n++; // Move?
                }
                else
                {
                    keys[n].Height = blackKeyHeight;
                    keys[n].Width = blackKeyWidth;
                    keys[n].Location = new Point(widthsum + offset);
                    //keys[n].Location = new Point(widthsum + offset - keys[n - 1].Width); // By this way, eliminates the LastWhiteWidth var
                    keys[n].BringToFront();
                    //n++; // Move?
                }
                n++; // Moved
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

        // When a keyboard key is pressed, this void code will activate, naming Keys as 'k' instead.
        public void PressPianoKey(Keys k)
        {
            // This part is activated as soon as a keyboard key is pressed, and will be ignored if it's true.
            if(!Focused)
            {
                // If the keyboard key is not focused, it will just return.
                return;
            }

            // If the keyTable contains one of the usable keyboard keys, this code will activate.
            if(keyTable.Contains(k))
            {
                // Uses the integer noteID, equally configured with an integer keyTable key plus 12 times the number in the octaveOffset.
                int noteID = (int)keyTable[k] + 12 * octaveOffset;

                // If the noteID is more than equal to the LowNoteID or the noteID is less than equal to the HighNoteID, this code will activate.
                if(noteID >= LowNoteID && noteID <= HighNoteID)
                {
                    // If the keys with a value of the noteID minus the value of the lowNoteID with the piano key pressed, the code within this will activate.
                    if(!keys[noteID - lowNoteID].IsPianoKeyPressed)
                    {
                        // The keys combined with a value of the noteID minus the value of the lowNoteID will use PressPianoKey(), and will play any sound attached to the piano key (if any).
                        keys[noteID - lowNoteID].PressPianoKey();
                    }
                }
            }

            // These will determine what octaveOffset value is assigned to each key.
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

        // As the piano keys are loading, this code will use the resize EventArgs.
        protected override void OnResize(EventArgs e)
        {
            // This initializes the piano keys upon load
            InitializePianoKeys();

            // The return code for the OnResize EventArgs.
            base.OnResize(e);
        }

        // Once the window is closed (or similar), this dispose code will take care of clearing it out of the RAM.
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                // For each piano key that's loaded in RAM, it will be cleared out of RAM.
                foreach(PianoKey key in keys)
                {
                    key.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        // When the piano key is pressed, this event code will handle it.
        protected virtual void OnPianoKeyDown(PianoKeyEventArgs e)
        {
            // Locally names the EventHandler assigned with PianoKeyEventArgs as 'handler, and configures it with it equal to PianoKeyDown.
            EventHandler<PianoKeyEventArgs> handler = PianoKeyDown;

            if(handler != null)
            {
                handler(this, e);
            }
        }

        // This virtual void will inform the program of the event handler code for when the piano key is released.
        protected virtual void OnPianoKeyUp(PianoKeyEventArgs e)
        {
            // Locally names the EventHandler assigned with PianoKeyEventArgs as 'handler, and configures it with it equal to PianoKeyUp.
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
                // 13th code to be called upon boot: The integer named LowNoteID is retrieved with a local name of 'lowNoteID'.
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
                // 12th code to be called upon boot: The integer named HighNoteID is retrieved with a local name of 'highNoteID'.
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
                // 29th code to be called upon boot: 
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
