using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.Cryptography.Xml;
using OpenTetsu.Commons;
using OpenTetsu.Commons.Train;
using System.Diagnostics;
using TatehamaATS.Exceptions;
using TatehamaATS.Database;

namespace TatehamaATS
{
    /// <summary>
    /// 伝送部：APIとの通信を管理するクラス。
    /// </summary>
    public class Transfer
    {
        private HttpClient _client;
        private TaskCompletionSource _tcs = new();
        private string Token;
        private bool isConnect;

        /// <summary>
        /// Transfer クラスのインスタンスを初期化する。
        /// </summary>
        public Transfer()
        {
            isConnect = false;
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:58680")
            };
            Task.Run(async () =>
            {
                await PostPIData();
                _tcs.SetResult();
            });
        }

        public async Task PostPIData()
        {
            while (!isConnect)
            {
                try
                {
                    // プラグインを登録
                    var plugin = new
                    {
                        uid = "TAKUMITE_TRAINCREW_MULTI_ATS",
                        name = "館浜M-ATS",
                        version = "0.4.0",
                        author = "Takumite Tudo",
                        description = "運転会用マルチプラグイン"
                    };
                    var registerResponse = await RegisterPluginAsync(plugin);
                    JsonDocument doc = JsonDocument.Parse(registerResponse);
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("token", out JsonElement tokenElement))
                    {
                        Token = tokenElement.GetString();
                        Debug.WriteLine($"Token: {Token}");
                        isConnect = true;
                    }
                    else
                    {
                        Debug.WriteLine("Token プロパティが見つかりませんでした。");
                        var e = new TransferInitialzingFailure(3, "Token不明");
                        MainWindow.inspectionRecord.AddException(e);
                        isConnect = false;
                    }
                }
                catch (Exception ex)
                {
                    TrainState.ATSBroken = true;
                    Debug.WriteLine($"故障");
                    Debug.WriteLine($"{ex.Message} {ex.InnerException}");
                    var e = new TransferInitialzingFailure(3, "", ex);
                    MainWindow.inspectionRecord.AddException(e);
                    isConnect = false;
                }
                await Task.Delay(1000);
            }
        }

        public async void SetRetsuban()
        {
            try
            {
                // プラグインのデータを送信
                var pluginData = new
                {
                    uid = "TAKUMITE_TRAINCREW_MULTI_ATS",
                    diagramNumber = TrainState.TrainDiaName
                };
                string dataResponse = await SendRetsubanDataAsync(pluginData);
            }
            catch (ATSCommonException ex)
            {
                MainWindow.inspectionRecord.AddException(ex);
            }
            catch (Exception ex)
            {
                var e = new TransferException(3, "", ex);
                MainWindow.inspectionRecord.AddException(e);
            }
        }

        /// <summary>
        /// API情報を取得するメソッド。
        /// </summary>
        /// <returns>API情報を含む文字列。</returns>
        public async Task<string> GetApiInfoAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/tanuden-api");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// プラグインリストを取得するメソッド。
        /// </summary>
        /// <returns>プラグインリストを含む文字列。</returns>
        public async Task<string> GetPluginsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/tanuden-api/plugins");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// プラグインを登録するメソッド。
        /// </summary>
        /// <param name="plugin">登録するプラグイン情報。</param>
        /// <returns>レスポンスを含む文字列。</returns>
        public async Task<string> RegisterPluginAsync(object plugin)
        {
            var content = new StringContent(JsonSerializer.Serialize(plugin), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/tanuden-api/plugins", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                return responseBody;
            }
            response.EnsureSuccessStatusCode();
            return responseBody;
        }

        /// <summary>
        /// プラグインのデータを送信するメソッド。
        /// </summary>
        /// <param name="pluginData">送信するプラグインデータ。</param>
        /// <returns>レスポンスを含む文字列。</returns>
        public async Task<string> SendDeleteAsync()
        {
            await _tcs.Task;
            // Authorizationヘッダーを追加
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent("");
            HttpResponseMessage response = await _client.DeleteAsync("/tanuden-api/plugins");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// プラグインのデータを送信するメソッド。
        /// </summary>
        /// <param name="pluginData">送信するプラグインデータ。</param>
        /// <returns>レスポンスを含む文字列。</returns>
        public async Task<string> SendPluginDataAsync(object pluginData)
        {
            await _tcs.Task;
            // Authorizationヘッダーを追加
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent(JsonSerializer.Serialize(pluginData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/tanuden-api/plugins/data", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// プラグインのデータを送信するメソッド。
        /// </summary>
        /// <param name="pluginData">送信するプラグインデータ。</param>
        /// <returns>レスポンスを含む文字列。</returns>
        public async Task<string> SendRetsubanDataAsync(object pluginData)
        {
            await _tcs.Task;
            // Authorizationヘッダーを追加
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent(JsonSerializer.Serialize(pluginData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/tanuden-api/plugins/override_diagram", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(responseBody);
            response.EnsureSuccessStatusCode();
            return responseBody;
        }


        class KeyData
        {
            public string token { get; set; }
        }
    }
}