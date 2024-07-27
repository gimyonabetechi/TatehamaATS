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
                var timer = Task.Delay(20);
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

                //ゲーム状態の取得
                if (!(TrainState.gameScreen == GameScreen.MainGame || TrainState.gameScreen == GameScreen.MainGame_Pause))
                {
                    if (TrainState.RouteDatabase != null)
                    {
                        foreach (var track in TrainState.RouteDatabase.CircuitList)
                        {
                            _ = MainWindow.signalSocket.leaveSignal(track);
                        }
                        if (MainWindow.retsuban != null && MainWindow.retsuban.NowSelect != 0)
                        {
                            MainWindow.retsuban.Init();
                        }
                    }
                    TrainState.init();
                }
                else
                {
                    if (MainWindow.retsuban != null && MainWindow.retsuban.NowSelect == 0)
                    {
                        MainWindow.retsuban?.Load();
                    }
                }

                //自車速度の取得
                try
                {
                    TrainState.TrainSpeed = TC_TrainState.Speed;
                }
                catch (Exception ex)
                {
                    new TGAbnormalException(3, "Relay.cs@Elapse()", ex);
                }

                //ノッチ・現在時刻取得
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

                //現在のATS表示器取得
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

                //軌道回路情報あり
                if (TrainState.RouteDatabase != null)
                {
                    var nowDis = TC_TrainState.TotalLength;
                    //Debug.WriteLine($"{nowDis}");
                    //現在軌道回路インデックス不明 ≒ リセット時
                    if (TrainState.OnTrackIndex == null)
                    {
                        TrainState.OnTrackIndex = 0;
                        foreach (var track in TrainState.RouteDatabase.CircuitList)
                        {
                            _ = MainWindow.signalSocket.leaveSignal(track);
                            if (track.StartMeter < nowDis && nowDis < track.EndMeter)
                            {
                                break;
                            }
                            TrainState.OnTrackIndex++;
                        }
                        TrackInfoGet();
                    }

                    //現在軌道回路あり
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
                            TrackInfoGet();
                            //Debug.WriteLine("現在在線を先頭が越えたら");
                            _ = MainWindow.signalSocket.enterSignal(TrainState.OnTrack);
                        }
                        //在線を先頭が踏んで戻ったら 
                        if (TrainState.OnTrack.StartMeter > nowDis)
                        {
                            TrainState.OnTrackIndex--;
                            TrackInfoGet();
                            //Debug.WriteLine("在線を先頭が踏んで戻ったら");
                            _ = MainWindow.signalSocket.leaveSignal(TrainState.NextTrack);
                        }
                        //在線を最後尾が踏んで戻ったら 
                        if (TrainState.OnTrack.StartMeter > nowDis - TrainState.TrainLength && TrainState.BeforeTrack == null)
                        {
                            TrainState.BeforeTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex - 1];
                            //Debug.WriteLine("在線を最後尾が踏んで戻ったら");
                            _ = MainWindow.signalSocket.enterSignal(TrainState.BeforeTrack);
                        }


                        if (!TrainState.OnTrack.enterComp)
                        {
                            //進入完了処理
                            if (TrainState.OnTrack.EndMeter - (nowDis - TrainState.TrainLength) < 130)
                            {
                                //ケツが130mならいくら何でも入りきってるで
                                _ = MainWindow.signalSocket.enteringComplete(TrainState.OnTrack);
                                TrainState.OnTrack.enterComp = true;
                            }
                            else if (TrainState.OnTrack.EndMeter - (nowDis - TrainState.TrainLength) < 170 && TrainState.TrainSpeed == 0)
                            {
                                //ケツが170m以内で停止したら入線しきってると思う
                                _ = MainWindow.signalSocket.enteringComplete(TrainState.OnTrack);
                                TrainState.OnTrack.enterComp = true;
                            }
                        }
                    }
                    if (TrainState.BeforeTrack != null)
                    {
                        //前在線を最後尾が抜けたら  
                        if (TrainState.BeforeTrack.EndMeter < nowDis - TrainState.TrainLength)
                        {
                            //Debug.WriteLine("前在線を最後尾が抜けたら");
                            _ = MainWindow.signalSocket.leaveSignal(TrainState.BeforeTrack);
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


        private void TrackInfoGet()
        {
            if (TrainState.RouteDatabase != null && TrainState.OnTrackIndex != null)
            {
                if ((int)TrainState.OnTrackIndex < TrainState.RouteDatabaseCount)
                {
                    TrainState.OnTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex];
                    _ = MainWindow.signalSocket.enterSignal(TrainState.OnTrack);
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
        }
    }
}
