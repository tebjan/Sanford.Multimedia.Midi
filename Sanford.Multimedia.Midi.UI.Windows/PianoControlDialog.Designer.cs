namespace Sanford.Multimedia.Midi.UI
{
    partial class PianoControlDialog
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.lowNoteIDNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.noteRangeGroupBox = new System.Windows.Forms.GroupBox();
            this.highNoteIDLabel = new System.Windows.Forms.Label();
            this.lowNoteIDLabel = new System.Windows.Forms.Label();
            this.highNoteIDNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.lowNoteIDNumericUpDown)).BeginInit();
            this.noteRangeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highNoteIDNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(18, 117);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(142, 117);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // lowNoteIDNumericUpDown
            // 
            this.lowNoteIDNumericUpDown.Location = new System.Drawing.Point(6, 43);
            this.lowNoteIDNumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.lowNoteIDNumericUpDown.Name = "lowNoteIDNumericUpDown";
            this.lowNoteIDNumericUpDown.Size = new System.Drawing.Size(60, 20);
            this.lowNoteIDNumericUpDown.TabIndex = 4;
            this.lowNoteIDNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lowNoteIDNumericUpDown.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.lowNoteIDNumericUpDown.ValueChanged += new System.EventHandler(this.lowNoteIDNumericUpDown_ValueChanged);
            // 
            // noteRangeGroupBox
            // 
            this.noteRangeGroupBox.Controls.Add(this.highNoteIDLabel);
            this.noteRangeGroupBox.Controls.Add(this.lowNoteIDLabel);
            this.noteRangeGroupBox.Controls.Add(this.highNoteIDNumericUpDown);
            this.noteRangeGroupBox.Controls.Add(this.lowNoteIDNumericUpDown);
            this.noteRangeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.noteRangeGroupBox.Name = "noteRangeGroupBox";
            this.noteRangeGroupBox.Size = new System.Drawing.Size(211, 88);
            this.noteRangeGroupBox.TabIndex = 5;
            this.noteRangeGroupBox.TabStop = false;
            this.noteRangeGroupBox.Text = "Note Range";
            // 
            // highNoteIDLabel
            // 
            this.highNoteIDLabel.AutoSize = true;
            this.highNoteIDLabel.Location = new System.Drawing.Point(150, 27);
            this.highNoteIDLabel.Name = "highNoteIDLabel";
            this.highNoteIDLabel.Size = new System.Drawing.Size(55, 13);
            this.highNoteIDLabel.TabIndex = 7;
            this.highNoteIDLabel.Text = "High Note";
            // 
            // lowNoteIDLabel
            // 
            this.lowNoteIDLabel.AutoSize = true;
            this.lowNoteIDLabel.Location = new System.Drawing.Point(12, 27);
            this.lowNoteIDLabel.Name = "lowNoteIDLabel";
            this.lowNoteIDLabel.Size = new System.Drawing.Size(53, 13);
            this.lowNoteIDLabel.TabIndex = 6;
            this.lowNoteIDLabel.Text = "Low Note";
            // 
            // highNoteIDNumericUpDown
            // 
            this.highNoteIDNumericUpDown.Location = new System.Drawing.Point(145, 43);
            this.highNoteIDNumericUpDown.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.highNoteIDNumericUpDown.Name = "highNoteIDNumericUpDown";
            this.highNoteIDNumericUpDown.Size = new System.Drawing.Size(60, 20);
            this.highNoteIDNumericUpDown.TabIndex = 5;
            this.highNoteIDNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.highNoteIDNumericUpDown.Value = new decimal(new int[] {
            109,
            0,
            0,
            0});
            this.highNoteIDNumericUpDown.ValueChanged += new System.EventHandler(this.highNoteIDNumericUpDown_ValueChanged);
            // 
            // PianoControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 155);
            this.Controls.Add(this.noteRangeGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Name = "PianoControlDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Piano Control Settings";
            ((System.ComponentModel.ISupportInitialize)(this.lowNoteIDNumericUpDown)).EndInit();
            this.noteRangeGroupBox.ResumeLayout(false);
            this.noteRangeGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.highNoteIDNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown lowNoteIDNumericUpDown;
        private System.Windows.Forms.GroupBox noteRangeGroupBox;
        private System.Windows.Forms.Label highNoteIDLabel;
        private System.Windows.Forms.Label lowNoteIDLabel;
        private System.Windows.Forms.NumericUpDown highNoteIDNumericUpDown;
    }
}