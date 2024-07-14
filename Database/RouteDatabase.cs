using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TatehamaATS.Exceptions;

namespace TatehamaATS.Database
{
    internal class RouteDatabase
    {
        internal List<TrackCircuitInfo> CircuitList { get; private set; } = new List<TrackCircuitInfo>();

        internal RouteDatabase()
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
    }
}
