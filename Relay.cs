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
        public event EventHandler Init;
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
                if (!(TrainState.gameScreen == GameScreen.MainGame || TrainState.gameScreen == GameScreen.MainGame_Pause))
                {
                    TrainState.init();
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
                if (TrainState.RouteDatabase != null)
                {
                    if (TrainState.OnTrackIndex == null)
                    {
                        TrainState.OnTrackIndex = 0;
                        TrainState.BeforeTrack = null;
                        TrainState.OnTrack = TrainState.RouteDatabase.CircuitList[0];
                        TrainState.NextTrack = TrainState.RouteDatabase.CircuitList[1];
                        TrainState.NextNextTrack = TrainState.RouteDatabase.CircuitList[2];
                    }
                    var nowDis = TC_TrainState.TotalLength;

                    if (TrainState.OnTrack != null)
                    {
                        //現在在線を先頭が越えたら
                        if (TrainState.OnTrack.EndMeter < nowDis)
                        {
                            TrainState.OnTrackIndex++;
                            if ((int)TrainState.OnTrackIndex - 1 < TrainState.RouteDatabaseCount)
                            {
                                TrainState.BeforeTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex - 1];
                            }
                            if ((int)TrainState.OnTrackIndex < TrainState.RouteDatabaseCount)
                            {
                                TrainState.OnTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex];
                            }
                            if ((int)TrainState.OnTrackIndex + 1 < TrainState.RouteDatabaseCount)
                            {
                                TrainState.NextTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex + 1];
                            }
                            else
                            {
                                TrainState.NextTrack = null;
                            }
                            if ((int)TrainState.OnTrackIndex + 2 < TrainState.RouteDatabaseCount)
                            {
                                TrainState.NextNextTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex + 2];
                            }
                            else
                            {
                                TrainState.NextNextTrack = null;
                            }
                        }
                        //在線を先頭が踏んで戻ったら 
                        if (TrainState.OnTrack.StartMeter > nowDis && TrainState.BeforeTrack == null)
                        {
                            TrainState.OnTrackIndex--;
                            if ((int)TrainState.OnTrackIndex - 1 < TrainState.RouteDatabaseCount)
                            {
                                TrainState.BeforeTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex - 1];
                            }
                            if ((int)TrainState.OnTrackIndex < TrainState.RouteDatabaseCount)
                            {
                                TrainState.OnTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex];
                            }
                            if ((int)TrainState.OnTrackIndex + 1 < TrainState.RouteDatabaseCount)
                            {
                                TrainState.NextTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex + 1];
                            }
                            else
                            {
                                TrainState.NextTrack = null;
                            }
                            if ((int)TrainState.OnTrackIndex + 2 < TrainState.RouteDatabaseCount)
                            {
                                TrainState.NextNextTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex + 2];
                            }
                            else
                            {
                                TrainState.NextNextTrack = null;
                            }
                        }
                        //在線を最後尾が踏んで戻ったら 
                        if (TrainState.OnTrack.StartMeter > nowDis - TrainState.TrainLength && TrainState.BeforeTrack == null)
                        {
                            TrainState.BeforeTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex - 1];
                        }
                    }
                    if (TrainState.BeforeTrack != null)
                    {
                        //前在線を最後尾が抜けたら  
                        if (TrainState.BeforeTrack.EndMeter < nowDis - TrainState.TrainLength)
                        {
                            TrainState.BeforeTrack = null;
                        }
                    }
                }
                else
                {
                    TrainState.OnTrackIndex = null;
                }
            }
            catch (Exception ex)
            {
                throw new RelayException(3, "Relay.cs@Elapse()", ex);
            }
        }
    }
}
