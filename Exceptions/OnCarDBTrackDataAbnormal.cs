namespace TatehamaATS.Exceptions
{
    internal class OnCarDBTrackDataAbnormal : ATSCommonException
    {
        public OnCarDBTrackDataAbnormal(int place) : base(place)
        {
        }
        public OnCarDBTrackDataAbnormal(int place, string message)
            : base(place, message)
        {
        }
        public OnCarDBTrackDataAbnormal(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "8D";
        }
    }
}