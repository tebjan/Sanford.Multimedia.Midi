/*
 * Created by: Leslie Sanford
 * 
 * Contact: jabberdabber@hotmail.com
 * 
 * Last modified: 02/09/2004
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace Multimedia.UI
{
    /// <summary>
    /// Specifies the orientation of the piano keys.
    /// </summary>
    public enum PianoKeyOrientation
    {
        /// <summary>
        /// Piano key is oriented vertically.
        /// </summary>
        Vertical,

        /// <summary>
        /// Piano key is oriented horizontally with the back of the key facing
        /// left.
        /// </summary>
        HorizontalLeft,

        /// <summary>
        /// Piano key is oriented horizontally with the back of the key facing
        /// right.
        /// </summary>
        HorizontalRight
    }

    /// <summary>
    /// Specifies the shape of piano keys.
    /// </summary>
    /// <remarks>
    /// There are four basic shapes for piano keys. The <b>L</b> shape 
    /// represents keys C and F, and the backwards version represents keys E 
    /// and B. The upside down <b>T</b> shape represents keys D, G, and A. 
    /// And finally, the rectangular shape represents all flat keys.  
    /// </remarks>
    public enum PianoKeyShape
    {
        /// <summary>
        /// The shape of the piano key will resemble the letter L.
        /// </summary>
        LShape,

        /// <summary>
        /// The shape of the piano key will resemble the letter L.
        /// </summary>
        LShapeBackwards,

        /// <summary>
        /// The shape of the piano key will resemble the letter T upside down.
        /// </summary>
        TShape,

        /// <summary>
        /// The shape of the piano key will be that of a rectangle.
        /// </summary>
        RectShape
    }

	/// <summary>
	/// Represents a piano key.
	/// </summary>
	public class PianoKey : Control
	{
        #region Constants

        // Number of points for L shaped piano keys.
        private const int PointCountLShape = 7;

        // Number of points for T shaped piano keys.
        private const int PointCountTShape = 9;

        // Number of points for rectangular shaped piano keys.
        private const int PointCountRectShape = 5;

        // Percent of overall length used for flat keys. 
        private const double FlatKeyOffset = 0.6666666;

        #endregion

        #region Fields

        // Piano key orientation.
        private PianoKeyOrientation orientation;

        // Piano key shape.
        private PianoKeyShape shape;

        // Indicates whether or not the piano key is on.
        private bool keyOn = false;

        // Brush used to paint the piano key when it is on.
        private SolidBrush keyOnBrush = new SolidBrush(Color.Blue);

        // Brush used to paint the piano key when it is off.
        private SolidBrush keyOffBrush = new SolidBrush(Color.White);

        // Pen used to draw border.
        private Pen borderPen = new Pen(Color.Black, 2.0f);

        // Points representing the border.
        private Point[] points;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the state of the piano key changes.
        /// </summary>
        /// <remarks>
        /// This event is triggered when the state of the piano key changes 
        /// from on to off and vice versa.
        /// </remarks>
        public event EventHandler StateChanged;

        #endregion

        #region Construction
		
        /// <summary>
        /// Initializes a new instance of the PianoKey class.
        /// </summary>
        public PianoKey()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            //
            // Initialize properties.
            //

            this.orientation = PianoKeyOrientation.Vertical;
            this.shape = PianoKeyShape.LShape;
            Size = new Size(19, 51);
        }        

        #endregion

        #region Methods

        /// <summary>
        /// Turns the piano key on.
        /// </summary>
        public void TurnKeyOn()
        {
            // If the key is not currently on.
            if(!IsKeyOn())
            {
                // Indicate that the key is now on.
                keyOn = true;
                
                // If anyone is listening for changes in the key's state.
                if(StateChanged != null)
                {
                    // Notify them that the key's state has changed.
                    StateChanged(this, new EventArgs());
                }

                // Repaint key showing that it is now on.
                Invalidate(Region);
            }
        }

        /// <summary>
        /// Turns the piano key off.
        /// </summary>
        public void TurnKeyOff()
        {
            // If the key is currently on.
            if(IsKeyOn())
            {
                // Indicate that the key is now off.
                keyOn = false;
                
                // If anyone is listening for changes in the key's state.
                if(StateChanged != null)
                {
                    // Notify them that the key's state has changed.
                    StateChanged(this, new EventArgs());
                }

                // Repaint key showing that it is now off.
                Invalidate(Region);
            }
        }

        /// <summary>
        /// Indicates whether or not the piano key is on.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the piano key is on; otherwise, <b>false</b>.
        /// </returns>
        public bool IsKeyOn()
        {
            return keyOn;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

        /// <summary>
        /// Raises the MouseEnter event.
        /// </summary>
        /// <param name="e">
        /// An EventArgs that contains the event data. 
        /// </param>
        protected override void OnMouseEnter(EventArgs e)
        {
            // If the left mouse button is pressed as the mouse enters the
            // piano key.
            if(Control.MouseButtons == MouseButtons.Left)
            {
                // Turn piano key on.
                TurnKeyOn();
            }

            base.OnMouseEnter (e);
        }

        /// <summary>
        /// Raises the MouseLeave event.
        /// </summary>
        /// <param name="e">
        /// An EventArgs that contains the event data. 
        /// </param>
        protected override void OnMouseLeave(EventArgs e)
        {
            // If the piano key is on when the mouse leaves.
            if(IsKeyOn())
            {
                // Turn key off.
                TurnKeyOff();
            }

            base.OnMouseLeave (e);
        }

        /// <summary>
        /// Raises the MouseDown event.
        /// </summary>
        /// <param name="e">
        /// An EventArgs that contains the event data. 
        /// </param>
        /// <remarks>
        /// This event turns the piano key on.
        /// </remarks>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            TurnKeyOn();

            base.OnMouseDown (e);
        }

        /// <summary>
        /// Raises the MouseUp event.
        /// </summary>
        /// <param name="e">
        /// An EventArgs that contains the event data. 
        /// </param>
        /// <remarks>
        /// This event turns the piano key off.
        /// </remarks>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            TurnKeyOff();

            base.OnMouseUp (e);
        }

        /// <summary>
        /// Raises the MouseMove event.
        /// </summary>
        /// <param name="e">
        /// An EventArgs that contains the event data. 
        /// </param>
        /// <remarks>
        /// If the mouse moves outside of the piano key and the key is on,
        /// the key is turned off and the mouse is released.
        /// </remarks>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // If the key is on.
            if(IsKeyOn())
            {
                // If the mouse has moved outside of the key.
                if(!Region.IsVisible(new Point(e.X, e.Y)))
                {
                    // Turn key off.
                    TurnKeyOff();

                    // Release mouse.
                    Capture = false;
                }
            }

            base.OnMouseMove (e);
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="pe">
        /// A PaintEventArgs that contains the event data. 
        /// </param>
		protected override void OnPaint(PaintEventArgs pe)
		{
            // If the key is on.
            if(IsKeyOn())
            {
                // Fill key with the key on color.
                pe.Graphics.FillRegion(keyOnBrush, Region);
            }
            // Else the key is off.
            else
            {
                // Fill the key with the key off color.
                pe.Graphics.FillRegion(keyOffBrush, Region);                
            }           

            // Draw border
            pe.Graphics.DrawPolygon(borderPen, points);             

			// Calling the base class OnPaint
			base.OnPaint(pe);
        }

        /// <summary>
        /// Raises the SizeChanged event.
        /// </summary>
        /// <param name="e">
        /// An EventArgs that contains the event data.
        /// </param>
        protected override void OnSizeChanged(EventArgs e)
        {
            // Change appearance to reflect the new size.
            InitPoints();
            CreateRegion();
            
            base.OnSizeChanged (e);
        }

        /// <summary>
        /// Initializes the points that make up the piano key's region based
        /// on its shape and orientation.
        /// </summary>
        private void InitPoints()
        {
            // If the orientation is horizontal.
            if(Orientation == PianoKeyOrientation.HorizontalLeft ||
                Orientation == PianoKeyOrientation.HorizontalRight)
            {
                // Initialize points horizontally.
                InitPointsHorz();                
            }
            // Else the orientation is vertical.
            else
            {
                // Initialize points vertically.
                InitPointsVert();
            }            
        }

        /// <summary>
        /// Initialize points for horizontal orientation.
        /// </summary>
        private void InitPointsHorz()
        {
            // Determine shape and initialize points accordingly.
            switch(shape)
            {
                case PianoKeyShape.LShape:
                    InitPointsHorzLShape();
                    break;

                case PianoKeyShape.LShapeBackwards:
                    InitPointsHorzLShapeBackwards();
                    break;

                case PianoKeyShape.TShape:
                    InitPointsHorzTShape();
                    break;

                case PianoKeyShape.RectShape:
                    InitPointsRectShape();
                    break;

                default:
                    // Should never reach here.
                    Debug.Assert(false);
                    break;
            }

            // By default, the points are initialized to the horizontal left 
            // orientation. If horizontal right orientation is used, flip the 
            // key horizontally so that it has the correct orientation.
            if(Orientation == PianoKeyOrientation.HorizontalRight)
            {
                FlipHorizontally();
            }
        }

        /// <summary>
        /// Initializes points for vertical orientation.
        /// </summary>
        private void InitPointsVert()
        {
            // Determine shape and initialize points accordingly.
            switch(shape)
            {
                case PianoKeyShape.LShape:
                    InitPointsVertLShape();
                    break;

                case PianoKeyShape.LShapeBackwards:
                    InitPointsVertLShapeBackwards();
                    break;

                case PianoKeyShape.TShape:
                    InitPointsVertTShape();
                    break;

                case PianoKeyShape.RectShape:
                    InitPointsRectShape();
                    break;

                default:
                    // Should never reach here.
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// Initialize points for horizontal L shaped piano keys.
        /// </summary>
        private void InitPointsHorzLShape()
        { 
            points = new Point[PointCountLShape];

            points[0].X = 0;
            points[0].Y = (int)Math.Round(Size.Height * (1.0 - FlatKeyOffset));

            points[1].X = 0;
            points[1].Y = Size.Height;

            points[2].X = Size.Width;
            points[2].Y = Size.Height;

            points[3].X = Size.Width;
            points[3].Y = 0;

            points[4].X = (int)Math.Round(Size.Width * FlatKeyOffset);
            points[4].Y = 0;

            points[5].X = points[4].X;
            points[5].Y = points[0].Y;

            points[6] = points[0];            
        }

        /// <summary>
        /// Initialize points for horizontal backward L shaped piano keys.
        /// </summary>
        private void InitPointsHorzLShapeBackwards()
        {
            InitPointsHorzLShape();
            FlipVertically();
        }

        /// <summary>
        /// Initialize points for vertical L shaped piano keys.
        /// </summary>
        private void InitPointsVertLShape()
        {
            points = new Point[PointCountLShape];

            points[0].X = 0;
            points[0].Y = 0;

            points[1].X = (int)Math.Round(Size.Width * FlatKeyOffset);
            points[1].Y = 0;

            points[2].X = points[1].X;
            points[2].Y = (int)Math.Round(Size.Height * FlatKeyOffset);

            points[3].X = Size.Width;
            points[3].Y = points[2].Y;

            points[4].X = Size.Width;
            points[4].Y = Size.Height;

            points[5].X = 0;
            points[5].Y = Size.Height;

            points[6].X = points[0].X;
            points[6].Y = points[0].Y;            
        }

        /// <summary>
        /// Initialize points for vertical backward L shaped piano keys.
        /// </summary>
        private void InitPointsVertLShapeBackwards()
        {
            InitPointsVertLShape();
            FlipHorizontally();
        }

        /// <summary>
        /// Initialize points for horizontal T shaped piano keys.
        /// </summary>
        private void InitPointsHorzTShape()
        {
            points = new Point[PointCountTShape];

            points[0].X = 0;
            points[0].Y = (int)Math.Round(Size.Height * (1.0 - FlatKeyOffset));

            points[1].X = (int)Math.Round(Size.Width * FlatKeyOffset);
            points[1].Y = points[0].Y;

            points[2].X = points[1].X;
            points[2].Y = 0;

            points[3].X = Size.Width;
            points[3].Y = 0;

            points[4].X = Size.Width;
            points[4].Y = Size.Height;

            points[5].X = points[1].X;
            points[5].Y = Size.Height;

            points[6].X = points[1].X;
            points[6].Y = (int)Math.Round(Size.Height * FlatKeyOffset);

            points[7].X = 0;
            points[7].Y = points[6].Y;

            points[8] = points[0];            
        }

        /// <summary>
        /// Initialize points for vertical T shaped piano keys.
        /// </summary>
        private void InitPointsVertTShape()
        {
            points = new Point[PointCountTShape];

            points[0].X = (int)Math.Round(Size.Width * (1.0 - FlatKeyOffset));
            points[0].Y = 0;

            points[1].X = (int)Math.Round(Size.Width * FlatKeyOffset);
            points[1].Y = 0;

            points[2].X = points[1].X;
            points[2].Y = (int)Math.Round(Size.Height * FlatKeyOffset);

            points[3].X = Size.Width;
            points[3].Y = points[2].Y;

            points[4].X = Size.Width;
            points[4].Y = Size.Height;

            points[5].X = 0;
            points[5].Y = Size.Height;

            points[6].X = 0;
            points[6].Y = points[2].Y;

            points[7].X = points[0].X;
            points[7].Y = points[2].Y;

            points[8] = points[0];            
        }

        /// <summary>
        /// Initialize points for rectangular piano keys.
        /// </summary>
        private void InitPointsRectShape()
        {
            points = new Point[PointCountRectShape];

            points[0].X = 0;
            points[0].Y = 0;

            points[1].X = Size.Width;
            points[1].Y = 0;

            points[2].X = Size.Width;
            points[2].Y = Size.Height;

            points[3].X = 0;
            points[3].Y = Size.Height;

            points[4] = points[0];
        }

        /// <summary>
        /// Flip points horizontally.
        /// </summary>
        private void FlipHorizontally()
        {
            for(int i = 0; i < points.Length; i++)
            {
                points[i].X = Size.Width - points[i].X;
            }
        }

        /// <summary>
        /// Flip points vertically.
        /// </summary>
        private void FlipVertically()
        {
            for(int i = 0; i < points.Length; i++)
            {
                points[i].Y = Size.Height - points[i].Y;
            }
        }

        /// <summary>
        /// Create region for piano key based on initialized points.
        /// </summary>
        private void CreateRegion()
        {  
            byte[] types = new byte[points.Length];

            for(int i = 0; i < types.Length; i++)
            {
                types[i] = (byte)PathPointType.Line;
            }

            GraphicsPath path = new GraphicsPath(points, types);

            Region = new Region(path); 

            Invalidate(Region);
        }   

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets a value indicating the horizontal or vertical 
        /// orientation of the piano key.
        /// </summary>
        /// <value>
        /// One of the Orientation values.
        /// </value>
        public PianoKeyOrientation Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                // If the current orientation is vertical.
                if(orientation == PianoKeyOrientation.Vertical)
                {
                    // If the new orientation if horizontal left or right.
                    if(value == PianoKeyOrientation.HorizontalLeft ||
                        value == PianoKeyOrientation.HorizontalRight)
                    {
                        // Change orientation.
                        orientation = value;

                        // Flip width and height to refect going from a 
                        // vertical orientation to a horizontal one.
                        Size = new Size(Height, Width);
                    }
                }  
                // Else the current orientation is horizontal left or right.
                else
                {
                    // If the new orientation is vertical.
                    if(value == PianoKeyOrientation.Vertical)
                    {
                        // Change orientation.
                        orientation = value;

                        // Flip width and height to reflect going from a 
                        // horizontal orientation to a vertical one.
                        Size = new Size(Height, Width);
                    }
                    // Else the new orientation is horizontal.
                    else
                    {
                        // Change orientation and update appearance.
                        orientation = value;
                        InitPoints();
                        CreateRegion();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the shape of the piano key.
        /// </summary>
        /// <remarks>
        /// Piano keys have several basic shapes: An <b>L</b> shape, both 
        /// forwards and backwards, an upside down <b>T</b> shape, and a 
        /// rectangle shape. The <b>L</b> shape is used for notes C and F, and
        /// the backwards <b>L</b> is used for notes E and B. The upside down
        /// <b>T</b> is used for notes D, G, and A, and the rectangle shape
        /// is used for all of the flat keys. After setting the piano key's 
        /// shape, you may need to adjust its alignment to get the exact shape
        /// you are looking for.  
        /// </remarks>
        public PianoKeyShape Shape
        {
            get
            {
                return shape;
            }
            set
            {
                // Change shape and update appearance.
                shape = value;
                InitPoints();
                CreateRegion();
            }
        }
        
        /// <summary>
        /// Gets or sets the color used to draw the piano key when it is on.
        /// </summary>
        /// <value>
        /// A Color that represents the color used to paint the piano key when
        /// it is in the on state. The default is Color.Blue.
        /// </value>
        public Color KeyOnColor
        {
            get
            {
                return keyOnBrush.Color;
            }
            set
            {
                keyOnBrush.Color = value;

                if(IsKeyOn())
                {
                    Invalidate(Region);
                }
            }
        }  
        
        /// <summary>
        /// Gets or sets the color used to paint the piano key when it is off.
        /// </summary>
        /// <value>
        /// A Color that represents the color used to paint the piano key when
        /// it is in the off state. The default is Color.White.
        /// </value>
        public Color KeyOffColor
        {
            get
            {
                return keyOffBrush.Color;
            }
            set
            {
                keyOffBrush.Color = value;

                if(!IsKeyOn())
                {
                    Invalidate(Region);
                }
            }
        }

        #endregion
    }
}
