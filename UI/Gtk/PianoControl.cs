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
using System.Collections.Specialized;
using System.ComponentModel;
//using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Gtk;
using Cairo;

using Action = Gtk.Action;
using Container = Gtk.Container;
using Keys = Gdk.Key;
using Point = Gdk.Point;
using Rectangle = Gdk.Rectangle;

namespace Sanford.Multimedia.Midi.UI.Gtk
{
    public partial class PianoControl : DrawingArea
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

        public class PianoKeyColor
        {
            public static Color Black { get; } = new Color(0, 0, 0, 1);
            public static Color SkyBlue { get; } = new Color(0.88, 1.23, 1.08, 1);
            public static Color White { get; } = new Color(1, 1, 1, 1);


            private readonly string? name;

            private readonly long value;

            private readonly short state;

            private const short StateKnownColorValid = 0x0001;

            public bool IsKnownColor => (state & StateKnownColorValid) != 0;


            public static bool operator ==(PianoKeyColor left, Color right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(PianoKeyColor left, Color right)
            {
                return !left.Equals(right);
            }

            public static explicit operator Color(PianoKeyColor keyColor)
            {
                return new Color(0.88, 1.23, 1.08, 1);
            }

            public override bool Equals([NotNullWhen(true)] object? obj) => obj is Color other && Equals(other);

            public bool Equals(Color other) => this == other;

            public override int GetHashCode()
            {
                if (name != null && !IsKnownColor)
                    return name.GetHashCode();

                return HashCode.Combine(value.GetHashCode(), state.GetHashCode());
            }
        }

        #region Experimental Color Name Code
        /*
        public readonly struct ColorName : IEquatable<ColorName>
        {
            public static readonly ColorName Empty;

            public static Color Black => new Color()
            {
                R = 0,
                G = 0,
                B = 0,
                A = 255
            };

            public static Color SkyBlue => new Color()
            {
                R = 135,
                G = 206,
                B = 235,
                A = 255
            };

            public static Color White => new Color()
            {
                R = 255,
                G = 255,
                B = 255,
                A = 255
            };

            public enum KnownColor
            {
                Black,
                SkyBlue,
                White
            };

            private const short StateKnownColorValid = 0x0001;

            private const short StateARGBValueValid = 0x0002;

            private const short StateValueMask = StateARGBValueValid;

            private const short StateNameValid = 0x0008;

            private const long NotDefinedValue = 0;


            internal const int ARGBAlphaShift = 24;
            internal const int ARGBRedShift = 16;
            internal const int ARGBGreenShift = 8;
            internal const int ARGBBlueShift = 0;
            internal const uint ARGBAlphaMask = 0xFFu << ARGBAlphaShift;
            internal const uint ARGBRedMask = 0xFFu << ARGBRedShift;
            internal const uint ARGBGreenMask = 0xFFu << ARGBGreenShift;
            internal const uint ARGBBlueMask = 0xFFu << ARGBBlueShift;


            private readonly string name;

            private readonly long value;

            private readonly short state;

            private readonly short knownColor;

            internal ColorName(KnownColor knownColor)
            {
                value = 0;
                state = StateKnownColorValid;
                name = null;
                this.knownColor = unchecked((short)knownColor);
            }

            private ColorName(long value, short state, string? name, KnownColor knownColor)
            {
                this.value = value;
                this.state = state;
                this.name = name;
                this.knownColor = unchecked((short)knownColor);
            }

            public byte R => unchecked((byte)(Value >> ARGBRedShift));

            public byte G => unchecked((byte)(Value >> ARGBGreenShift));

            public byte B => unchecked((byte)(Value >> ARGBBlueShift));

            public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

            public bool IsKnownColor => (state & StateKnownColorValid) != 0;

            public bool IsEmpty => state == 0;

            public bool IsNamedColor => ((state & StateNameValid) != 0) || IsKnownColor;

            public string Name
            {
                get
                {
                    if ((state & StateNameValid) != 0)
                    {
                        Debug.Assert(name != null);
                        return name;
                    }

                    // if we reached here, just encode the value
                    //
                    return value.ToString("x");
                }
            }

            private long Value
            {
                get
                {
                    if ((state & StateValueMask) != 0)
                    {
                        return value;
                    }

                    // This is the only place we have system colors value exposed
                    if (IsKnownColor)
                    {
                        return KnownColorTable.KnownColorToArgb((KnownColor)knownColor);
                    }
                    
                    return NotDefinedValue;
                }
            }

            public KnownColor ToKnownColor() => (KnownColor)knownColor;

            public override string ToString() =>
                IsNamedColor ? $"{nameof(Color)} [{Name}]" :
                (state & StateValueMask) != 0 ? $"{nameof(Color)} [R={R}, G={G}, B={B}, A={A}]" :
                $"{nameof(Color)} [Empty]";

            public static bool operator ==(ColorName left, ColorName right) =>
            left.value == right.value
                && left.state == right.state
                && left.knownColor == right.knownColor
                && left.name == right.name;

            public static bool operator !=(ColorName left, ColorName right) => !(left == right);

            public static implicit operator Color()
            {
                return new Color();
            }

            public static bool operator ==(Color left, ColorName right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Color left, ColorName right)
            {
                return !left.Equals(right);
            }
            
            public override bool Equals([NotNullWhen(true)] object? obj) => obj is ColorName other && Equals(other);

            public bool Equals(ColorName other) => this == other;

            public override int GetHashCode()
            {
                if (name != null && !IsKnownColor)
                    return name.GetHashCode();

                return HashCode.Combine(value.GetHashCode(), state.GetHashCode(), knownColor.GetHashCode());
            }
        }
        */
        #endregion

        private delegate void NoteMessageCallback(ChannelMessage message);

        private const int DefaultLowNoteID = 21;

        private const int DefaultHighNoteID = 109;

        private const double BlackKeyScale = 0.666666666;

        private SynchronizationContext context;

        private int lowNoteID = DefaultLowNoteID;

        private int highNoteID = DefaultHighNoteID;

        private int octaveOffset = 5;

        private Color noteOnColor = PianoKeyColor.SkyBlue;

        private readonly PianoKeyColor keyColor = null!;

        private PianoKey[] keys = null!;

        private int whiteKeyCount;

        private NoteMessageCallback noteOnCallback;

        private NoteMessageCallback noteOffCallback;

        public event EventHandler<PianoKeyEventArgs> ?PianoKeyDown;

        public event EventHandler<PianoKeyEventArgs> ?PianoKeyUp;


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
            keyTable.Add(Keys.Z, 9);
            keyTable.Add(Keys.H, 10);
            keyTable.Add(Keys.U, 11);
            keyTable.Add(Keys.J, 12);
            keyTable.Add(Keys.K, 13);
            keyTable.Add(Keys.O, 14);
            keyTable.Add(Keys.L, 15);
            keyTable.Add(Keys.P, 16);
        }

        public PianoControl()
        {
            CreatePianoKeys();
            InitializePianoKeys();

            context = SynchronizationContext.Current!;

            noteOnCallback = delegate (ChannelMessage message)
            {
                if (message.Data2 > 0)
                {
                    keys[message.Data1 - lowNoteID].PressPianoKey();
                }
                else
                {
                    keys[message.Data1 - lowNoteID].ReleasePianoKey();
                }
            };

            noteOffCallback = delegate (ChannelMessage message)
            {
                keys[message.Data1 - lowNoteID].ReleasePianoKey();
            };
        }

        private void CreatePianoKeys()
        {
            Orientation horizontal = Orientation.Horizontal;
            int spacing = 2;

            Box box = new Box(horizontal, spacing);

            // If piano keys have already been created.
            if (keys != null)
            {

                // Remove and dispose of current piano keys.
                foreach (PianoKey key in keys)
                {
                    box.Remove(key);
                    key.Dispose();
                }
            }

            keys = new PianoKey[HighNoteID - LowNoteID];

            whiteKeyCount = 0;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = new PianoKey(this);
                keys[i].NoteID = i + LowNoteID;

                if (KeyTypeTable[keys[i].NoteID] == KeyType.White)
                {
                    whiteKeyCount++;
                }
                else
                {
                    keys[i].NoteOffColor = PianoKeyColor.Black;
                    //keys[i].Window.Raise();
                }

                 keys[i].NoteOnColor = (Color)NoteOnColor;

                box.Add(keys[i]);
            }
        }

        private void InitializePianoKeys()
        {
            #region Guard

            if (keys.Length == 0)
            {
                return;
            }

            #endregion

            int Width = WidthRequest;
            int Height = HeightRequest;

            int whiteKeyWidth = Width / whiteKeyCount;
            int blackKeyWidth = (int)(whiteKeyWidth * BlackKeyScale);
            int blackKeyHeight = (int)(Height * BlackKeyScale);
            int offset = whiteKeyWidth - blackKeyWidth / 2;
            //int swhiteKeyWidth = new Gdk.Point(Width / whiteKeyCount);
            //int soffset = new Gdk.Size(swhiteKeyWidth - sblackKeyWidth / 2);
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

                    keys[n].HeightRequest = Height;
                    keys[n].WidthRequest = whiteKeyWidth;

                    if (remainder != 0 && counter <= whiteKeyCount && Convert.ToInt32(step * counter) == w)
                    {
                        counter++;
                        keys[n].WidthRequest++;
                    }
                    // See the Location property of black keys to understand
                    widthsum += LastWhiteWidth;
                    LastWhiteWidth = keys[n].WidthRequest;
                    keys[n].Location = new Point(widthsum, 0);

                    w++;
                    //n++; // Move?
                }
                else
                {
                    keys[n].HeightRequest = blackKeyHeight;
                    keys[n].WidthRequest = blackKeyWidth;
                    keys[n].Location = new Point(widthsum, offset);
                    //keys[n].Location = new Point(widthsum + offset - keys[n - 1].Width); // By this way, eliminates the LastWhiteWidth var
                    //keys[n].Window.Raise();
                    //n++; // Move?
                }
                n++; // Moved
            }
        }

        /*
        private EventHandler d;

        private bool InvokeRequired
        {
            get
            {
                Application.Invoke(d);
                return false;
            }
        }

        private void BeginInvoke(Delegate method, params object[] args)
        {

        }
        */

        public void Send(ChannelMessage message, object sender, EventArgs args, EventHandler d, AsyncCallback callback)
        {
            
            if (message.Command == ChannelCommand.NoteOn &&
                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
            {
                
                //Application.Invoke(d);

                if ((bool)d.Target!)
                {
                    noteOnCallback.BeginInvoke(message, callback, d);
                }
                else
                {
                    noteOnCallback(message);
                }

            }
            else if (message.Command == ChannelCommand.NoteOff &&
                message.Data1 >= LowNoteID && message.Data1 <= HighNoteID)
            {
                //Application.Invoke(d);

                if ((bool)d.Target!)
                {
                    noteOffCallback.BeginInvoke(message, callback, d);
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

            if (noteID < lowNoteID || noteID > highNoteID)
            {
                throw new ArgumentOutOfRangeException();
            }

            #endregion

            keys[noteID - lowNoteID].PressPianoKey();
        }

        public void ReleasePianoKey(int noteID)
        {
            #region Require

            if (noteID < lowNoteID || noteID > highNoteID)
            {
                throw new ArgumentOutOfRangeException();
            }

            #endregion

            keys[noteID - lowNoteID].ReleasePianoKey();
        }

        public void PressPianoKey(Keys k)
        {
            if (!HasFocus)
            {
                return;
            }

            if (keyTable.Contains(k))
            {
                int noteID = (int)keyTable[k]! + 12 * octaveOffset;

                if (noteID >= LowNoteID && noteID <= HighNoteID)
                {
                    if (!keys[noteID - lowNoteID].IsPianoKeyPressed)
                    {
                        keys[noteID - lowNoteID].PressPianoKey();
                    }
                }
            }
            else
            {
                if (k == Keys.Key_0)
                {
                    octaveOffset = 0;
                }
                else if (k == Keys.Key_1)
                {
                    octaveOffset = 1;
                }
                else if (k == Keys.Key_2)
                {
                    octaveOffset = 2;
                }
                else if (k == Keys.Key_3)
                {
                    octaveOffset = 3;
                }
                else if (k == Keys.Key_4)
                {
                    octaveOffset = 4;
                }
                else if (k == Keys.Key_5)
                {
                    octaveOffset = 5;
                }
                else if (k == Keys.Key_6)
                {
                    octaveOffset = 6;
                }
                else if (k == Keys.Key_7)
                {
                    octaveOffset = 7;
                }
                else if (k == Keys.Key_8)
                {
                    octaveOffset = 8;
                }
                else if (k == Keys.Key_9)
                {
                    octaveOffset = 9;
                }
            }
        }

        public void ReleasePianoKey(Keys k)
        {
            #region Guard

            if (!keyTable.Contains(k))
            {
                return;
            }

            #endregion            

            int noteID = (int)keyTable[k]! + 12 * octaveOffset;

            if (noteID >= LowNoteID && noteID <= HighNoteID)
            {
                keys[noteID - lowNoteID].ReleasePianoKey();
            }
        }

        protected override void OnAdjustSizeRequest(Orientation orientation, out int minimum_size, out int natural_size)
        {
            InitializePianoKeys();

            base.OnAdjustSizeRequest(orientation, out minimum_size, out natural_size);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (PianoKey key in keys)
                {
                    key.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected virtual void OnPianoKeyDown(PianoKeyEventArgs e)
        {
            EventHandler<PianoKeyEventArgs> handler = PianoKeyDown!;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPianoKeyUp(PianoKeyEventArgs e)
        {
            EventHandler<PianoKeyEventArgs> handler = PianoKeyUp!;

            if (handler != null)
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

                if (value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("LowNoteID", value,
                        "Low note ID out of range.");
                }

                #endregion

                #region Guard

                if (value == lowNoteID)
                {
                    return;
                }

                #endregion

                lowNoteID = value;

                if (lowNoteID > highNoteID)
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

                if (value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("HighNoteID", value,
                        "High note ID out of range.");
                }

                #endregion

                #region Guard

                if (value == highNoteID)
                {
                    return;
                }

                #endregion

                highNoteID = value;

                if (highNoteID < lowNoteID)
                {
                    lowNoteID = highNoteID;
                }

                CreatePianoKeys();
                InitializePianoKeys();
            }
        }

        public PianoKeyColor NoteOnColor
        {
            get
            {
                return keyColor;
            }
            set
            {
                #region Guard

                if (value == noteOnColor)
                {
                    return;
                }

                #endregion

                noteOnColor = (Color)value;

                foreach (PianoKey key in keys)
                {
                    key.NoteOnColor = noteOnColor;
                }
            }
        }
    }
}
