using TatehamaATS.Exceptions;

namespace TatehamaATS.Database
{
    public enum SignalLight
    {
        /// <summary> 無現示・情報なし</summary>
        N,
        /// <summary> 停止現示 </summary>
        R,
        /// <summary> 警戒現示 </summary>
        YY,
        /// <summary> 注意現示 </summary>
        Y,
        /// <summary> 減速現示 </summary>
        YG,
        /// <summary> 進行現示 </summary>
        G,
        /// <summary> 入換進行現示 </summary>
        SwitchG
    }


    public enum SignalType
    {
        /// <summary> 本　線・二現示・R/Y </summary>
        Main_2Y,
        /// <summary> 本　線・三現示・R/Y/G </summary>
        Main_3,
        /// <summary> 本線細・三現示・R/Y/G </summary>
        Main_3nb,
        /// <summary> 本　線・四現示・R/Y/YG/G </summary>
        Main_4yg,
        /// <summary> 本線細・四現示・R/Y/YG/G </summary>
        Main_4ygnb,
        /// <summary> 中継細・四現示・R/Y/YG/G </summary>
        Relay_4ygnb,
        /// <summary> 本　線・四現示・R/YY/Y/G </summary>
        Main_4yy,
        /// <summary> 本　線・四現示・R/YY/Y・進行表示不可 </summary>
        Main_4yyng,
        /// <summary> 本線細・四現示・R/YY/Y・進行表示不可 </summary>
        Main_4yyngnb,
        /// <summary> 中継細・四現示・R/YY/Y・進行表示不可 </summary>
        Relay_4yyngnb,
        /// <summary> 本　線・五現示・R/YY/Y/YG/G </summary>
        Main_5,
        /// <summary> 進路予・三現示・L/N/R </summary>       
        Pre_3,
        /// <summary> 中　継・三現示・R/YY-YG/G </summary>       
        Relay_3,
        /// <summary> 入換信・二現示・R/G </summary>       
        Switch_2,
        /// <summary> 誘　導・二現示・R/G </summary>       
        Yudo_2
    }


    public class TrackCircuitInfo
    {
        public string Name { get; }
        public double StartMeter { get; private set; }
        public double EndMeter { get; private set; }
        public SignalLight Signal { get; set; }
        public SignalType SignalType { get; private set; }

        public TrackCircuitInfo(string name, double startMeter, double endMeter, SignalLight signal, SignalType signalType)
        {
            try
            {
                Name = name;
                StartMeter = startMeter;
                EndMeter = endMeter;
                Signal = signal;
                SignalType = signalType;
            }
            catch (Exception ex)
            {
                Name = "";
                new OnCarDBTrackDataAbnormal(3, "TrackCircuitInfo.cs@TrackCircuitInfo()", ex);
            }
            SignalType = signalType;
        }

        public override string ToString()
        {
            var SignalText = "　";
            switch (Signal)
            {
                case SignalLight.N:
                    SignalText = "無";
                    break;
                case SignalLight.R:
                    SignalText = "Ｒ";
                    break;
                case SignalLight.YY:
                    SignalText = "YY";
                    break;
                case SignalLight.Y:
                    SignalText = "Ｙ";
                    break;
                case SignalLight.YG:
                    SignalText = "YG";
                    break;
                case SignalLight.G:
                    SignalText = "Ｇ";
                    break;
                case SignalLight.SwitchG:
                    SignalText = "換";
                    break;
            }
            return $"現示：{SignalText}／{StartMeter.ToString("000000;-00000")}→{EndMeter.ToString("000000;-00000")}／{SignalType}／{Name}";
        }
    }
}