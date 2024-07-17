using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// A8:車両側ATS情報異常
    /// </summary>
    internal class TCSideATSDataAbnormalException : ATSCommonException
    {
        /// <summary>
        /// A8:車両側ATS情報異常
        /// </summary>
        public TCSideATSDataAbnormalException(int place) : base(place)
        {
        }
        /// <summary>
        /// A8:車両側ATS情報異常
        /// </summary>
        public TCSideATSDataAbnormalException(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// A8:車両側ATS情報異常
        /// </summary>
        public TCSideATSDataAbnormalException(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "80";
        }
    }
}