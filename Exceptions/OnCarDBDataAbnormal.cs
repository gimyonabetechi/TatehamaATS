namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// 9E:車上DBデータ異常
    /// </summary>
    internal class OnCarDBDataAbnormal : ATSCommonException
    {
        /// <summary>
        /// 9E:車上DBデータ異常
        /// </summary>
        public OnCarDBDataAbnormal(int place) : base(place)
        {
        }
        /// <summary>
        /// 9E:車上DBデータ異常
        /// </summary>
        public OnCarDBDataAbnormal(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// 9E:車上DBデータ異常
        /// </summary>
        public OnCarDBDataAbnormal(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "9E";
        }
        public override ResetConditions ResetCondition()
        {
            return ResetConditions.RetsubanReset;
        }
        public override OutputBrake ToBrake()
        {
            return OutputBrake.EB;
        }
    }
}