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
    public partial class PianoControlDialog : Form
    {
        private int lowNoteID;

        private int highNoteID;

        public PianoControlDialog()
        {
            InitializeComponent();

            UpdateProperties();
        }

        private void UpdateProperties()
        {
            lowNoteID = (int)lowNoteIDNumericUpDown.Value;
            highNoteID = (int)highNoteIDNumericUpDown.Value;
        }

        private void lowNoteIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(lowNoteIDNumericUpDown.Value > highNoteIDNumericUpDown.Value)
            {
                highNoteIDNumericUpDown.Value = lowNoteIDNumericUpDown.Value;
            }
        }

        private void highNoteIDNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if(highNoteIDNumericUpDown.Value < lowNoteIDNumericUpDown.Value)
            {
                lowNoteIDNumericUpDown.Value = highNoteIDNumericUpDown.Value;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            UpdateProperties();

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        public int LowNoteID
        {
            get
            {
                return lowNoteID;
            }
            set
            {
                #region Require

                if(value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("LowNoteID", value,
                        "Low note ID out of range.");
                }

                #endregion

                lowNoteID = value;

                lowNoteIDNumericUpDown.Value = value;   

                if(lowNoteID > highNoteID)
                {
                    highNoteID = lowNoteID;
                    highNoteIDNumericUpDown.Value = highNoteID;
                }                             
            }
        }

        public int HighNoteID
        {
            get
            {
                return highNoteID;
            }
            set
            {
                #region Require

                if(value < 0 || value > ShortMessage.DataMaxValue)
                {
                    throw new ArgumentOutOfRangeException("HighNoteID", value,
                        "High note ID out of range.");
                }

                #endregion

                highNoteID = value;

                highNoteIDNumericUpDown.Value = value;

                if(highNoteID < lowNoteID)
                {
                    lowNoteID = highNoteID;
                    lowNoteIDNumericUpDown.Value = highNoteID;
                }
            }
        }
    }
}