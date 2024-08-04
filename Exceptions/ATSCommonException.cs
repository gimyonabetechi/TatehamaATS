using System.Windows.Forms;

namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// 復旧条件の種類を表す列挙型。
    /// </summary>
    public enum ResetConditions
    {
        /// <summary>故障復旧のみで復旧</summary>
        ExceptionReset,
        /// <summary>停車検知・マスコン非常</summary>
        StopDetection_MasconEB,
        /// <summary>電源再投入</summary>
        PowerReset
    }

    /// <summary>
    /// FF:未定義故障
    /// </summary>
    internal class ATSCommonException : Exception
    {
        /// <summary>
        /// 系コード
        /// </summary>
        public int Place;

        /// <summary>
        /// FF:未定義故障
        /// </summary>
        public ATSCommonException(int place)
        {
            Place = place;
        }
        /// <summary>
        /// FF:未定義故障
        /// </summary>
        public ATSCommonException(int place, string message)
            : base(message)
        {
            Place = place;
        }
        /// <summary>
        /// FF:未定義故障
        /// </summary>
        public ATSCommonException(int place, string message, Exception inner)
            : base(message, inner)
        {
            Place = place;
        }
        public virtual string ToCode()
        {
            return Place.ToString() + "FF";
        }
        public virtual ResetConditions ResetCondition()
        {
            return ResetConditions.PowerReset;
        }
    }
}
