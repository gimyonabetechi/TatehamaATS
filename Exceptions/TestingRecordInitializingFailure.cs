namespace TatehamaATS.Exceptions
{
    internal class TestingRecordInitializingFailure : ATSCommonException
    {
        public TestingRecordInitializingFailure(int place) : base(place)
        {
        }
        public TestingRecordInitializingFailure(int place, string message)
            : base(place, message)
        {
        }
        public TestingRecordInitializingFailure(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "80";
        }
    }
}