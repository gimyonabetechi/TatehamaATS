using System;
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
        internal static Dictionary<string, Exception> exceptions = new Dictionary<string, Exception>();
        /// <summary>
        /// 故障時間
        /// </summary>
        internal static Dictionary<string, DateTime> exceptionsTime = new Dictionary<string, DateTime>();

        /// <summary>
        /// 故障発生から故障復帰までの最低時間
        /// </summary>
        private static TimeSpan resetTime = TimeSpan.FromSeconds(1);

        private static bool ExceptionReset;
        private static bool StopDetection_MasconEB;
        private static bool PowerReset;

        /// <summary>
        /// 検査記録部
        /// </summary>
        internal InspectionRecord()
        {
            exceptions.Clear();
            exceptionsTime.Clear();
        }

        /// <summary>
        /// 故障追加
        /// </summary>
        /// <param name="exception"></param>
        internal void AddException(ATSCommonException exception)
        {
            exceptions.Add(exception.ToCode(), exception);
            exceptionsTime.Add(exception.ToCode(), DateTime.Now);
            
        }

        /// <summary>
        /// 故障追加
        /// </summary>
        /// <param name="exception"></param>
        internal void AddException(Exception exception)
        {
            exceptions.Add("39F", exception);
            exceptionsTime.Add("39F", DateTime.Now);
        }

        /// <summary>
        /// リセット条件達成時
        /// </summary>
        internal void ResetException(ResetConditions resetConditions)
        {
            if (ExceptionReset)
            {
                
            }
        }
    }
}
