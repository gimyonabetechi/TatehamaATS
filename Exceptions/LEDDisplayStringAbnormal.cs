namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// B1:LED表示内容異常
    /// </summary>
    internal class LEDDisplayStringAbnormal : ATSCommonException
    {
        /// <summary>
        /// B1:LED表示内容異常
        /// </summary>
        public LEDDisplayStringAbnormal(int place) : base(place)
        {
        }
        /// <summary>
        /// B1:LED表示内容異常
        /// </summary>
        public LEDDisplayStringAbnormal(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// B1:LED表示内容異常
        /// </summary>
        public LEDDisplayStringAbnormal(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "B1";
        }
    }
}

