namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// 9F:C#系異常
    /// </summary>
    internal class CsharpException : ATSCommonException
    {
        /// <summary>
        /// 9F:C#系異常
        /// </summary>
        public CsharpException(int place) : base(place)
        {
        }
        /// <summary>
        /// 9F:C#系異常
        /// </summary>
        public CsharpException(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// 9F:C#系異常
        /// </summary>
        public CsharpException(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "80";
        }
    }
}
