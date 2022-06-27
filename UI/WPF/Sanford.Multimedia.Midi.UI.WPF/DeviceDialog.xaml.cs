using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sanford.Multimedia.Midi.UI.WPF
{
    /// <summary>
    /// Interaction logic for DeviceDialog.xaml
    /// </summary>
    public partial class DeviceDialog : Window
    {
        private int inputDeviceID = 0;

        private int outputDeviceID = 0;

        public DeviceDialog()
        {
            InitializeComponent();

            if (InputDevice.DeviceCount > 0)
            {
                for (int i = 0; i < InputDevice.DeviceCount; i++)
                {
                    inputComboBox.Items.Add(InputDevice.GetDeviceCapabilities(i).name);
                }

                inputComboBox.SelectedIndex = inputDeviceID;
            }

            if (OutputDevice.DeviceCount > 0)
            {
                for (int i = 0; i < OutputDevice.DeviceCount; i++)
                {
                    outputComboBox.Items.Add(OutputDevice.GetDeviceCapabilities(i).name);
                }

                outputComboBox.SelectedIndex = inputDeviceID;
            }
        }

        protected override void OnContentRendered(EventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                inputComboBox.SelectedIndex = inputDeviceID;
            }

            if (OutputDevice.DeviceCount > 0)
            {
                outputComboBox.SelectedIndex = outputDeviceID;
            }

            base.OnContentRendered(e);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                inputDeviceID = inputComboBox.SelectedIndex;
            }

            if (OutputDevice.DeviceCount > 0)
            {
                outputDeviceID = outputComboBox.SelectedIndex;
            }

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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
