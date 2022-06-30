using Gtk;
using System;
using NUnit.Framework;

namespace GTKGraphicsTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Application.Init();

            var app = new Application("org.GTKGraphicsTest.MainWindow", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}