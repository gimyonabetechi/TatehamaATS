using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatehamaATS.Exceptions
{
    internal class RelayInitialzingFailure : ATSCommonException
    {
        public RelayInitialzingFailure(int place) : base(place)
        {
        }
        public RelayInitialzingFailure(int place, string message)
            : base(place, message)
        {
        }
        public RelayInitialzingFailure(int place, string message, Exception inner)
            : base(place, message, inner)
        {
        }
        public override string ToCode()
        {
            return Place.ToString() + "84";
        }
    }
}
