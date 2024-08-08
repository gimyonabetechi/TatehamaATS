using System;
using System.Diagnostics;
using TatehamaATS.Exceptions;

namespace TatehamaATS
{
    /// <summary>
    /// 検査記録部
    /// </summary>
    internal class InspectionRecord
    {
        /// <summary>
        /// 故障情報マスター
        /// </summary>
        internal static Dictionary<string, ATSCommonException> exceptions = new Dictionary<string, ATSCommonException>();
        /// <summary>
        /// 故障時間
        /// </summary>
        internal static Dictionary<string, DateTime> exceptionsTime = new Dictionary<string, DateTime>();

        /// <summary>
        /// 故障発生から故障復帰までの最低時間
        /// </summary>
        private static TimeSpan resetTime = TimeSpan.FromSeconds(3);

        public bool RetsubanReset;
        private bool StopDetection;
        private bool MasconEB;
        public bool ATSReset;
        private bool PowerReset;

        /// <summary>
        /// 検査記録部
        /// </summary>
        internal InspectionRecord()
        {
            exceptions.Clear();
            exceptionsTime.Clear();
            RetsubanReset = false;
            StopDetection = false;
            MasconEB = false;
            ATSReset = false;
            PowerReset = false;
            Task.Run(() => StartDisplayUpdateLoop());
        }

        /// <summary>
        /// 非同期で表示器を更新するループを開始する
        /// </summary>
        private async void StartDisplayUpdateLoop()
        {
            while (true)
            {
                var timer = Task.Delay(10);
                try
                {
                    ResetException();
                    if (exceptions.Any(x => x.Value.ToBrake() == OutputBrake.EB))
                    {
                        Relay.EB = true;
                    }
                    else
                    {
                        Relay.EB = false;
                    }
                }
                catch (ATSCommonException ex)
                {
                    MainWindow.inspectionRecord.AddException(ex);
                }
                catch (Exception ex)
                {
                    var e = new LEDControlException(3, "", ex);
                    MainWindow.inspectionRecord.AddException(e);
                }
                await timer;
            }
        }


        /// <summary>
        /// 故障追加
        /// </summary>
        /// <param name="exception"></param>
        internal void AddException(ATSCommonException exception)
        {
            TrainState.ATSBroken = true;
            Debug.WriteLine($"故障");
            Debug.WriteLine($"{exception.Message} {exception.InnerException}");
            exceptions[exception.ToCode()] = exception;
            exceptionsTime[exception.ToCode()] = DateTime.Now;
            //exceptions.Add(exception.ToCode(), exception);
            //exceptionsTime.Add(exception.ToCode(), DateTime.Now);
        }

        /// <summary>
        /// 故障追加
        /// </summary>
        /// <param name="exception"></param>
        internal void AddException(Exception exception)
        {
            exceptions.Add("39F", new CsharpException(3, exception.ToString()));
            exceptionsTime.Add("39F", DateTime.Now);
        }

        /// <summary>
        /// リセット条件達成確認
        /// </summary>
        internal void ResetException()
        {
            StopDetection = TrainState.TrainSpeed == 0;
            MasconEB = TrainState.TrainBnotch == 8;
            foreach (var ex in exceptions)
            {
                string key = ex.Key;
                TimeSpan time = DateTime.Now - exceptionsTime[key];

                if (time < resetTime)
                {
                    //故障復帰最低時間を下回っている場合無視
                    continue;
                }
                switch (ex.Value.ResetCondition())
                {
                    case ResetConditions.ExceptionReset:
                        exceptions.Remove(key);
                        break;
                    case ResetConditions.RetsubanReset:
                        if (RetsubanReset) exceptions.Remove(key);
                        break;
                    case ResetConditions.StopDetection:
                        if (StopDetection) exceptions.Remove(key);
                        break;
                    case ResetConditions.StopDetection_MasconEB:
                        if (StopDetection && MasconEB) exceptions.Remove(key);
                        break;
                    case ResetConditions.StopDetection_MasconEB_ATSReset:
                        if (StopDetection && MasconEB && ATSReset) exceptions.Remove(key);
                        break;
                }
            }
            RetsubanReset = false;
            ATSReset = false;

            if (MainWindow.controlLED != null)
            {
                MainWindow.controlLED.ExceptionCodes = exceptions.Values.Select(e => e.ToCode()).ToList();
            }
        }
    }
}
