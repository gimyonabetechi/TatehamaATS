using System.Diagnostics;
using TatehamaATS.Exceptions;
using TrainCrew;

namespace TatehamaATS
{
    /// <summary>
    /// 継電部
    /// </summary>
    internal class Relay
    {
        internal Relay()
        {
            try
            {
                TrainCrewInput.Init();
            }
            catch (Exception ex)
            {
                new RelayInitialzingFailure(3, "Relay.cs@Relay()", ex);
            }
            Task.Run(() => StartUpdateLoop());

        }

        /// <summary>
        /// 非同期で表示器を更新するループを開始する
        /// </summary>
        private async void StartUpdateLoop()
        {


            while (true)
            {
                var timer = Task.Delay(15);
                try
                {
                    Elapse();
                }
                catch (ATSCommonException ex)
                {
                    // ここで例外をキャッチしてログなどに出力する     
                    TrainState.ATSBroken = true;
                    Debug.WriteLine($"故障");
                    TrainState.ATSDisplay?.SetLED("", "");
                    TrainState.ATSDisplay?.AddState(ex.ToCode());
                    Debug.WriteLine($"{ex.Message}");
                }
                catch (Exception ex)
                {
                    // 他の例外もキャッチしてログなどに出力する     
                    TrainState.ATSBroken = true;
                    Debug.WriteLine($"{ex.Message}");
                    var e = new RelayException(3, "", ex);
                    TrainState.ATSDisplay?.SetLED("", "");
                    TrainState.ATSDisplay?.AddState(e.ToCode());
                }
                await timer;
            }
        }

        internal void Elapse()
        {
            try
            {
                var TC_TrainState = TrainCrewInput.GetTrainState();

                try
                {
                    TrainState.gameScreen = TrainCrewInput.gameState.gameScreen;
                }
                catch (Exception ex)
                {
                    new RelayException(3, "Relay.cs@Elapse()", ex);
                }
                try
                {
                    TrainState.TrainSpeed = TC_TrainState.Speed;
                }
                catch (Exception ex)
                {
                    new TGAbnormalException(3, "Relay.cs@Elapse()", ex);
                }
                try
                {
                    TrainState.TrainPnotch = TC_TrainState.Pnotch;
                    TrainState.TrainBnotch = TC_TrainState.Bnotch;
                    TrainState.NowTime = TC_TrainState.NowTime;
                }
                catch (Exception ex)
                {
                    new RelayException(3, "Relay.cs@Elapse()", ex);
                }
                try
                {
                    //TrainState.TC_ATSDisplay = new ATSDisplay(TC_TrainState.ATS_Class, TC_TrainState.ATS_Speed, [TC_TrainState.ATS_State]);
                    TrainState.ATSDisplay?.SetLED(TC_TrainState.ATS_Class, TC_TrainState.ATS_Speed);
                    TrainState.ATSDisplay?.AddState(TC_TrainState.ATS_State);
                }
                catch (Exception ex)
                {
                    new TCSideATSDataAbnormalException(3, "Relay.cs@Elapse()", ex);
                }
            }
            catch (Exception ex)
            {
                throw new RelayException(3, "Relay.cs@Elapse()", ex);
            }
        }
    }
}
