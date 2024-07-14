using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TatehamaATS.Exceptions
{
    internal class RelayException : ATSCommonException
    {
        public RelayException(int place) : base(place)
        {
        }
        public RelayException(int place, string message)
            : base(place, message)
        {
        }
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