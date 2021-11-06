using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Sanford.Multimedia.Midi
{
    class DeviceDialog : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;

        private int _counter;

        public DeviceDialog() : this(new Builder("MainWindow.glade")) { }

        private DeviceDialog(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _button1.Clicked += Button1_Clicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void Button1_Clicked(object sender, EventArgs a)
        {
            _counter++;
            _label1.Text = "Hello World! This button has been clicked " + _counter + " time(s).";
        }
    }
}
