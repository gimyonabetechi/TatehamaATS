using TatehamaATS.Database;
using TrainCrew;

namespace TatehamaATS
{
    static internal class TrainState
    {
        /// <summary>
        /// 現在ゲーム状態
        /// </summary>
        static public GameScreen gameScreen;
        /// <summary>
        /// 自車速度
        /// </summary>
        static public float? TrainSpeed;
        /// <summary>
        /// 自車手動P段
        /// </summary>
        static public int TrainPnotch;
        /// <summary>
        /// 自車手動B段
        /// </summary>
        static public int TrainBnotch;
        /// <summary>
        /// 自車車種名
        /// </summary>
        static public string? TrainName;
        /// <summary>
        /// 現在時刻
        /// </summary>
        static public TimeSpan NowTime;
        /// <summary>
        /// 現在閉塞情報
        /// </summary>
        static public TrackCircuitInfo? OnTrack;
        /// <summary>
        /// 次閉塞情報
        /// </summary>
        static public TrackCircuitInfo? NextTrack;
        /// <summary>
        /// 路線データベース
        /// </summary>
        static public RouteDatabase? RouteDatabase;
        /// <summary>
        /// 本体表示器
        /// </summary>
        static public ATSDisplay? TC_ATSDisplay;
        /// <summary>
        /// 合計表示器
        /// </summary>
        static public ATSDisplay? ATSDisplay;
        /// <summary>
        /// LED試験
        /// </summary>
        static public bool ATSLEDTest;
        /// <summary>
        /// ATS故障
        /// </summary>
        static public bool ATSBroken;

        /// <summary>
        /// 完全初期化
        /// </summary>
        static public void init()
        {
            TrainSpeed = 0f;
            TrainName = null;
            OnTrack = null;
            NextTrack = null;
            RouteDatabase = null;
            TC_ATSDisplay = null;
            ATSDisplay = new ATSDisplay("", "", [""]);
            ATSBroken = false;
        }
    }
}