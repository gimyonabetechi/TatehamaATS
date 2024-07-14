namespace TatehamaATS.Exceptions
{
    internal class OnCarDBDataAbnormal : ATSCommonException
    {
        public OnCarDBDataAbnormal(int place) : base(place)
        {
        }
        public OnCarDBDataAbnormal(int place, string message)
            : base(place, message)
        {
        }
        public OnCarDBDataAbnormal(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "8E";
        }
    }
}