using SocketIO.Serializer.SystemTextJson;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using TatehamaATS.Database;
using TatehamaATS.Exceptions;

namespace TatehamaATS
{
    internal class SignalSocket
    {
        private SocketIOClient.SocketIO client;
        private bool isSocketConnect;
        private DateTime lastNormalResponse;
        private TimeSpan dataOKTime = TimeSpan.FromSeconds(3);
        private int elapseTimeoutCount = 0;
        private int enterSignalTimeoutCount = 0;
        private int leaveSignalTimeoutCount = 0;
        private const int TimeoutLimit = 5;

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
            //Debug.WriteLine($"進入：{track}");
            try
            {
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("enterSignal", data);
                client.Off("elapsed");
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
            //Debug.WriteLine($"進出：{track}");
            try
            {
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("leaveSignal", data);
                client.Off("elapsed");
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

        private async Task ConnectAndProcessAsync()
        {
            try
            {
                await client.ConnectAsync();
                isSocketConnect = true;
            }
            catch (Exception ex)
            {
                isSocketConnect = false;
                Debug.WriteLine($"再接続失敗: {ex.Message}");
                throw new SocketException(3, "SignalSocket.cs@ConnectAndProcessAsync()", ex);
            }
        }

        private async Task GetRoute()
        {
            if (TrainState.TrainDiaName == null)
            {
                return;
            }
            var diaName = TrainState.TrainDiaName;

            //非プレイアブル列番対照表に従って置換
            if (RetsubanTable.ContainsKey(diaName))
            {
                diaName = RetsubanTable[diaName];
            }
            await client.EmitAsync("getRoute", diaName);
            var ecs = new TaskCompletionSource<List<RawTrackCircuitInfo>>();
            client.On("getRouteResult", response =>
            {
                var route = response.GetValue<List<RawTrackCircuitInfo>>();
                ecs.SetResult(route);
            });
            try
            {
                // そんなに時間かからないと思うけど一応5秒待つ
                var route = await ecs.Task.WaitAsync(TimeSpan.FromSeconds(5));
                client.Off("route");
                TrainState.RouteDatabase = new RouteDatabase();
                TrainState.RouteDatabase.AddTrack(new TrackCircuitInfo("初期在線", -200d, route[0].startMeter, SignalLight.N, SignalType.Yudo_2));
                foreach (var track in route)
                {
                    TrainState.RouteDatabase.AddTrack(track.toTrackCircuitInfo());
                }
                TrainState.chengeDiaName = false;
                TrainState.RouteDatabaseCount = TrainState.RouteDatabase.CircuitList.Count;
                TrainState.OnTrackIndex = null;
                Debug.WriteLine(TrainState.RouteDatabase);
            }
            catch (TimeoutException ex)
            {
                throw new SocketTimeOutException(3, "SignalSocket.cs@GetRoute()", ex);
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
                    TrainState.ATSBroken = true;
                    Debug.WriteLine($"故障");
                    Debug.WriteLine($"{ex}");
                    Debug.WriteLine($"{ex.InnerException}");
                    TrainState.ATSDisplay?.SetLED("", "");
                    TrainState.ATSDisplay?.AddState(ex.ToCode());
                }
                catch (Exception ex)
                {
                    TrainState.ATSBroken = true;
                    Debug.WriteLine($"故障");
                    Debug.WriteLine($"{ex}");
                    Debug.WriteLine($"{ex.InnerException}");
                    var e = new SocketCountaException(3, "", ex);
                    TrainState.ATSDisplay?.SetLED("", "");
                    TrainState.ATSDisplay?.AddState(e.ToCode());
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
                var elapsed = await tcs.Task.WaitAsync(TimeSpan.FromSeconds(1));
                client.Off("elapsed");
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
                var elapsed2 = await tcs2.Task.WaitAsync(TimeSpan.FromSeconds(1));
                client.Off("elapsed");
                elapseTimeoutCount = 0;
                lastNormalResponse = DateTime.Now;

                //Debug.WriteLine($"{elapsed.signalName}/{elapsed.signalPhase}/{elapsed.signalType}");
                TrainState.RouteDatabase?.SetSignalLight(elapsed.signalName, elapsed.signalPhase);
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

        private Dictionary<string, string> RetsubanTable = new Dictionary<string, string> {
            {"回451", "551"},
            {"回1013A", "回1105A"},
            {"1112A", "1204A"},
            {"1017A", "1107A"},
            {"回1216A", "回1306A"},
            {"回1217A", "回1105A"},
            {"1316A", "1204A"},
            {"1075", "1261"},
            {"1174", "1260"},
            {"1275", "1261"},
            {"1274", "1166"},
            {"1060", "1166"},
            {"1161", "1267"},
            {"1183C", "1283C"},
            {"1182C", "1282C"},
            {"1168C", "1284C"},
            {"1169C", "1285C"},
            {"1268C", "1284C"},
            {"1269C", "1285C"},
            {"1011A", "1209A"},
            {"1110A", "1204A"},
            {"1103A", "1209A"},
            {"回1104A", "回1306A"},
            {"1164", "1260"},
            {"1165", "1261"},
            {"1264", "1260"},
            {"1167", "1261"},
            {"1185C", "1285C"},
            {"1184C", "1284C"},
            {"回7290", "回862"},
            {"回7291", "回607A"},
            {"回1295", "回607A"},
            {"1295B", "回607A"},
            {"回1294", "回862"},
            {"1294K", "796K"},
            {"回1281", "回607A"},
            {"1281B", "回607A"},
            {"回1280", "回862"},
        };
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
        public SignalType signalType { get; set; }
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
