using System.Windows.Forms;

namespace TatehamaATS.Exceptions
{
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
    }
}
