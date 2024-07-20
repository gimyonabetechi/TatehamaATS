namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// 91:編成両数異常
    /// </summary>
    internal class CarAbnormal : ATSCommonException
    {
        /// <summary>
        /// 91:編成両数異常
        /// </summary>
        public CarAbnormal(int place) : base(place)
        {
        }
        /// <summary>
        /// 91:編成両数異常
        /// </summary>
        public CarAbnormal(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// 91:編成両数異常
        /// </summary>
        public CarAbnormal(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "91";
        }
    }
}
