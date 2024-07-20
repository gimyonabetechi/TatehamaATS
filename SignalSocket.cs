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
                Debug.WriteLine("return");
                return;
            }
            Debug.WriteLine($"進入：{track}");
            try
            {
                //こっからフレーム処理
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("enterSignal", data);
                // 念の為明示的にイベントを解除
                client.Off("elapsed");
            }
            catch (TimeoutException ex)
            {
                throw new SocketTimeOutException(3, "SignalSocket.cs@enterSignal()", ex);
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
                Debug.WriteLine("return");
                return;
            }
            Debug.WriteLine($"進出：{track}");
            try
            {
                //こっからフレーム処理
                var data = new CommonData
                {
                    diaName = TrainState.TrainDiaName,
                    signalName = track.Name
                };
                await client.EmitAsync("leaveSignal", data);
                // 念の為明示的にイベントを解除
                client.Off("elapsed");
            }
            catch (TimeoutException ex)
            {
                throw new SocketTimeOutException(3, "SignalSocket.cs@enterSignal()", ex);
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@enterSignal()", ex);
            }
        }

        private async Task ConnectAndProcessAsync()
        {
            await client.ConnectAsync();
            isSocketConnect = true;
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
                // 念の為明示的にイベントを解除
                client.Off("route");
                // ここでサーバーから来た結果に応じた処理を書く
                TrainState.RouteDatabase = new RouteDatabase();
                TrainState.RouteDatabase.AddTrack(new TrackCircuitInfo("初期在線", -200d, route[0].startMeter, SignalLight.N, SignalType.Yudo_2));
                foreach (var track in route)
                {
                    TrainState.RouteDatabase.AddTrack(track.toTrackCircuitInfo());
                }
                //TrainState.RouteDatabase.AddTrack(new TrackCircuitInfo("最終在線", route[route.Count - 1].endMeter,100000d, SignalLight.N, SignalType.Yudo_2));
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
                var timer = Task.Delay(1000);
                try
                {
                    await Elapse();
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
                    var e = new SocketException(3, "", ex);
                    TrainState.ATSDisplay?.SetLED("", "");
                    TrainState.ATSDisplay?.AddState(e.ToCode());
                }
                if (TrainState.chengeDiaName)
                {
                    try
                    {
                        await GetRoute();
                    }
                    catch (ATSCommonException ex)
                    {
                        TrainState.chengeDiaName = true;
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
                        TrainState.chengeDiaName = true;
                        // 他の例外もキャッチしてログなどに出力する    
                        TrainState.ATSBroken = true;
                        Debug.WriteLine($"故障");
                        Debug.WriteLine($"{ex.Message} {ex.InnerException}");
                        var e = new SocketException(3, "", ex);
                        TrainState.ATSDisplay?.SetLED("", "");
                        TrainState.ATSDisplay?.AddState(e.ToCode());
                    }
                }
                await timer;
            }
        }


        private async Task Elapse()
        {
            if (TrainState.TrainDiaName == null || TrainState.TrainDiaName == "" || TrainState.NextTrack == null)
            {
                Debug.WriteLine("return");
                return;
            }
            Debug.WriteLine(TrainState.BeforeTrack);
            Debug.WriteLine(TrainState.OnTrack);
            Debug.WriteLine(TrainState.OnTrackIndex);
            Debug.WriteLine(TrainState.NextTrack);
            try
            {
                //こっからフレーム処理
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
                // 念の為明示的にイベントを解除
                client.Off("elapsed");
                // ここでサーバーから来た結果に応じた処理を書く
                Debug.WriteLine($"{elapsed.signalName}/{elapsed.signalPhase}/{elapsed.signalType}");
                SignalLight signalLight;
                switch (elapsed.signalPhase)
                {
                    case "R":
                        signalLight = SignalLight.R; break;
                    case "YY":
                        signalLight = SignalLight.YY; break;
                    case "Y":
                        signalLight = SignalLight.Y; break;
                    case "YG":
                        signalLight = SignalLight.YG; break;
                    case "G":
                        signalLight = SignalLight.G; break;
                    case "SwitchG":
                        signalLight = SignalLight.SwitchG; break;
                    default:
                        signalLight = SignalLight.N; break;

                }
                TrainState.RouteDatabase?.SetSignalLight(elapsed.signalName, signalLight);
            }
            catch (TimeoutException ex)
            {
                throw new SocketTimeOutException(3, "SignalSocket.cs@Elapse()", ex);
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@Elapse()", ex);
            }
            if (TrainState.TrainDiaName == null || TrainState.TrainDiaName == "" || TrainState.NextTrack == null)
            {
                Debug.WriteLine("return");
                return;
            }
            try
            {
                if (TrainState.NextNextTrack == null)
                {
                    return;
                }
                //こっからフレーム処理
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
                var elapsed = await tcs2.Task.WaitAsync(TimeSpan.FromSeconds(1));
                // 念の為明示的にイベントを解除
                client.Off("elapsed");
                // ここでサーバーから来た結果に応じた処理を書く
                Debug.WriteLine($"{elapsed.signalName}/{elapsed.signalPhase}/{elapsed.signalType}");
                SignalLight signalLight;
                switch (elapsed.signalPhase)
                {
                    case "R":
                        signalLight = SignalLight.R; break;
                    case "YY":
                        signalLight = SignalLight.YY; break;
                    case "Y":
                        signalLight = SignalLight.Y; break;
                    case "YG":
                        signalLight = SignalLight.YG; break;
                    case "G":
                        signalLight = SignalLight.G; break;
                    default:
                        signalLight = SignalLight.N; break;

                }
                TrainState.RouteDatabase?.SetSignalLight(elapsed.signalName, signalLight);
            }
            catch (TimeoutException ex)
            {
                throw new SocketTimeOutException(3, "SignalSocket.cs@Elapse()", ex);
            }
            catch (Exception ex)
            {
                throw new SocketException(3, "SignalSocket.cs@Elapse()", ex);
            }
        }

        private Dictionary<string, string> RetsubanTable = new Dictionary<string, string> {
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
        public string signalPhase { get; set; }
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
