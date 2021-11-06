namespace MidiWatcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.channelListBox = new System.Windows.Forms.ListBox();
            this.channelMessageGroupBox = new System.Windows.Forms.GroupBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.sysExGroupBox = new System.Windows.Forms.GroupBox();
            this.sysExRichTextBox = new System.Windows.Forms.RichTextBox();
            this.systemCommonGroupBox = new System.Windows.Forms.GroupBox();
            this.sysCommonListBox = new System.Windows.Forms.ListBox();
            this.sysRealtimeGroupBox = new System.Windows.Forms.GroupBox();
            this.sysRealtimeListBox = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.channelMessageGroupBox.SuspendLayout();
            this.sysExGroupBox.SuspendLayout();
            this.systemCommonGroupBox.SuspendLayout();
            this.sysRealtimeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelListBox
            // 
            this.channelListBox.FormattingEnabled = true;
            this.channelListBox.Location = new System.Drawing.Point(6, 19);
            this.channelListBox.Name = "channelListBox";
            this.channelListBox.Size = new System.Drawing.Size(258, 147);
            this.channelListBox.TabIndex = 0;
            // 
            // channelMessageGroupBox
            // 
            this.channelMessageGroupBox.Controls.Add(this.channelListBox);
            this.channelMessageGroupBox.Location = new System.Drawing.Point(12, 12);
            this.channelMessageGroupBox.Name = "channelMessageGroupBox";
            this.channelMessageGroupBox.Size = new System.Drawing.Size(270, 176);
            this.channelMessageGroupBox.TabIndex = 1;
            this.channelMessageGroupBox.TabStop = false;
            this.channelMessageGroupBox.Text = "Channel Messages";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(574, 213);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(574, 252);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // sysExGroupBox
            // 
            this.sysExGroupBox.Controls.Add(this.sysExRichTextBox);
            this.sysExGroupBox.Location = new System.Drawing.Point(288, 12);
            this.sysExGroupBox.Name = "sysExGroupBox";
            this.sysExGroupBox.Size = new System.Drawing.Size(367, 176);
            this.sysExGroupBox.TabIndex = 4;
            this.sysExGroupBox.TabStop = false;
            this.sysExGroupBox.Text = "SysEx Messages";
            // 
            // sysExRichTextBox
            // 
            this.sysExRichTextBox.Location = new System.Drawing.Point(6, 19);
            this.sysExRichTextBox.Name = "sysExRichTextBox";
            this.sysExRichTextBox.Size = new System.Drawing.Size(355, 147);
            this.sysExRichTextBox.TabIndex = 7;
            this.sysExRichTextBox.Text = "";
            // 
            // systemCommonGroupBox
            // 
            this.systemCommonGroupBox.Controls.Add(this.sysCommonListBox);
            this.systemCommonGroupBox.Location = new System.Drawing.Point(12, 194);
            this.systemCommonGroupBox.Name = "systemCommonGroupBox";
            this.systemCommonGroupBox.Size = new System.Drawing.Size(270, 176);
            this.systemCommonGroupBox.TabIndex = 5;
            this.systemCommonGroupBox.TabStop = false;
            this.systemCommonGroupBox.Text = "System Common Messages";
            // 
            // sysCommonListBox
            // 
            this.sysCommonListBox.FormattingEnabled = true;
            this.sysCommonListBox.Location = new System.Drawing.Point(6, 19);
            this.sysCommonListBox.Name = "sysCommonListBox";
            this.sysCommonListBox.Size = new System.Drawing.Size(258, 147);
            this.sysCommonListBox.TabIndex = 0;
            // 
            // sysRealtimeGroupBox
            // 
            this.sysRealtimeGroupBox.Controls.Add(this.sysRealtimeListBox);
            this.sysRealtimeGroupBox.Location = new System.Drawing.Point(294, 194);
            this.sysRealtimeGroupBox.Name = "sysRealtimeGroupBox";
            this.sysRealtimeGroupBox.Size = new System.Drawing.Size(268, 176);
            this.sysRealtimeGroupBox.TabIndex = 6;
            this.sysRealtimeGroupBox.TabStop = false;
            this.sysRealtimeGroupBox.Text = "System Realtime Messages";
            // 
            // sysRealtimeListBox
            // 
            this.sysRealtimeListBox.FormattingEnabled = true;
            this.sysRealtimeListBox.Location = new System.Drawing.Point(6, 19);
            this.sysRealtimeListBox.Name = "sysRealtimeListBox";
            this.sysRealtimeListBox.Size = new System.Drawing.Size(258, 147);
            this.sysRealtimeListBox.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(574, 281);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(87, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Input Thread";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(574, 304);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(93, 17);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Event Context";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 382);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.sysRealtimeGroupBox);
            this.Controls.Add(this.systemCommonGroupBox);
            this.Controls.Add(this.sysExGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.channelMessageGroupBox);
            this.Name = "Form1";
            this.Text = "Midi Watcher";
            this.channelMessageGroupBox.ResumeLayout(false);
            this.sysExGroupBox.ResumeLayout(false);
            this.systemCommonGroupBox.ResumeLayout(false);
            this.sysRealtimeGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox channelListBox;
        private System.Windows.Forms.GroupBox channelMessageGroupBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox sysExGroupBox;
        private System.Windows.Forms.GroupBox systemCommonGroupBox;
        private System.Windows.Forms.ListBox sysCommonListBox;
        private System.Windows.Forms.GroupBox sysRealtimeGroupBox;
        private System.Windows.Forms.ListBox sysRealtimeListBox;
        private System.Windows.Forms.RichTextBox sysExRichTextBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}

