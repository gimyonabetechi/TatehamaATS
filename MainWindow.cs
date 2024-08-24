using System.Data.Common;
using System.Diagnostics;
using TatehamaATS.Exceptions;
using TatehamaATS.Signal;

namespace TatehamaATS
{
    public partial class MainWindow : Form
    {
        static internal InspectionRecord inspectionRecord = new InspectionRecord();
        private SignalWindow signalWindow;
        private Relay relay;
        static internal SignalSocket signalSocket = new SignalSocket();
        static internal Retsuban retsuban;
        static internal Transfer transfer = new Transfer();
        static internal ControlLED controlLED;


        public MainWindow()
        {
            try
            {
                InitializeComponent();
                TrainState.init();
                relay = new Relay(Clock);
                signalWindow = new SignalWindow();
                retsuban = new Retsuban(RetsubanText, CarText);
                controlLED = new ControlLED();
            }
            catch (Exception ex)
            {
                inspectionRecord.AddException(ex);
            }
        }

        private void Time_Click(object sender, MouseEventArgs e)
        {
            relay.Time_Click(sender, e);
        }

        private void LEDWindowButton_Click(object sender, EventArgs e)
        {
            if (controlLED.isShow)
            {
                controlLED.LEDHide();
            }
            else
            {
                controlLED.LEDShow();
            }
            LEDStatus.Text = controlLED.isShow ? "表　示" : "非表示";
        }

        private void ATSResetButton_Click(object sender, EventArgs e)
        {
            //故障復帰
            if (TrainState.ATSBroken && TrainState.TrainSpeed < 1.0 && TrainState.TrainBnotch >= 8)
            {
                MainWindow.inspectionRecord.ATSReset = true;
            }
        }

        private void SignalWindowButton_Click(object sender, EventArgs e)
        {
            if (signalWindow.isShow)
            {
                signalWindow.SignalHide();
            }
            else
            {
                signalWindow.SignalShow();
            }

            SigWinStatus.Text = signalWindow.isShow ? "表　示" : "非表示";
        }
        private void Retsuban0_Click(object sender, EventArgs e)
        {
            retsuban.addText("0");
        }

        private void Retsuban1_Click(object sender, EventArgs e)
        {
            retsuban.addText("1");
        }

        private void Retsuban2_Click(object sender, EventArgs e)
        {
            retsuban.addText("2");
        }

        private void Retsuban3_Click(object sender, EventArgs e)
        {
            retsuban.addText("3");
        }

        private void Retsuban4_Click(object sender, EventArgs e)
        {
            retsuban.addText("4");
        }

        private void Retsuban5_Click(object sender, EventArgs e)
        {
            retsuban.addText("5");
        }

        private void Retsuban6_Click(object sender, EventArgs e)
        {
            retsuban.addText("6");
        }

        private void Retsuban7_Click(object sender, EventArgs e)
        {
            retsuban.addText("7");
        }

        private void Retsuban8_Click(object sender, EventArgs e)
        {
            retsuban.addText("8");
        }

        private void Retsuban9_Click(object sender, EventArgs e)
        {
            retsuban.addText("9");
        }

        private void RetsubanKai_Click(object sender, EventArgs e)
        {
            retsuban.addText("回");
        }

        private void RetsubanRin_Click(object sender, EventArgs e)
        {
            retsuban.addText("X");
        }

        private void RetsubanC_Click(object sender, EventArgs e)
        {
            retsuban.addText("C");
        }

        private void RetsubanB_Click(object sender, EventArgs e)
        {
            retsuban.addText("B");
        }

        private void RetsubanK_Click(object sender, EventArgs e)
        {
            retsuban.addText("K");
        }

        private void RetsubanA_Click(object sender, EventArgs e)
        {
            retsuban.addText("A");
        }

        private void RetsubanBack_Click(object sender, EventArgs e)
        {
            retsuban.Back();
        }

        private void RetsubanAllDel_Click(object sender, EventArgs e)
        {
            retsuban.AllDel();
        }

        private void RetsubanDel_Click(object sender, EventArgs e)
        {
            retsuban.Del();
        }

        private void RetsubanEnter_Click(object sender, EventArgs e)
        {
            retsuban.Enter();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
