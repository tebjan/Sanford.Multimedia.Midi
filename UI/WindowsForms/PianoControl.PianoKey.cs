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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Sanford.Multimedia;

namespace Sanford.Multimedia.Midi.UI.Windows
{
    public partial class PianoControl
    {
        private class PianoKey : Control
        {
            private PianoControl owner;

            // 17th, 33rd, 47th, code to be called upon boot: A private boolean named 'on' is given a value equal to false.
            private bool on = false;

            // 18th, 34th, 48th code to be called upon boot: 
            private SolidBrush onBrush = new SolidBrush(Color.SkyBlue);

            // 19th, 35th, 49th code to be called upon boot: 
            private SolidBrush offBrush = new SolidBrush(Color.White);

            // 20th, 36th, 50th code to be called upon boot: 
            private int noteID = 60;

            // 21st, 37th, 51st code to be called upon boot: 
            public PianoKey(PianoControl owner)
            {
                this.owner = owner;
                this.TabStop = false;
            }

            // When one of the piano keys are pressed, this code will activate.
            public void PressPianoKey()
            {
                #region Guard

                // If the 'on' is set to true, this code will activate and return the value.
                if(on)
                {
                    return;
                }

                #endregion

                // This tells the program that the 'on' value is set to true for this void method.
                on = true;

                Invalidate();

                owner.OnPianoKeyDown(new PianoKeyEventArgs(noteID));
            }

            // This will activate when the ReleasePianoKey() is called, when the left mouse button is released.
            public void ReleasePianoKey()
            {
                #region Guard

                // If the piano key isn't 'on' and is set to false, this code will activate with a return value.
                if(!on)
                {
                    return;
                }

                #endregion

                // This tells the program that the 'on' value is set to false for this void method.
                on = false;

                Invalidate();

                owner.OnPianoKeyUp(new PianoKeyEventArgs(noteID));
            }

            // This will remove and clear out the SolidBrush colors, and remove from RAM, and repeats until all have been removed.
            protected override void Dispose(bool disposing)
            {
                if(disposing)
                {
                    onBrush.Dispose();
                    offBrush.Dispose();
                }

                base.Dispose(disposing);
            }

            // This code is used when the mouse cursor enters and hovers over one of the piano keys, which also calls upon OnMouseMove as well.
            protected override void OnMouseEnter(EventArgs e)
            {
                // This checks if the left mouse button is pressed while hovering over the key.
                if(Control.MouseButtons == MouseButtons.Left)
                {
                    // This will activate when capture is set to false, to PressPianoKey() again.
                    PressPianoKey();
                }

                // If no key is pressed, the base return code here will activate.
                base.OnMouseEnter(e);
            }

            // This code is used when the mouse cursor leaves one of the piano keys.
            protected override void OnMouseLeave(EventArgs e)
            {
                if(on)
                {
                    // This code will activate when the left mouse button is released.
                    ReleasePianoKey();
                }

                base.OnMouseLeave(e);
            }

            // This code is called when one of the piano keys are pressed with the left mouse button.
            protected override void OnMouseDown(MouseEventArgs e)
            {
                // PressPianoKey() will activate any sound assigned to the piano key (if any).
                PressPianoKey();

                if(!owner.Focused)
                {
                    owner.Focus();
                }

                base.OnMouseDown(e);
            }

            // This code is called when one of the piano keys released after being pressed by the left mouse button.
            protected override void OnMouseUp(MouseEventArgs e)
            {
                // ReleasePianoKey() will stop any sound assigned to this piano key (if any).
                ReleasePianoKey();

                base.OnMouseUp(e);
            }

            // This code is called when the mouse cursor moves over one of the piano keys while it hovers.
            protected override void OnMouseMove(MouseEventArgs e)
            {
                if(e.X < 0 || e.X > Width || e.Y < 0 || e.Y > Height)
                {
                    // If the mouse cursor moves to another piano key while selecting one of the piano keys, this code will activate by asssigning capture value as false.
                    Capture = false;
                }

                base.OnMouseMove(e);
            }

            // Here in this void method, there are three graphics drawn, they will load piece by piece when needed.
            protected override void OnPaint(PaintEventArgs e)
            {
                // If 'on' is activated for the piano key, it will activate this code.
                if(on)
                {
                    // This draws a filled in skyblue rectangle.
                    e.Graphics.FillRectangle(onBrush, 0, 0, Size.Width, Size.Height);
                }

                // Otherwise, if 'on' isn't activated, this code will activate instead.
                else
                {
                    // This draws a filled in white rectangle.
                    e.Graphics.FillRectangle(offBrush, 0, 0, Size.Width, Size.Height);
                }

                // This draws a black rectangle outline without a fill color.
                e.Graphics.DrawRectangle(Pens.Black, 0, 0, Size.Width - 1, Size.Height - 1);

                base.OnPaint(e);
            }

            protected override void OnResize(EventArgs e)
            {
                Invalidate(); // Calls OnPaint while resizing to prevent design errors
                base.OnResize(e);
            }

            public Color NoteOnColor
            {
                get
                {
                    return onBrush.Color;
                }
                set
                {
                    // 30th, 45th, 59th code to be called upon boot: 
                    onBrush.Color = value;

                    if(on)
                    {
                        Invalidate();
                    }
                }
            }

            public Color NoteOffColor
            {
                get
                {
                    return offBrush.Color;
                }
                set
                {
                    // 42nd code to be called upon boot: 
                    offBrush.Color = value;

                    if(!on)
                    {
                        Invalidate();
                    }
                }
            }

            public int NoteID
            {
                get
                {
                    // 26th, 56th code to be called upon boot: 
                    return noteID;
                }
                set
                {
                    #region Require

                    // 23rd, 39th, 53rd code to be called upon boot: 
                    if (value < 0 || value > ShortMessage.DataMaxValue)
                    {
                        throw new ArgumentOutOfRangeException("NoteID", noteID,
                            "Note ID out of range.");
                    }

                    #endregion

                    // 24th, 40th, 54th code to be called upon boot: 
                    noteID = value;
                }
            }

            public bool IsPianoKeyPressed
            {
                get
                {
                    return on;
                }
            }
        }
    }
}
