using System.Windows.Forms;
using NUnit.Framework;

namespace UI.WindowsForms
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.Run(new Form1());
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}