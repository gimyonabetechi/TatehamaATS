using System.Diagnostics;
using TatehamaATS.DisplayLED;
using TatehamaATS.Exceptions;

namespace TatehamaATS
{

    internal class ControlLED
    {
        public bool isShow;
        public bool isTest;
        public List<string> ExceptionCodes;
        private LEDWindow ledWindow;
        private int l3Index = 0; // display.L3のインデックスを追跡するための変数         
        private TimeSpan L3Start = new TimeSpan();
        private DateTime TestStart = new DateTime();
        public string? overrideText = null;

        public ControlLED()
        {
            ExceptionCodes = new List<string>();
            try
            {
                ledWindow = new LEDWindow();
                LEDShow();
            }
            catch (Exception ex)
            {
                throw new LEDControlInitialzingFailure(3, "ControlLED.cs@ControlLED()", ex);
            }
            Task.Run(() => StartDisplayUpdateLoop());
        }

        public void LEDHide()
        {
            if (ledWindow.InvokeRequired)
            {
                ledWindow.Invoke(new Action(() => LEDHide()));
                return;
            }
            ledWindow.Hide();
            isShow = false;
        }

        public void LEDShow()
        {
            if (ledWindow.IsDisposed)
            {
                ledWindow = new LEDWindow();
                ledWindow.Show();
                ledWindow.BringToFront();
                isShow = true;
            }
            else
            {
                ledWindow.Show();
                ledWindow.BringToFront();
                isShow = true;
            }
        }

        /// <summary>
        /// 非同期で表示器を更新するループを開始する
        /// </summary>
        private async void StartDisplayUpdateLoop()
        {
            while (true)
            {
                var timer = Task.Delay(20);
                try
                {
                    UpdateDisplay();
                }
                catch (ATSCommonException ex)
                {
                    MainWindow.inspectionRecord.AddException(ex);
                }
                catch (Exception ex)
                {
                    var e = new LEDControlException(3, "", ex);
                    MainWindow.inspectionRecord.AddException(e);
                }
                await timer;
            }
        }

        /// <summary>
        /// 表示を更新する
        /// </summary>
        private void UpdateDisplay()
        {
            //Debug.WriteLine(TrainState.ATSDisplay);
            if (TrainState.ATSLEDTest)
            {
                if (TestStart == DateTime.MinValue)
                {
                    TestStart = DateTime.Now;
                }
                var deltaT = DateTime.Now - TestStart;

                var LED = deltaT.Seconds % 3 + 319;
                var Place = deltaT.Seconds / 3 % 3 + 1;
                if (deltaT < TimeSpan.FromSeconds(9))
                {
                    ledWindow.DisplayImage(Place, LED);
                    if (Place == 1)
                    {
                        ledWindow.DisplayImage(2, 0);
                        ledWindow.DisplayImage(3, 0);
                    }
                    else if (Place == 2)
                    {
                        ledWindow.DisplayImage(1, 0);
                        ledWindow.DisplayImage(3, 0);
                    }
                    else if (Place == 3)
                    {
                        ledWindow.DisplayImage(1, 0);
                        ledWindow.DisplayImage(2, 0);
                    }
                    return;
                }
                TestStart = DateTime.MinValue;
                TrainState.ATSLEDTest = false;
            }
            if (TrainState.ATSDisplay != null)
            {

                var display = TrainState.ATSDisplay;
                if (display != null)
                {
                    if (ExceptionCodes.Count != 0)
                    {
                        display.L3 = ExceptionCodes;
                    }
                    else
                    {
                        display.RemoveError();
                    }
                    if (display.L3.Count == 0)
                    {
                        L3Start = TrainState.NowTime;
                        ledWindow.DisplayImage(1, ConvertToLEDNumber(overrideText != null ? overrideText : display.L1));
                        ledWindow.DisplayImage(2, ConvertToLEDNumber(display.L2));
                        ledWindow.DisplayImage(3, 0);
                    }
                    else
                    {
                        if (display.L3.Count == 1)
                        {
                            L3Start = TrainState.NowTime;
                        }
                        l3Index = (int)((TrainState.NowTime - L3Start).TotalSeconds * 2 + 1) % display.L3.Count;
                        ledWindow.DisplayImage(3, ConvertToLEDNumber(display.L3[l3Index]));

                        //L3に数値赤要素があるとき
                        if (display.L3.Contains("B動作") || display.L3.Contains("EB"))
                        {
                            //赤い方
                            ledWindow.DisplayImage(2, ConvertToLEDNumber(display.L2) + 100);
                        }
                        else
                        {
                            ledWindow.DisplayImage(2, ConvertToLEDNumber(display.L2));
                        }
                        ledWindow.DisplayImage(1, ConvertToLEDNumber(overrideText != null ? overrideText : display.L1));
                    }
                }
                else
                {
                    ledWindow.DisplayImage(1, 0);
                    ledWindow.DisplayImage(2, 0);
                    ledWindow.DisplayImage(3, 0);
                }
            }
            else
            {
                ledWindow.DisplayImage(1, 0);
                ledWindow.DisplayImage(2, 0);
                ledWindow.DisplayImage(3, 0);
            }
        }

        private int ConvertToLEDNumber(string str)
        {
            //16進数で解釈できる場合=故障表示の可能性
            if (int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out _))
            {
                int parse = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
                //数値が故障表示範囲内の場合
                if (0x180 <= parse && parse <= 0x1FF || 0x280 <= parse && parse <= 0x2FF || 0x380 <= parse && parse <= 0x3FF)
                {
                    return parse;
                }
            }
            if (int.TryParse(str, System.Globalization.NumberStyles.Number, null, out _))
            {
                int parse = int.Parse(str, System.Globalization.NumberStyles.Number);
                if (parse == 0)
                {
                    return 100;
                }
                if (parse == 5)
                {
                    return 101;
                }
                if (parse == 110)
                {
                    return 122;
                }
                if (parse == 112)
                {
                    return 122;
                }
                if (parse == 120)
                {
                    return 123;
                }
                if (parse == 130)
                {
                    return 124;
                }
                if (parse == 160)
                {
                    return 125;
                }
                if (parse == 300)
                {
                    return 126;
                }
                return (parse / 5) + 101;
            }
            switch (str)
            {
                case "":
                case "無表示":
                case null:
                    return 0;
                case "普通":
                    return 1;
                case "準急":
                    return 2;
                case "急行":
                    return 3;
                case "快急":
                    return 4;
                case "快速急行":
                    return 4;
                case "区急":
                    return 6;
                case "A特":
                    return 7;
                case "B特":
                    return 8;
                case "C特1":
                    return 9;
                case "C特2":
                    return 10;
                case "C特3":
                    return 11;
                case "C特4":
                    return 12;
                case "D特":
                    return 13;
                case "回送":
                    return 15;
                case "だんじり急行":
                    return 21;
                case "だんじり快急":
                    return 22;
                case "回送-2":
                    return 23;
                case "C特2-2":
                    return 24;
                case "F":
                    return 126;
                case "P":
                    return 300;
                case "P接近":
                    return 301;
                case "B動作":
                    return 302;
                case "EB":
                    return 303;
                case "終端P":
                    return 304;
                case "停P":
                    return 305;
                default:
                    throw new LEDDisplayStringAbnormal(3, $"未定義:{str}　ControlLED.cs@ConvertToLEDNumber()");
            }
        }
    }
}
