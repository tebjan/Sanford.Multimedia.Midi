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
//using System.Drawing;
using System.Threading;
using Gtk;
using Gdk;
using GLib;

using Color = Cairo.Color;


namespace Sanford.Multimedia.Midi.UI.Gtk
{
    public partial class PianoControl
    {
        private class PianoKey : DrawingArea
        {
            private PianoControl owner;

            private bool on = false;

            //private Color onBrush = new Color(135, 206, 235);
            private Color onBrush = PianoKeyColor.SkyBlue;

            //private Color offBrush = new Color(1, 1, 1);
            private Color offBrush = PianoKeyColor.White;

            private int noteID = 60;


            private int px;
            private int py;
            private Orientation orientation;
            private int minimum_size;
            private int natural_size;
            private int allocated_pos;
            private int allocated_size;

            public Point Location
            {
                get => new Point(px, py);
                set => OnAdjustSizeAllocation(orientation, out minimum_size, out natural_size, out allocated_pos, out allocated_size);
            }

            public PianoKey(PianoControl owner)
            {
                this.owner = owner;
                this.IsFocus = false;
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

                QueueDraw();

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

                QueueDraw();

                owner.OnPianoKeyUp(new PianoKeyEventArgs(noteID));
            }



            /* 
            protected override bool OnWidgetEvent(Event evnt)
            {
                var LeftMouseButton = new EventType();


                if (LeftMouseButton == EventType.ButtonPress)
                {
                    PressPianoKey();
                }

                base.OnWidgetEvent(evnt);

                return true;
            }
             */

            protected void OnMouseEnter(ButtonPressEventArgs args, EventCrossing evnt)
            {
                if (args.Event.Button == 1 && args.Event.Type == EventType.ButtonPress)
                {
                    PressPianoKey();
                }

                base.OnEnterNotifyEvent(evnt);
            }

            /*
            protected override bool OnWidgetEvent(Event evnt)
            {
                var LeftMouseButton = ButtonPressHandler;

                if (LeftMouseButton)
                {
                    PressPianoKey();
                }

                base.OnWidgetEvent(evnt);

                return true;
            }
            */

            protected void OnMouseLeave(EventCrossing evnt)
            {
                if (on)
                {
                    ReleasePianoKey();
                }

                base.OnLeaveNotifyEvent(evnt);
            }

            protected void OnMousePress(EventButton evnt)
            {
                PressPianoKey();


                if (!owner.HasFocus)
                {
                    owner.Activate();
                }

                base.OnButtonPressEvent(evnt);
            }

            protected void OnMouseRelease(EventButton evnt)
            {
                ReleasePianoKey();

                base.OnButtonReleaseEvent(evnt);
            }

            protected void OnMouseMove(EventMotion e)
            {
                if (e.X < 0 || e.X > WidthRequest || e.Y < 0 || e.Y > HeightRequest)
                {
                    CanFocus = false;
                }

                base.OnMotionNotifyEvent(e);
            }

            protected void OnPaint(Cairo.Context cr, int width, int height, bool disposing)
            {

                if (on)
                {
                    cr.Rectangle(0, 0, width, height);
                    cr.SetSourceColor(onBrush);
                    cr.Fill();
                }
                else
                {
                    cr.Rectangle(0, 0, width, height);
                    cr.SetSourceColor(offBrush);
                    cr.Fill();
                }

                cr.SetSourceRGB(0, 0, 0);
                cr.LineWidth = 1;
                cr.Rectangle(0, 0, width - 1, height - 1);
                cr.StrokePreserve();
                cr.Stroke();

                if (disposing)
                {
                    ((IDisposable)cr).Dispose();
                }

                base.OnDrawn(cr);
            }

            protected void OnResize(EventWindowState evnt)
            {
                QueueDraw(); // Calls OnPaint while resizing to prevent design errors
                base.OnWindowStateEvent(evnt);
            }

            public Color NoteOnColor
            {
                get
                {
                    return onBrush;
                }
                set
                {
                    onBrush = value;

                    if (on)
                    {
                        QueueDraw();
                    }
                }
            }

            public Color NoteOffColor
            {
                get
                {
                    return offBrush;
                }
                set
                {
                    offBrush = value;

                    if (!on)
                    {
                        QueueDraw();
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
