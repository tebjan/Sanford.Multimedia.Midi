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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sanford.Multimedia.Midi.UI
{
    public partial class DeviceDialog : Form
    {
        private int inputDeviceID = 0;

        private int outputDeviceID = 0;

        public DeviceDialog()
        {
            InitializeComponent();

            if(InputDevice.DeviceCount > 0)
            {
                for(int i = 0; i < InputDevice.DeviceCount; i++)
                {
                    inputComboBox.Items.Add(InputDevice.GetDeviceCapabilities(i).name);
                }

                inputComboBox.SelectedIndex = inputDeviceID;
            }

            if(OutputDevice.DeviceCount > 0)
            {
                for(int i = 0; i < OutputDevice.DeviceCount; i++)
                {
                    outputComboBox.Items.Add(OutputDevice.GetDeviceCapabilities(i).name);
                }

                outputComboBox.SelectedIndex = inputDeviceID;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            if(InputDevice.DeviceCount > 0)
            {
                inputComboBox.SelectedIndex = inputDeviceID;
            }

            if(OutputDevice.DeviceCount > 0)
            {
                outputComboBox.SelectedIndex = outputDeviceID;
            }

            base.OnShown(e);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(InputDevice.DeviceCount > 0)
            {
                inputDeviceID = inputComboBox.SelectedIndex;
            }

            if(OutputDevice.DeviceCount > 0)
            {
                outputDeviceID = outputComboBox.SelectedIndex;
            }

            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        public int InputDeviceID
        {
            get
            {
                #region Require

                if(InputDevice.DeviceCount == 0)
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

                if(OutputDevice.DeviceCount == 0)
                {
                    throw new InvalidOperationException();
                }

                #endregion

                return outputDeviceID;
            }
        }
    }
}