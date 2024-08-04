using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TatehamaATS.Exceptions;

namespace TatehamaATS.Database
{
    public class RouteDatabase
    {
        public List<TrackCircuitInfo> CircuitList { get; set; }

        public RouteDatabase()
        {
            try
            {
                CircuitList = new List<TrackCircuitInfo>();
            }
            catch (Exception ex)
            {
                new OnCarDBTrackDataAbnormal(3, "RouteDatabase.cs@RouteDatabase()", ex);
            }
        }

        public void AddTrack(TrackCircuitInfo trackCircuitInfo)
        {
            if (CircuitList.Count == 1)
            {
                if (SignalNameTable.ContainsKey(trackCircuitInfo.Name))
                {
                    CircuitList[0].ChengeName(SignalNameTable[trackCircuitInfo.Name]);
                }
            }
            CircuitList.Add(trackCircuitInfo);
        }

        public void SetSignalLight(string SignalName, SignalLight signal)
        {
            try
            {
                var info = CircuitList.Find(x => x.Name == SignalName);
                if (info != null)
                {
                    info.Signal = signal;
                }
            }
            catch (Exception ex)
            {
                throw new DBTrackDataChengeAbnormal(3, "RouteDatabase.cs@SetSignalLight()");
            }
        }

        public override string ToString()
        {
            string Text = "軌道回路情報：\n";
            int i = 0;
            foreach (var c in CircuitList)
            {
                Text += $"　{i:000}{c}\n";
                i++;
            }
            return Text;
        }

        private Dictionary<string, string> SignalNameTable = new Dictionary<string, string> {
            { "館浜上り出発1R", "館浜下り場内1LA" },
            { "館浜上り出発2R", "館浜下り場内1LB" },
            { "館浜上り出発3R", "館浜下り場内1LC" },
            { "館浜上り出発4R", "館浜下り場内1LD" },

            { "駒野下り出発4L", "駒野上り場内1RB" },

            { "新野崎入換101L", "新野崎入換111R" },

            { "浜園入換101L", "浜園入換102R" },

            { "江ノ原検車区下り出発2L", "江ノ原検車区上り場内1RB" },
            { "江ノ原検車区下り出発3L", "江ノ原検車区上り場内1RC" },

            { "大道寺下り出発1L", "大道寺入換110LC" }  ,
            { "大道寺下り出発2L", "大道寺入換110LD" }  ,
            { "大道寺入換112L", "大道寺入換110R" }
        };
    }
}
