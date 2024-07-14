using TatehamaATS.Exceptions;

namespace TatehamaATS.Database
{
    public enum Signal
    {
        /// <summary>
        /// 停止現示
        /// </summary>
        R,
        /// <summary>
        /// 警戒現示
        /// </summary>
        YY,
        /// <summary>
        /// 注意現示
        /// </summary>
        Y,
        /// <summary>
        /// 減速現示
        /// </summary>
        YG,
        /// <summary>
        /// 進行現示
        /// </summary>
        G,
        /// <summary>
        /// 入換進行現示
        /// </summary>
        SwitchG
    }
    internal class TrackCircuitInfo
    {
        internal string Name { get; }
        internal double? StartMeter { get; private set; }
        internal double? EndMeter { get; private set; }
        internal Signal Signal { get; private set; }

        internal TrackCircuitInfo(string name, double startMeter, double endMeter, Signal signal)
        {
            try
            {
                Name = name;
                StartMeter = startMeter;
                EndMeter = endMeter;
                Signal = signal;
            }
            catch (Exception ex)
            {
                new OnCarDBTrackDataAbnormal(3, "TrackCircuitInfo.cs@TrackCircuitInfo()", ex);
            }
        }

        public override string ToString()
        {
            var SignalText = "";
            switch (Signal)
            {
                case Signal.R:
                    SignalText = "Ｒ";
                    break;
                case Signal.YY:
                    SignalText = "YY";
                    break;
                case Signal.Y:
                    SignalText = "Ｙ";
                    break;
                case Signal.YG:
                    SignalText = "YG";
                    break;
                case Signal.G:
                    SignalText = "Ｇ";
                    break;
                case Signal.SwitchG:
                    SignalText = "換";
                    break;
            }
            return $"現示：{SignalText}／{StartMeter}-{EndMeter}／{Name}";
        }
    }
}