using TatehamaATS.DisplayLED;
using TatehamaATS.Exceptions;

namespace TatehamaATS
{
    internal class ControlLED
    {
        public bool isShow;
        private LEDWindow ledWindow;
        private int l3Index = 0; // display.L3のインデックスを追跡するための変数

        public ControlLED()
        {
            try
            {
                ledWindow = new LEDWindow();
                LEDHide();
            }
            catch (Exception ex)
            {
                throw new LEDControlInitialzingFailure(3, "ControlLED.cs@ControlLED()", ex);
            }

            // 非同期メソッドを呼び出すが、戻り値のTaskを保存しておく                        
            StartDisplayUpdateLoop();
        }

        public void LEDHide()
        {
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
                try
                {
                    UpdateDisplay();
                }
                catch (ATSCommonException ex)
                {
                    // ここで例外をキャッチしてログなどに出力する
                    TrainState.ATSDisplay = new ATSDisplay("", "", new[] { ex.ToCode() });
                }
                catch (Exception ex)
                {
                    // 他の例外もキャッチしてログなどに出力する
                    Console.WriteLine($"例外が発生しました: {ex.Message}");
                }

                await Task.Delay(100); // 100msごとに更新
            }
        }

        /// <summary>
        /// 表示を更新する
        /// </summary>
        private void UpdateDisplay()
        {
            if (TrainState.TC_ATSDisplay != null && TrainState.ATSDisplay == null)
            {
                TrainState.ATSDisplay = TrainState.TC_ATSDisplay;
            }
            if (TrainState.ATSDisplay != null)
            {
                var display = TrainState.ATSDisplay;

                if (display != null)
                {
                    ledWindow.DisplayImage(1, ConvertToLEDNumber(display.L1));
                    ledWindow.DisplayImage(2, ConvertToLEDNumber(display.L2));
                    ledWindow.DisplayImage(3, ConvertToLEDNumber(display.L3[l3Index]));

                    // display.L3の次のインデックスに移動
                    l3Index = (l3Index + 1) % display.L3.Length;
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
                int parse = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
                if (parse == 0)
                {
                    return 100;
                }
                if (parse == 5)
                {
                    return 101;
                }
                return (parse / 5) + 101;
            }
            switch (str)
            {
                case "":
                    return 0;
                case "普通":
                    return 1;
                default:
                    throw new LEDDisplayStringAbnormal(3, $"未定義:{str}　ControlLED.cs@ConvertToLEDNumber()");
            }
        }
    }
}
