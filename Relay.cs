using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TatehamaATS.Exceptions;
using TrainCrew;

namespace TatehamaATS
{
    /// <summary>
    /// 継電部
    /// </summary>
    internal class Relay
    {
        internal Relay()
        {
            try
            {
                TrainCrewInput.Init();
            }
            catch (Exception ex)
            {
                new RelayInitialzingFailure(3, "Relay.cs@Relay()", ex);
            }
        }
        internal void Elapse()
        {
            try
            {
                TrainState.gameScreen = TrainCrewInput.gameState.gameScreen;
                TrainState.TrainSpeed = TrainCrewInput.GetTrainState().Speed;

            }
            catch (Exception ex)
            {
                new RelayException(3, "Relay.cs@Elapse()", ex);
            }
        }
    }
}
