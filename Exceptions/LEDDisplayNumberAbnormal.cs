namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// B0:LED表示番号異常
    /// </summary>
    internal class LEDDisplayNumberAbnormal : ATSCommonException
    {
        /// <summary>
        /// B0:LED表示番号異常
        /// </summary>
        public LEDDisplayNumberAbnormal(int place) : base(place)
        {
        }
        /// <summary>
        /// B0:LED表示番号異常
        /// </summary>
        public LEDDisplayNumberAbnormal(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// B0:LED表示番号異常
        /// </summary>
        public LEDDisplayNumberAbnormal(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "80";
        }
    }
}

