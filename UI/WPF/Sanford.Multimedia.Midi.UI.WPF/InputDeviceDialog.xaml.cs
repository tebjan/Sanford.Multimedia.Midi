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
    public partial class InputDeviceDialog : Window
    {
        private int inputDeviceID = 0;

        public InputDeviceDialog()
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
        }

        protected override void OnContentRendered(EventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                inputComboBox.SelectedIndex = inputDeviceID;
            }

            base.OnContentRendered(e);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (InputDevice.DeviceCount > 0)
            {
                inputDeviceID = inputComboBox.SelectedIndex;
            }

            DialogResult = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
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
    }
}
