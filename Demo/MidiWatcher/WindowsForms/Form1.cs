using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System.Diagnostics;

namespace MidiWatcher
{
    public partial class Form1 : Form
    {
        private const int SysExBufferSize = 128;

        private InputDevice inDevice = null;

        private SynchronizationContext context;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if(InputDevice.DeviceCount == 0)
            {
                MessageBox.Show("No MIDI input devices available.", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Close();
            }
            else
            {
                try
                {
                    context = SynchronizationContext.Current;

                    inDevice = new InputDevice(0);
                    inDevice.ChannelMessageReceived += HandleChannelMessageReceived;
                    inDevice.SysCommonMessageReceived += HandleSysCommonMessageReceived;
                    inDevice.SysExMessageReceived += HandleSysExMessageReceived;
                    inDevice.SysRealtimeMessageReceived += HandleSysRealtimeMessageReceived;
                    inDevice.Error += new EventHandler<ErrorEventArgs>(inDevice_Error);                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Close();
                }
            }

            base.OnLoad(e);
        }            

        protected override void OnClosed(EventArgs e)
        {
            if(inDevice != null)
            {
                inDevice.Close();
            }

            base.OnClosed(e);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            channelListBox.Items.Clear();

            try
            {               
                inDevice.StartRecording();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                inDevice.StopRecording();
                inDevice.Reset();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void inDevice_Error(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.Error.Message, "Error!",
                   MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate(object dummy)
            {
                channelListBox.Items.Add(
                    e.Message.Command.ToString() + '\t' + '\t' +
                    e.Message.MidiChannel.ToString() + '\t' +
                    e.Message.Data1.ToString() + '\t' +
                    e.Message.Data2.ToString());

                channelListBox.SelectedIndex = channelListBox.Items.Count - 1;
            }, null);
        }

        private void HandleSysExMessageReceived(object sender, SysExMessageEventArgs e)
        {
            context.Post(delegate(object dummy)
            {
                string result = "\n\n"; ;

                foreach(byte b in e.Message)
                {
                    result += string.Format("{0:X2} ", b);
                }

                sysExRichTextBox.Text += result;
            }, null);
        }

        private void HandleSysCommonMessageReceived(object sender, SysCommonMessageEventArgs e)
        {
            context.Post(delegate(object dummy)
            {
                sysCommonListBox.Items.Add(
                    e.Message.SysCommonType.ToString() + '\t' + '\t' +
                    e.Message.Data1.ToString() + '\t' +
                    e.Message.Data2.ToString());

                sysCommonListBox.SelectedIndex = sysCommonListBox.Items.Count - 1;
            }, null);
        }

        Stopwatch FWatch = Stopwatch.StartNew();
        double FLastMillis;
        double FDiff;
        double FDiffTimeStamp;
        int counter;
        int FLastTimestamp;
        private void HandleSysRealtimeMessageReceived(object sender, SysRealtimeMessageEventArgs e)
        {
            counter++;
            if (counter % 24 == 0)
            {
                var millis = FWatch.Elapsed.TotalMilliseconds;
                FDiff = 60000 / (millis - FLastMillis);
                FLastMillis = millis;

                var timestamp = e.Message.Timestamp;
                FDiffTimeStamp = 60000.0 / (timestamp - FLastTimestamp);
                FLastTimestamp = timestamp;
            }
           
            context.Post(delegate(object dummy)
            {
                sysRealtimeListBox.Items.Add(
                    e.Message.SysRealtimeType.ToString());

                sysRealtimeListBox.Items.Add("BPM from stopwatch: " + FDiff.ToString("F4"));
                sysRealtimeListBox.Items.Add("BPM from driver timestamp: " + FDiffTimeStamp.ToString("F4"));

                sysRealtimeListBox.SelectedIndex = sysRealtimeListBox.Items.Count - 1;
            }, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            inDevice.PostDriverCallbackToDelegateQueue = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            inDevice.PostEventsOnCreationContext = checkBox2.Checked;
        }
    }
}