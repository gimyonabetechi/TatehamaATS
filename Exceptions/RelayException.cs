using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TatehamaATS.Exceptions
{
    /// <summary>
    /// CF:継電部未定義故障
    /// </summary>
    internal class RelayException : ATSCommonException
    {
        /// <summary>
        /// CF:継電部未定義故障
        /// </summary>
        public RelayException(int place) : base(place)
        {
        }
        /// <summary>
        /// CF:継電部未定義故障
        /// </summary>
        public RelayException(int place, string message)
            : base(place, message)
        {
        }
        /// <summary>
        /// CF:継電部未定義故障
        /// </summary>
        public RelayException(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "CF";
        }
    }
}