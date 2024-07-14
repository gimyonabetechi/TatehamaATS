using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TatehamaATS.DisplayLED;

namespace TatehamaATS
{
    public partial class MainWindow : Form
    {

        private LEDWindow ledWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LEDWindowButton_Click(object sender, EventArgs e)
        {
            if (ledWindow == null || ledWindow.IsDisposed)
            {
                ledWindow = new LEDWindow();
                ledWindow.Show();
            }
            else
            {
                ledWindow.BringToFront();
            }
        }
    }
}
