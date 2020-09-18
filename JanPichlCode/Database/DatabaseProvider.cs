//-----------------------------------------------------------------------
// <copyright file="DatabaseProvider.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Database
{
    using JanPichlCode.Model;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Databázová vrstva aplikace
    /// 
    /// </summary>
    public class DatabaseProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DatabaseProvider));

        /// <summary>
        /// Načtení informací o stavu technologie z databáze
        /// 
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns>Načtené aktuální informace o výrobě.</returns>
        public async Task<FurnaceResult> ReadCurrentFurnacesState(CancellationToken cancelToken)
        {
            Dictionary<int, FurnaceStateInfo> furnaceInfosByFurnaceId = new Dictionary<int, FurnaceStateInfo>();
            Dictionary<int, CastingStateInfo> castingInfosByLineId = new Dictionary<int, CastingStateInfo>();
            List<FurnaceStateInfo> orderedFurnaceInfos = new List<FurnaceStateInfo>();

            FurnaceResult result;

            // TODO:  Dodělat Databázovou vestvu

            try
            {
                for (int x = 0; x < 16; x++)
                {
                    FurnaceStateInfo stateInfo = new FurnaceStateInfo(x, x, x * 10, x * 5, false, DateTime.Now);
                    orderedFurnaceInfos.Add(stateInfo);
                }

                await Task.CompletedTask;

                result = new FurnaceResult(furnaceInfosByFurnaceId, orderedFurnaceInfos, castingInfosByLineId);

            }
            catch (Exception ex)
            {
                _log.Error(nameof(ReadCurrentFurnacesState), ex);

                result = new FurnaceResult(null, null, null, false, ex.Message);
            }


            return result;


        }

    }
}
