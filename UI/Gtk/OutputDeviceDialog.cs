using System;
using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

namespace Sanford.Multimedia.Midi
{
    class OutputDeviceDialog : Window
    {
        [GUI] private ToggleButton _okToggle;
        [GUI] private ToggleButton _cancelToggle;
        [GUI] private ComboBox _outputComboBox;
        [GUI] private Label _outputLabel;

        private int outputDeviceID = 0;

        public OutputDeviceDialog() : this(new Builder("OutputDeviceDialog.glade")) { }

        private OutputDeviceDialog(Builder builder) : base(builder.GetRawOwnedObject("OutputDeviceDialog"))
        {
            builder.Autoconnect(this);

            _okToggle.Clicked += okToggle_toggled;
            _cancelToggle.Clicked += cancelToggle_toggled;

            if (OutputDevice.DeviceCount > 0)
            {
                for (int i = 0; i < OutputDevice.DeviceCount; i++)
                {
                    _outputComboBox.CellArea.GetProperty(OutputDevice.GetDeviceCapabilities(i).name);
                }

                _outputComboBox.Active = outputDeviceID;
            }
        }

        protected void OnShown(EventHandler e)
        {
            if (OutputDevice.DeviceCount > 0)
            {
                _outputComboBox.Active = outputDeviceID;
            }

            OnShown(e);
        }

        private void okToggle_toggled(object sender, EventArgs e)
        {
            if (OutputDevice.DeviceCount > 0)
            {
                outputDeviceID = _outputComboBox.Active;
            }

            Dispose();
        }

        private void cancelToggle_toggled(object sender, EventArgs e)
        {
            Dispose();
        }

        public int OutputDeviceID
        {
            get
            {
                #region Require

                if (OutputDevice.DeviceCount == 0)
                {
                    throw new InvalidOperationException();
                }

                #endregion

                return outputDeviceID;
            }
        }
    }
}
