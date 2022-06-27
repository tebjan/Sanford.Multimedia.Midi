using System;
using Gtk;
using GUI = Gtk.Builder.ObjectAttribute;

namespace Sanford.Multimedia.Midi
{
    class DeviceDialog : Window
    {
        [GUI] private ToggleButton _okToggle;
        [GUI] private ToggleButton _cancelToggle;
        [GUI] private ComboBox _inputComboBox;
        [GUI] private ComboBox _outputComboBox;
        [GUI] private Label _inputLabel;
        [GUI] private Label _outputLabel;


        private int inputDeviceID = 0;

        private int outputDeviceID = 0;

        /*
        private void InitializeComponent()
        {
            _okToggle.Clicked += okToggle_toggled;
            _cancelToggle.Clicked += cancelToggle_toggled;
        }
        */

        public DeviceDialog() : this(new Builder("DeviceDialog.glade")) { }

        private DeviceDialog(Builder builder) : base(builder.GetRawOwnedObject("DeviceDialog"))
        {
            builder.Autoconnect(this);

            _okToggle.Clicked += okToggle_toggled;
            _cancelToggle.Clicked += cancelToggle_toggled;

            //InitializeComponent();

            if (InputDevice.DeviceCount > 0)
            {
                for (int i = 0; i < InputDevice.DeviceCount; i++)
                {
                    _inputComboBox.CellArea.GetProperty(InputDevice.GetDeviceCapabilities(i).name);
                }

                _inputComboBox.Active = inputDeviceID;
            }

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
            if (InputDevice.DeviceCount > 0)
            {
                _inputComboBox.Active = inputDeviceID;
            }

            if (OutputDevice.DeviceCount > 0)
            {
                _outputComboBox.Active = outputDeviceID;
            }

            OnShown(e);
        }

        private void okToggle_toggled(object sender, EventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                inputDeviceID = _inputComboBox.Active;
            }

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
