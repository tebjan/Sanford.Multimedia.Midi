using Gtk;
using Cairo;
using System;
using UI = Gtk.Builder.ObjectAttribute;

namespace GTKGraphicsTest
{

    #region Glade test

    /*internal class MainWindow : Window
    {
        static void Main()
        {

        }

        [UI] private DrawingArea _testDrawing1 = new TestDrawing();

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            _testDrawing1.Show();

            DeleteEvent += Window_DeleteEvent;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }


    public class TestDrawing : DrawingArea
    {

        private Color onBrush = new Color(0.88, 1.23, 1.08);

        private Color offBrush = new Color(1, 1, 1);

        void Draw(Context cr, int width, int height)
        {
            cr.SetSourceRGB(0, 0, 0);
            cr.LineWidth = 1;
            cr.Rectangle(20, 20, 120, 80);
            cr.StrokePreserve();
            cr.Stroke();

            cr.Rectangle(180, 20, 120, 80);
            cr.SetSourceColor(onBrush);
            cr.Fill();

            cr.Rectangle(330, 20, 80, 80);
            cr.SetSourceColor(offBrush);
            cr.Fill();

            ((IDisposable)cr).Dispose();
        }

        protected override bool OnDrawn(Cairo.Context ctx)
        {
            Draw(ctx, AllocatedWidth, AllocatedHeight);
            return true;
        }
    }*/

    #endregion


    #region Manual GTK window test

    class MainWindow : Window
    {
        public MainWindow() : base("Simple drawing")
        {
            SetDefaultSize(230, 150);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); }; ;

            DrawingArea darea = new DrawingArea();

            Add(darea);

            ShowAll();
        }

		public static void Main()
        {
            Application.Init();
            Window win = new Window("Cairo with Gtk# 3");
            win.SetDefaultSize(400, 400);
            win.DeleteEvent += delegate { Application.Quit(); };
            win.Add(new MainWindow());
            win.ShowAll();
            Application.Run();
        }

        private Color onBrush = new Color(0.88, 1.23, 1.08);

        private Color offBrush = new Color(1, 1, 1);

		void Draw(Context cr, int width, int height)
		{
            cr.SetSourceRGB(0, 0, 0);
            cr.LineWidth = 1;
            cr.Rectangle(20, 20, 120, 80);
            cr.StrokePreserve();
            cr.Stroke();

            cr.Rectangle(180, 20, 120, 80);
            cr.SetSourceColor(onBrush);
            cr.Fill();

            cr.Rectangle(330, 20, 80, 80);
            cr.SetSourceColor(offBrush);
            cr.Fill();

            ((IDisposable)cr).Dispose();
        }

		protected override bool OnDrawn(Cairo.Context ctx)
        {
            Draw(ctx, AllocatedWidth, AllocatedHeight);
            return true;
        }
    }

    #endregion

}
