﻿#region License

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;

namespace SkiaSharp.PianoKeys.Test.WindowsForms
{
    public partial class PianoControl
    {
        private class PianoKey : Control
        {
            private PianoControl owner;

            private bool on = false;

            private SKPaint onFillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.SkyBlue
            };

            private SKPaint offFillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            };

            private SKPaint strokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black,
                StrokeWidth = 1,
                StrokeCap = SKStrokeCap.Square
            };

            private int noteID = 60;

            public PianoKey(PianoControl owner)
            {
                this.owner = owner;
                this.TabStop = false;
            }

            public void PressPianoKey()
            {
                #region Guard

                if (on)
                {
                    return;
                }

                #endregion

                on = true;

                Invalidate();

                owner.OnPianoKeyDown(new PianoKeyEventArgs(noteID));
            }

            public void ReleasePianoKey()
            {
                #region Guard

                if (!on)
                {
                    return;
                }

                #endregion

                on = false;

                Invalidate();

                owner.OnPianoKeyUp(new PianoKeyEventArgs(noteID));
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    onFillPaint.Dispose();
                    offFillPaint.Dispose();
                }

                base.Dispose(disposing);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    PressPianoKey();
                }

                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                if (on)
                {
                    ReleasePianoKey();
                }

                base.OnMouseLeave(e);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                PressPianoKey();

                if (!owner.Focused)
                {
                    owner.Focus();
                }

                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                ReleasePianoKey();

                base.OnMouseUp(e);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                if (e.X < 0 || e.X > Width || e.Y < 0 || e.Y > Height)
                {
                    Capture = false;
                }

                base.OnMouseMove(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                SKImageInfo info = new SKImageInfo();
                SKSurface surface = SKSurface.Create(info);
                SKCanvas canvas = surface.Canvas;

                if (on)
                {
                    canvas.DrawRect(0, 0, Size.Width, Size.Height, onFillPaint);
                }
                else
                {
                    canvas.DrawRect(0, 0, Size.Width, Size.Height, offFillPaint);
                }

                canvas.DrawRect(0, 0, Size.Width - 1, Size.Height - 1, strokePaint);

                base.OnPaint(e);
            }

            protected override void OnResize(EventArgs e)
            {
                Invalidate(); // Calls OnPaint while resizing to prevent design errors
                base.OnResize(e);
            }

            public SKColor NoteOnColor
            {
                get
                {
                    return onFillPaint.Color;
                }
                set
                {
                    onFillPaint.Color = value;

                    if (on)
                    {
                        Invalidate();
                    }
                }
            }

            public SKColor NoteOffColor
            {
                get
                {
                    return offFillPaint.Color;
                }
                set
                {
                    offFillPaint.Color = value;

                    if (!on)
                    {
                        Invalidate();
                    }
                }
            }

            public int NoteID
            {
                get
                {
                    return noteID;
                }
                set
                {
                    #region Require

                    if (value < 0 || value > ShortMessage.DataMaxValue)
                    {
                        throw new ArgumentOutOfRangeException("NoteID", noteID,
                            "Note ID out of range.");
                    }

                    #endregion

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
