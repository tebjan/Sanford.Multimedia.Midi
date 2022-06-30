using System;
using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

namespace Sanford.Multimedia.Midi
{
    class InputDeviceDialog : Window
    {
        [GUI] private ToggleButton _okToggle;
        [GUI] private ToggleButton _cancelToggle;
        [GUI] private ComboBox _inputComboBox;
        [GUI] private Label _inputLabel;

        private int inputDeviceID = 0;

        public InputDeviceDialog() : this(new Builder("InputDeviceDialog.glade")) { }

        private InputDeviceDialog(Builder builder) : base(builder.GetRawOwnedObject("InputDeviceDialog"))
        {
            builder.Autoconnect(this);

            _okToggle.Clicked += okToggle_toggled;
            _cancelToggle.Clicked += cancelToggle_toggled;

            if (InputDevice.DeviceCount > 0)
            {
                for (int i = 0; i < InputDevice.DeviceCount; i++)
                {
                    _inputComboBox.CellArea.GetProperty(InputDevice.GetDeviceCapabilities(i).name);
                }

                _inputComboBox.Active = inputDeviceID;
            }
        }

        protected void OnShown(EventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                _inputComboBox.Active = inputDeviceID;
            }

            OnShown(e);
        }

        private void okToggle_toggled(object sender, EventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                inputDeviceID = _inputComboBox.Active;
            }

            Dispose();
        }

        private void cancelToggle_toggled(object sender, EventArgs e)
        {
            Dispose();
        }

        public int InputDeviceID
        {
            get
            {
                #region Require

                if (InputDevice.DeviceCount == 0)
                {
                    throw new InvalidOperationException();
                }

                #endregion

                return inputDeviceID;
            }
        }
    }
}
