namespace SkiaTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PictureBoxClockSK = new System.Windows.Forms.PictureBox();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxClockSK)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxClockSK
            // 
            this.PictureBoxClockSK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.PictureBoxClockSK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxClockSK.Location = new System.Drawing.Point(0, 0);
            this.PictureBoxClockSK.Name = "PictureBoxClockSK";
            this.PictureBoxClockSK.Size = new System.Drawing.Size(281, 326);
            this.PictureBoxClockSK.TabIndex = 0;
            this.PictureBoxClockSK.TabStop = false;
            this.PictureBoxClockSK.StyleChanged += new System.EventHandler(this.canvasView_PaintSurface);
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Tick += new System.EventHandler(this.canvasView_PaintSurface);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 326);
            this.Controls.Add(this.PictureBoxClockSK);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.canvasView_PaintSurface);
            this.ResizeBegin += new System.EventHandler(this.canvasView_PaintSurface);
            this.ResizeEnd += new System.EventHandler(this.canvasView_PaintSurface);
            this.Resize += new System.EventHandler(this.canvasView_PaintSurface);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxClockSK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox PictureBoxClockSK;
        private System.Windows.Forms.Timer Timer1;
    }
}