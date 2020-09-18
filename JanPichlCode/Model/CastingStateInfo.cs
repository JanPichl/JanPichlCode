//-----------------------------------------------------------------------
// <copyright file="CastingStateInfo.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Model
{
    /// <summary>
    /// Stav licího stroje
    /// 
    /// </summary>
    public class CastingStateInfo
    {
        #region Properties

        /// <summary>
        /// Id stroje
        /// 
        /// </summary>
        public int IdMachine
        {
            get;
            private set;
        }

        /// <summary>
        /// Zda je na stroji porucha
        /// 
        /// </summary>
        public bool? MachineFailure
        {
            get;
            private set;
        }

        /// <summary>
        /// Požadovaná teplota
        /// 
        /// </summary>
        public double? DesiredTemperature
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idmachine">Id stroje.</param>
        /// <param name="machineFailure">Příznak poruchy.</param>
        /// <param name="desiredTemperature">Požadovaná teplota.</param>
        public CastingStateInfo(int idmachine, bool? machineFailure, double? desiredTemperature)
        {
            IdMachine = idmachine;
            MachineFailure = machineFailure;
            DesiredTemperature = desiredTemperature;
        }

        #endregion
    }
}
