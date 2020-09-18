//-----------------------------------------------------------------------
// <copyright file="FurnaceResult.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Kontejner pro načtená data z Db
    /// 
    /// </summary>
    public class FurnaceResult
    {
        #region Properties

        /// <summary>
        /// Pece
        /// 
        /// </summary>
        public Dictionary<int, FurnaceStateInfo> FurnaceInfosByFurnaceId
        {
            get;
            private set;
        }

        /// <summary>
        /// Stroje
        /// 
        /// </summary>
        public Dictionary<int, CastingStateInfo> CastingInfosByLineId
        {
            get;
            private set;
        }

        /// <summary>
        /// Seřazená kolekce pecí
        /// 
        /// </summary>
        public List<FurnaceStateInfo> OrderedFurnaceInfos
        {
            get;
            private set;
        }

        /// <summary>
        /// Příznak, zda jsou načtena data
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (OrderedFurnaceInfos == null || OrderedFurnaceInfos.Count == 0);
            }
        }

        /// <summary>
        /// Příznak zda je výsledek platný
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get;
            private set;
        } = true;

        /// <summary>
        /// Chybová správa
        /// 
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Konstruktor pro validní data
        /// 
        /// </summary>
        /// <param name="furnaceInfosByFurnaceId"></param>
        /// <param name="orderedFurnaceInfos"></param>
        /// <param name="castingInfosByLineId"></param>
        public FurnaceResult(Dictionary<int, FurnaceStateInfo> furnaceInfosByFurnaceId, List<FurnaceStateInfo> orderedFurnaceInfos, Dictionary<int, CastingStateInfo> castingInfosByLineId)
        {
            FurnaceInfosByFurnaceId = furnaceInfosByFurnaceId;
            OrderedFurnaceInfos = orderedFurnaceInfos;
            CastingInfosByLineId = castingInfosByLineId;
        }

        /// <summary>
        /// Konstruktor pro případ nenačtení dat
        /// 
        /// </summary>
        /// <param name="furnaceInfosByFurnaceId"></param>
        /// <param name="orderedFurnaceInfos"></param>
        /// <param name="castingInfosByLineId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="errorMessage"></param>
        public FurnaceResult(Dictionary<int, FurnaceStateInfo> furnaceInfosByFurnaceId,  List<FurnaceStateInfo> orderedFurnaceInfos, Dictionary<int, CastingStateInfo> castingInfosByLineId, bool isSuccess, string errorMessage)
            : this(furnaceInfosByFurnaceId, orderedFurnaceInfos, castingInfosByLineId)
        {
            IsSuccess = isSuccess;
            Message = errorMessage;
        }

        #endregion
    }
}