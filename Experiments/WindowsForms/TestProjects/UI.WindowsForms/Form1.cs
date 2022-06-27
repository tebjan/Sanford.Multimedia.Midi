using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }
    }

    public partial class Form2 : Form
    {
        // 18th, 34th, 48th code to be called upon boot: 
        private SolidBrush onBrush = new SolidBrush(Color.SkyBlue);

        // 19th, 35th, 49th code to be called upon boot: 
        private SolidBrush offBrush = new SolidBrush(Color.White);

        public Form2()
        {
            this.Size = new Size(450, 400);
            this.Paint += new PaintEventHandler(OnPaint);
        }

        protected void OnPaint(object sender, PaintEventArgs e)
        {
            // This fills the piano key with the specific color as it's being pressed.
            e.Graphics.FillRectangle(onBrush, 250, 50, 20, 150);

            // This fills the piano key with the specific color when it's not being pressed.
            e.Graphics.FillRectangle(offBrush, 50, 50, 20, 150);
            
            // This draws the rectangle to create a piano key.
            e.Graphics.DrawRectangle(Pens.Black, 150, 50, 20 - 1, 150 - 1);

            base.OnPaint(e);
        }
    }
}
