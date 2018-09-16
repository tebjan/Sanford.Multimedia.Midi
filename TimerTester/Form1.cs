using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using VL.Reactive.Timer;

namespace TimerTester
{
    public partial class Form1 : Form
    {
        MultimediaTimer HighResTimer;
        List<PerformanceCounter> CPUCounters = new List<PerformanceCounter>();

        public Form1()
        {
            InitializeComponent();
            HighResTimer = new MultimediaTimer();
            HighResTimer.Tick += HighResTimer_Tick;

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                CPUCounters.Add(new PerformanceCounter("Processor", "% Processor Time", i.ToString()));
            }
        }

        int count;
        int max = 1000;
        TimeSpan start;
        TimeSpan stop;  
        private void HighResTimer_Tick(object sender, EventArgs e)
        {
            count++;
            //if((count % 10) == 0)
            //{
            //    //Debug.WriteLine(HighResTimer.LastIntervalDiff.Ticks/10);
            //    Debug.WriteLine(HighResTimer.LastTestDiff.Milliseconds);
            //    Debug.WriteLine(HighResTimer.LastTestDiffMax / 10);
            //}

            if (count >= max)
            {
                stop = HighResTimer.Now;
                HighResTimer.Stop();
                TimeSpan span = stop - start;
                double msec = span.Ticks / 10000.0;
                Debug.WriteLine((msec/count) + " ms");
                count = 0;
            }
        }

        TimeSpan afterStart;
        private void button1_Click(object sender, EventArgs e)
        {
            HighResTimer.Start();
            start = HighResTimer.StartedAt;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            var sb = new StringBuilder();
            foreach (var counter in CPUCounters)
            {
                sb.AppendLine(counter.NextValue().ToString());
            }
            textBox1.Text = sb.ToString();
        }
    }
}
