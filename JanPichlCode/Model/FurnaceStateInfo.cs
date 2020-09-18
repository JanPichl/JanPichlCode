//-----------------------------------------------------------------------
// <copyright file="FurnaceStateInfo.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Model
{
    using System;

    /// <summary>
    /// Stav dávkovací pece na taveninu
    /// 
    /// </summary>
    public class FurnaceStateInfo
    {
        #region Properties

        /// <summary>
        /// Id Pece.
        /// 
        /// </summary>
        public int IdFurnace
        {
            get;
            private set;
        }

        /// <summary>
        /// Id linky.
        /// 
        /// </summary>
        public int IdLine
        {
            get;
            private set;
        }

        /// <summary>
        /// Procento naplnění taveninou.
        /// 
        /// </summary>
        public int? MaterialPercentage
        {
            get;
            private set;
        }

        /// <summary>
        /// Aktuální teplota.
        /// 
        /// </summary>
        public double? Temperature
        {
            get;
            private set;
        }

        /// <summary>
        /// Požadavek na doplnění taveniny.
        /// 
        /// </summary>
        public bool? IsFillingRequired
        {
            get;
            private set;
        }

        /// <summary>
        /// Chybový kód.
        /// 
        /// </summary>
        public int? EventErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Skupina chybových kódů.
        /// 
        /// </summary>
        public int? EventErrorGroup
        {
            get;
            private set;
        }

        /// <summary>
        /// Kód používané slitiny
        /// 
        /// </summary>
        public string MaterialCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Poslední aktualizace
        /// 
        /// </summary>
        public DateTime LastUpdate
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idFurnace">Id Pece.</param>
        /// <param name="idLine">Id linky.</param>
        /// <param name="materialPercentage">MaterialPercentage</param>
        /// <param name="temperature">Aktuální teplota.</param>
        /// <param name="isFillingRequired">Požadavek na doplnění taveniny.</param>
        /// <param name="lastUpdate">Poslední aktualizace</param>
        public FurnaceStateInfo(int idFurnace, int idLine, int? materialPercentage, double? temperature, bool? isFillingRequired, DateTime lastUpdate)
        {
            IdFurnace = idFurnace;
            IdLine = idLine;
            MaterialPercentage = materialPercentage;
            Temperature = temperature;
            IsFillingRequired = isFillingRequired;
            LastUpdate = lastUpdate;
        }

        #endregion
    }
}
