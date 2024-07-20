using System.Diagnostics;
using TatehamaATS.Exceptions;
using TatehamaATS.Signal;

namespace TatehamaATS
{
    public partial class MainWindow : Form
    {
        private ControlLED controlLED;
        private SignalWindow signalWindow;
        private Relay relay;
        private SignalSocket signalSocket;
        private Retsuban retsuban;
        static internal Transfer transfer = new Transfer();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                TrainState.init();
                controlLED = new ControlLED();
                relay = new Relay();
                signalWindow = new SignalWindow();
                signalSocket = new SignalSocket();
                retsuban = new Retsuban(RetsubanText, CarText);
            }
            catch (ATSCommonException ex)
            {
                // ここで例外をキャッチしてログなどに出力する     
                TrainState.ATSBroken = true;
                Debug.WriteLine($"故障");
                Debug.WriteLine($"{ex.Message} {ex.InnerException}");
                TrainState.ATSDisplay?.SetLED("", "");
                TrainState.ATSDisplay?.AddState(ex.ToCode());
                Debug.WriteLine($"{ex.Message}");
            }
            catch (Exception ex)
            {
                // 他の例外もキャッチしてログなどに出力する    
                TrainState.ATSBroken = true;
                Debug.WriteLine($"故障");
                Debug.WriteLine($"{ex.Message} {ex.InnerException}");
                var e = new CsharpException(3, "", ex);
                TrainState.ATSDisplay?.SetLED("", "");
                TrainState.ATSDisplay?.AddState(e.ToCode());
            }
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
                TrainState.ATSBroken = false;
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
            retsuban.addText("臨");
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
    }
}
