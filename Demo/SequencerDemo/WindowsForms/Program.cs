using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SequencerDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var mainWindow = new Form1();
                if (args.Length >= 1)
                {
                    mainWindow.Open(args[0]);
                }
                Application.Run(mainWindow);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}