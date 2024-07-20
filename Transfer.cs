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

namespace TatehamaATS
{
    /// <summary>
    /// 伝送部：APIとの通信を管理するクラス。
    /// </summary>
    public class Transfer
    {
        private HttpClient _client;
        private TaskCompletionSource _tcs = new();

        /// <summary>
        /// Transfer クラスのインスタンスを初期化する。
        /// </summary>
        public Transfer()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:58680/tanuden-api")
            };
            Task.Run(async () =>
            {
                await PostPIData();
                _tcs.SetResult();
            });
        }

        public async Task PostPIData()
        {
            // プラグインを登録
            var plugin = new
            {
                uid = "TAKUMITE_TRAINCREW_MULTI_ATS",
                name = "館浜M-ATS",
                version = "1.0.0",
                author = "Takumite Tudo",
                description = "運転会用マルチプラグイン"
            };
            string registerResponse = await RegisterPluginAsync(plugin);
            Console.WriteLine(registerResponse);
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
                // ここで例外をキャッチしてログなどに出力する     
                TatehamaATS.TrainState.ATSBroken = true;
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
                var e = new TransferException(3, "", ex);
                TrainState.ATSDisplay?.SetLED("", "");
                TrainState.ATSDisplay?.AddState(e.ToCode());
            }
        }

        /// <summary>
        /// API情報を取得するメソッド。
        /// </summary>
        /// <returns>API情報を含む文字列。</returns>
        public async Task<string> GetApiInfoAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/");
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
            HttpResponseMessage response = await _client.GetAsync("/plugins");
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
            HttpResponseMessage response = await _client.PostAsync("/plugins", content);
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
            var content = new StringContent(JsonSerializer.Serialize(pluginData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/plugins/data", content);
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
            var content = new StringContent(JsonSerializer.Serialize(pluginData), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("http://127.0.0.1:58680/tanuden-api/plugins/override_diagram", content);
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(responseBody);
            response.EnsureSuccessStatusCode();
            return responseBody;
        }
    }
}