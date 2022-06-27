namespace UI.WindowsForms
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
            if (disposing && (components != null))
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
            this.pianoControl1 = new Sanford.Multimedia.Midi.UI.Windows.PianoControl();
            this.analogClocksk1 = new SkiaSharp.AnalogClock.AnalogClockSK();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pianoControl1
            // 
            this.pianoControl1.HighNoteID = 109;
            this.pianoControl1.Location = new System.Drawing.Point(12, 359);
            this.pianoControl1.LowNoteID = 21;
            this.pianoControl1.Name = "pianoControl1";
            this.pianoControl1.NoteOnColor = System.Drawing.Color.SkyBlue;
            this.pianoControl1.Size = new System.Drawing.Size(700, 80);
            this.pianoControl1.TabIndex = 0;
            this.pianoControl1.Text = "pianoControl1";
            // 
            // analogClocksk1
            // 
            this.analogClocksk1.Location = new System.Drawing.Point(12, 12);
            this.analogClocksk1.Name = "analogClocksk1";
            this.analogClocksk1.Size = new System.Drawing.Size(252, 302);
            this.analogClocksk1.TabIndex = 1;
            this.analogClocksk1.Text = "analogClocksk1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(444, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Non-Library Graphics Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 451);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.analogClocksk1);
            this.Controls.Add(this.pianoControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Sanford.Multimedia.Midi.UI.Windows.PianoControl pianoControl1;
        private SkiaSharp.AnalogClock.AnalogClockSK analogClocksk1;
        private System.Windows.Forms.Button button1;
    }
}