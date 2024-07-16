namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// BF:LED制御部未定義故障
    /// </summary>
    internal class LEDControlException : ATSCommonException
    {
        /// <summary>
        /// BF:LED制御部未定義故障
        /// </summary>
        public LEDControlException(int place) : base(place)
        {
        }
        /// <summary>
        /// BF:LED制御部未定義故障
        /// </summary>
        public LEDControlException(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// BF:LED制御部未定義故障
        /// </summary>
        public LEDControlException(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "80";
        }
    }
}