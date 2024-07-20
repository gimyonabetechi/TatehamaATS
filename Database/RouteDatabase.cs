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
        public List<TrackCircuitInfo> CircuitList { get; private set; }

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
            foreach (var c in CircuitList)
            {
                Text += $"　{c}\n";
            }
            return Text;
        }
    }
}
