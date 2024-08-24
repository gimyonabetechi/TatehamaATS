using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using TatehamaATS.Exceptions;
using TrainCrew;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace TatehamaATS
{
    /// <summary>
    /// 継電部
    /// </summary>
    internal class Relay
    {
        public event EventHandler Init;
        public static bool EB;
        private TimeSpan ShiftTime = TimeSpan.FromHours(10);
        private Label Clock;
        internal Relay(Label clock)
        {
            Clock = clock;
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

        internal void Time_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShiftTime += TimeSpan.FromHours(1);
            }
            else
            {
                ShiftTime -= TimeSpan.FromHours(1);
            }
        }

        internal void ClockChenge()
        {
            if (Clock != null && Clock.IsHandleCreated)
            {
                Clock.Invoke((MethodInvoker)(() =>
                {
                    Clock.Text = $"現在時刻　{DateTime.Now - ShiftTime:HH:mm:ss}";
                }));
            }
        }

        /// <summary>
        /// 非同期で表示器を更新するループを開始する
        /// </summary>
        private async void StartUpdateLoop()
        {
            while (true)
            {
                ClockChenge();
                var timer = Task.Delay(100);
                try
                {
                    Elapse();
                }
                catch (ATSCommonException ex)
                {
                    MainWindow.inspectionRecord.AddException(ex);
                }
                catch (Exception ex)
                {
                    var e = new RelayException(3, "", ex);
                    MainWindow.inspectionRecord.AddException(e);
                }
                await timer;
            }
        }

        internal void Elapse()
        {
            try
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
                        new RelayOtherInfoAbnormal(3, "Relay.cs@Elapse()", ex);
                    }

                    //ゲーム状態の取得
                    if (!(TrainState.gameScreen == GameScreen.MainGame || TrainState.gameScreen == GameScreen.MainGame_Pause))
                    {
                        //ゲーム中orゲームポーズ中ではない
                        if (TrainState.RouteDatabase != null)
                        {
                            if (TrainState.OnTrackIndex == TrainState.RouteDatabase.CircuitList.Count - 1 && TrainState.OnTrack != null)
                            {
                                //最終トラック
                                switch (TrainState.OnTrack.Name)
                                {
                                    case "館浜下り場内1LA":
                                    case "館浜下り場内1LB":
                                    case "館浜下り場内1LC":
                                    case "館浜下り場内1LD":
                                    case "新野崎入換111R":
                                    case "大道寺上り場内8R":
                                    case "大道寺入換105R":
                                    case "大道寺入換110R":
                                        //在線解除しない
                                        break;
                                    default:
                                        _ = MainWindow.signalSocket.leaveSignal(TrainState.OnTrack);
                                        break;
                                }

                            }
                            MainWindow.retsuban?.Init();
                            TrainState.init();
                        }
                    }
                    else
                    {
                        //ゲーム中orゲームポーズ中
                        if (MainWindow.retsuban != null && MainWindow.retsuban.NowSelect == 0)
                        {
                            Debug.WriteLine("リセット");
                            MainWindow.retsuban?.Load();
                        }
                    }

                    //自車車種の取得
                    try
                    {
                        TrainState.TrainName = TC_TrainState.CarStates[0].CarModel;
                    }
                    catch (Exception ex)
                    {
                        new RelayCarTypeAbnormal(3, "Relay.cs@Elapse()", ex);
                    }

                    if (TrainState.gameScreen == GameScreen.MainGame || TrainState.gameScreen == GameScreen.MainGame_Pause)
                    {
                        //運転中
                        switch (TrainState.TrainName)
                        {
                            case "50000":
                            case "5320":
                            case "5300":
                            case "4321":
                            case "4300":
                            case "4000R":
                            case "4000":
                            case "3300V":
                            case "3300":
                                if (EB)
                                {
                                    TrainCrewInput.SetATO_Notch(-8);
                                }
                                else
                                {
                                    TrainCrewInput.SetATO_Notch(0);
                                }
                                break;
                            case "3020":
                            case "3000":
                                if (EB)
                                {
                                    TrainCrewInput.SetATO_Notch(-9);
                                }
                                else
                                {
                                    TrainCrewInput.SetATO_Notch(0);
                                }
                                break;
                            default:
                                throw new RelayCarTypeAbnormal(3, $"未定義車種{TrainState.TrainName}");
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
                        new RelayCarInfoAbnormal(3, "Relay.cs@Elapse()", ex);
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
                                if (track.StartMeter < nowDis && nowDis < track.EndMeter)
                                {
                                    break;
                                }
                                _ = MainWindow.signalSocket.leaveSignal(track);
                                TrainState.OnTrackIndex++;
                            }
                            TrackInfoGet(TrainState.OnTrackIndex == 0);
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
                                    _ = MainWindow.signalSocket.leaveSignal(TrainState.BeforeTrack);
                                    TrainState.BeforeTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex - 1];
                                }
                                TrackInfoGet();
                                Debug.WriteLine("現在在線を先頭が越えたら");
                                _ = MainWindow.signalSocket.enterSignal(TrainState.OnTrack);
                            }
                            //在線を先頭が踏んで戻ったら 
                            if (TrainState.OnTrack.StartMeter > nowDis)
                            {
                                TrainState.OnTrackIndex--;
                                TrackInfoGet();
                                Debug.WriteLine("在線を先頭が踏んで戻ったら");
                                _ = MainWindow.signalSocket.leaveSignal(TrainState.NextTrack);
                            }
                            //在線を最後尾が踏んで戻ったら 
                            if (TrainState.OnTrack.StartMeter > nowDis - TrainState.TrainLength && TrainState.BeforeTrack == null)
                            {
                                TrainState.BeforeTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex - 1];
                                Debug.WriteLine("在線を最後尾が踏んで戻ったら");
                                _ = MainWindow.signalSocket.enterSignal(TrainState.BeforeTrack);
                            }

                            //進入完了処理
                            if (TrainState.OnTrack.EndMeter - (nowDis - TrainState.TrainLength) < 150)
                            {
                                //ケツが150mならいくら何でも入りきってるで
                                if (!TrainState.OnTrack.enterComp)
                                {
                                    _ = MainWindow.signalSocket.enteringComplete(TrainState.OnTrack);
                                    TrainState.OnTrack.enterComp = true;
                                }
                            }
                            else if (TrainState.OnTrack.EndMeter - (nowDis - TrainState.TrainLength) < 200 && TrainState.TrainSpeed == 0)
                            {
                                //ケツが200m以内で停止したら入線しきってると思う    
                                if (!TrainState.OnTrack.enterComp)
                                {
                                    _ = MainWindow.signalSocket.enteringComplete(TrainState.OnTrack);
                                    TrainState.OnTrack.enterComp = true;
                                }
                            }
                            else if (TrainState.OnTrackIndex == 0)
                            {
                                //0番軌道回路の場合は強制OK
                                if (!TrainState.OnTrack.enterComp)
                                {
                                    _ = MainWindow.signalSocket.enteringComplete(TrainState.OnTrack);
                                    TrainState.OnTrack.enterComp = true;
                                }
                            }
                            else
                            {
                                TrainState.OnTrack.enterComp = false;
                            }
                        }
                        if (TrainState.BeforeTrack != null)
                        {
                            //前在線を最後尾が抜けたら  
                            if (TrainState.BeforeTrack.EndMeter < nowDis - TrainState.TrainLength)
                            {
                                Debug.WriteLine("前在線を最後尾が抜けたら");
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
                catch (ATSCommonException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new RelayOtherInfoAbnormal(3, "Relay.cs@Elapse()", ex);
                }
            }
            catch (ATSCommonException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new RelayException(3, "Relay.cs@Elapse()", ex);
            }
        }
        private void TrackInfoGet(bool first = false)
        {
            if (TrainState.RouteDatabase != null && TrainState.OnTrackIndex != null)
            {
                if ((int)TrainState.OnTrackIndex < TrainState.RouteDatabaseCount)
                {
                    TrainState.OnTrack = TrainState.RouteDatabase.CircuitList[(int)TrainState.OnTrackIndex];
                    if (first)
                    {
                        _ = MainWindow.signalSocket.enteringComplete(TrainState.OnTrack);
                        TrainState.OnTrack.enterComp = true;
                    }
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
