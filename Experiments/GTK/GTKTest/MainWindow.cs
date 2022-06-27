using Gtk;
using System;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI.Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace GTKTest
{

    #region Glade test
    /*internal class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;
        //[UI] private DrawingArea _pianoControl1 = new PianoControl();

        //private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;

            //_pianoControl1.Activate();
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void Button1_Clicked(object sender, EventArgs a)
        {
            //_counter++;
            //_label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";

            PianoControl _pianoControl1 = new PianoControl();

            Window win = new Window("Cairo with Gtk# 3");
            win.SetDefaultSize(400, 400);
            win.Add(new PianoControl());
            win.ShowAll();
        }

    }*/

    #endregion

    #region Manual GTK window graphic test

    class MainWindow : Window
    {
        public MainWindow() : base("Simple drawing")
        {
            SetDefaultSize(500, 300);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate { Application.Quit(); }; ;

            PianoControl piano = new PianoControl();

            Add(piano);

            ShowAll();
        }

        public static void Main()
        {
            Application.Init();
            Window win = new Window("Cairo with Gtk# 3");
            win.SetDefaultSize(500, 300);
            win.DeleteEvent += delegate { Application.Quit(); };
            win.Add(new MainWindow());
            win.ShowAll();
            Application.Run();
        }
    }

        #endregion

    }
