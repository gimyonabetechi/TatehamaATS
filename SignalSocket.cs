using SocketIO.Serializer.SystemTextJson;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using TatehamaATS.Database;
using TatehamaATS.Exceptions;
using TatehamaATS.Diadata;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace TatehamaATS
{
    internal class SignalSocket
    {
        private SocketIOClient.SocketIO client;
        private bool isSocketConnect;
        private DateTime lastNormalResponse;
        private TimeSpan dataOKTime = TimeSpan.FromSeconds(5);
        private int elapseTimeoutCount = 0;
        private int enterSignalTimeoutCount = 0;
        private int leaveSignalTimeoutCount = 0;
        private int enteringCompleteTimeoutCount = 0;
        private const int TimeoutLimit = 5;
        private DiaNameToDatebaseName DiaNameToDatebaseName = new DiaNameToDatebaseName();
        private DiaNameTrack DiaNameTrack = new DiaNameTrack();
        private TypeLED TypeLED = new TypeLED();

        internal SignalSocket()
        {
            isSocketConnect = false;
            // Todo: Dev/Prod変更できるようにする
            client = new SocketIOClient.SocketIO(Program.ServerAddress);
            var config = new JsonSerializerOptions();
            config.Converters.Add(new JsonStringEnumConverter());
            client.Serializer = new SystemTextJsonSerializer(config);

            Task.Run(() => ConnectAndProcessAsync());
            Task.Run(() => UpdateLoop());
        }

        internal async Task enterSignal(TrackCircuitInfo? track)
        {
            if (TrainState.TrainDiaName == null || TrainState.TrainDiaName == "" || track == null)
            {
                //Debug.WriteLine("return");
                return;
            }
            Debug.WriteLine($"進入：{track}");
            try
            {
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("enterSignal", data);
                enterSignalTimeoutCount = 0;
            }
            catch (TimeoutException ex)
            {
                enterSignalTimeoutCount++;
                if (enterSignalTimeoutCount >= TimeoutLimit)
                {
                    throw new SocketTimeOutException(3, "SignalSocket.cs@enterSignal()", ex);
                }
                else
                {
                    await enterSignal(track);
                }
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@enterSignal()", ex);
            }
        }

        internal async Task leaveSignal(TrackCircuitInfo track)
        {
            if (TrainState.TrainDiaName == null || TrainState.TrainDiaName == "" || track == null)
            {
                //Debug.WriteLine("return");
                return;
            }
            Debug.WriteLine($"進出：{track}");
            try
            {
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("leaveSignal", data);
                leaveSignalTimeoutCount = 0;
            }
            catch (TimeoutException ex)
            {
                leaveSignalTimeoutCount++;
                if (leaveSignalTimeoutCount >= TimeoutLimit)
                {
                    throw new SocketTimeOutException(3, "SignalSocket.cs@leaveSignal()", ex);
                }
                else
                {
                    await leaveSignal(track);
                }
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@leaveSignal()", ex);
            }
        }
        internal async Task enteringComplete(TrackCircuitInfo track)
        {
            if (TrainState.TrainDiaName == null || TrainState.TrainDiaName == "" || track == null)
            {
                //Debug.WriteLine("return");
                return;
            }
            Debug.WriteLine($"進完：{track}");
            try
            {
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("enteringComplete", data);
                leaveSignalTimeoutCount = 0;
            }
            catch (TimeoutException ex)
            {
                enteringCompleteTimeoutCount++;
                if (enteringCompleteTimeoutCount >= TimeoutLimit)
                {
                    throw new SocketTimeOutException(3, "SignalSocket.cs@leaveSignal()", ex);
                }
                else
                {
                    await enteringComplete(track);
                }
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@leaveSignal()", ex);
            }
        }

        private async Task ConnectAndProcessAsync()
        {
            while (true)
            {
                try
                {
                    if (isSocketConnect == true)
                    {
                        await Task.Delay(5000);
                    }
                    else
                    {
                        await client.ConnectAsync();
                        isSocketConnect = true;
                    }
                }
                catch (Exception ex)
                {
                    isSocketConnect = false;
                    var e = new SocketException(3, "SignalSocket.cs@ConnectAndProcessAsync()", ex);
                    MainWindow.inspectionRecord.AddException(e);
                }
            }
        }

        private async Task GetRoute()
        {
            if (TrainState.TrainDiaName == null && TrainState.TrainCar == null)
            {
                return;
            }
            var diaName = DiaNameToDatebaseName.ChengeDiaName(TrainState.TrainDiaName);

            await client.EmitAsync("getRoute", $"{diaName}_{TrainState.TrainCar}");
            var ecs = new TaskCompletionSource<List<RawTrackCircuitInfo>>();
            client.On("getRouteResult", response =>
            {
                var route = response.GetValue<List<RawTrackCircuitInfo>>();
                ecs.SetResult(route);
            });
            try
            {
                //故障復帰最低要件の関係で2秒待ち
                var route = await ecs.Task.WaitAsync(TimeSpan.FromSeconds(2));
                client.Off("route");
                TrainState.RouteDatabase = new RouteDatabase();
                TrainState.RouteDatabase.AddTrack(new TrackCircuitInfo("初期在線", -200d, route[0].startMeter, SignalLight.N, SignalType.Yudo_2));
                route[route.Count - 1].endMeter += 20;
                foreach (var track in route)
                {
                    TrainState.RouteDatabase.AddTrack(track.toTrackCircuitInfo());
                }

                DiaNameTrack.ChengeTrack(TrainState.TrainDiaName);
                TypeLED.ChengeTypeLED(TrainState.TrainDiaName);

                TrainState.chengeDiaName = false;
                TrainState.RouteDatabaseCount = TrainState.RouteDatabase.CircuitList.Count;
                TrainState.OnTrackIndex = null;
                Debug.WriteLine(TrainState.RouteDatabase);
            }
            catch (TimeoutException ex)
            {
                throw new OnCarDBDataGetException(3, "SignalSocket.cs@GetRoute()", ex);
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@GetRoute()", ex);
            }
        }

        /// <summary>
        /// 定常ループ
        /// </summary>
        /// <returns></returns>
        private async Task UpdateLoop()
        {
            while (true)
            {
                if (!isSocketConnect)
                {

                    continue;
                }
                var timer = Task.Delay(500);
                try
                {
                    await Elapse();
                    if (TrainState.chengeDiaName)
                    {
                        await GetRoute();
                    }
                }
                catch (ATSCommonException ex)
                {
                    MainWindow.inspectionRecord.AddException(ex);
                }
                catch (Exception ex)
                {
                    var e = new SocketException(3, "", ex);
                    MainWindow.inspectionRecord.AddException(e);
                }
                await timer;
            }
        }

        private async Task Elapse()
        {
            if (TrainState.TrainDiaName == null || TrainState.TrainDiaName == "" || TrainState.NextTrack == null)
            {
                //Debug.WriteLine("return");
                lastNormalResponse = DateTime.Now;
                return;
            }
            //Debug.WriteLine(TrainState.BeforeTrack);
            //Debug.WriteLine(TrainState.OnTrack);
            //Debug.WriteLine(TrainState.OnTrackIndex);
            //Debug.WriteLine(TrainState.NextTrack);
            try
            {
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = TrainState.NextTrack.Name
                };
                await client.EmitAsync("elapse", data);
                var tcs = new TaskCompletionSource<ElapsedData>();
                client.On("elapsed", response =>
                {
                    var elapsedData = response.GetValue<ElapsedData>();
                    tcs.SetResult(elapsedData);
                });
                var elapsed = await tcs.Task.WaitAsync(TimeSpan.FromMilliseconds(500));
                client.Off("elapsed");
                TrainState.RouteDatabase?.SetSignalLight(elapsed.signalName, elapsed.signalPhase);
                if (TrainState.NextNextTrack != null)
                {
                    var data2 = new CommonData
                    {
                        diaName = TrainState.TrainDiaName,
                        signalName = TrainState.NextNextTrack.Name
                    };
                    await client.EmitAsync("elapse", data2);
                    var tcs2 = new TaskCompletionSource<ElapsedData>();
                    client.On("elapsed", response =>
                    {
                        var elapsedData = response.GetValue<ElapsedData>();
                        tcs2.SetResult(elapsedData);
                    });
                    var elapsed2 = await tcs2.Task.WaitAsync(TimeSpan.FromMilliseconds(500));
                    client.Off("elapsed");
                    TrainState.RouteDatabase?.SetSignalLight(elapsed2.signalName, elapsed2.signalPhase);
                }
                elapseTimeoutCount = 0;
                lastNormalResponse = DateTime.Now;
            }
            catch (TimeoutException ex)
            {
                elapseTimeoutCount++;
                if (elapseTimeoutCount >= TimeoutLimit)
                {
                    throw new SocketTimeOutException(3, "SignalSocket.cs@Elapse()", ex);
                }
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@Elapse()", ex);
            }
            finally
            {
                if ((DateTime.Now - lastNormalResponse) > dataOKTime)
                {
                    throw new SocketDataExpired(3, "SignalSocket.cs@Elapse()");
                }
            }
        }
    }

    class CommonData
    {
        public string diaName { get; set; }
        public string signalName { get; set; }
    }

    class ElapsedData
    {
        public string signalName { get; set; }
        public SignalLight signalPhase { get; set; }
        public string signalType { get; set; }
    }

    class RawTrackCircuitInfo
    {
        public string name { get; set; }
        public double startMeter { get; set; }
        public double endMeter { get; set; }
        public SignalType signalType { get; set; }

        public TrackCircuitInfo toTrackCircuitInfo()
        {
            return new TrackCircuitInfo(name, startMeter, endMeter, SignalLight.N, signalType);
        }
    }
}
