//-----------------------------------------------------------------------
// <copyright file="FurnaceBoardViewModel.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.ViewModel
{
    using JanPichlCode.Configuration;
    using JanPichlCode.Database;
    using JanPichlCode.Model;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Threading;

    /// <summary>
    /// VievModel pro tabuli.
    /// 
    /// </summary>
    public class FurnaceBoardViewModel : BaseViewModel, IDisposable
    {
        #region Properties

        private static readonly ILog _log = LogManager.GetLogger(typeof(FurnaceBoardViewModel));

        private readonly FurnaceBoardSection _section;

        private ObservableCollection<FurnaceViewModel> _furnaces;

        /// <summary>
        /// Kolekce pracovišt.
        /// 
        /// </summary>
        public ObservableCollection<FurnaceViewModel> Furnaces
        {
            get => _furnaces;
            private set => SetPropertyValue(ref _furnaces, value);
        }

        /// <summary>
        /// Mapování pracovišt podle id
        /// 
        /// </summary>
        private readonly Dictionary<int, FurnaceViewModel> _furnaceMapById;

        private DispatcherTimer _refreshDataTimer;

        private readonly DatabaseProvider _database;

        private readonly int _checkIntervalInSeconds;

        private bool _timerInitialized = false;

        public string Version { get; private set; }


        #endregion

        #region Constructors

        /// <summary>
        /// Konstruktor pro potřeby designTime
        /// 
        /// </summary>
        public FurnaceBoardViewModel()
        {
            _checkIntervalInSeconds = 60;

            Furnaces = new ObservableCollection<FurnaceViewModel>();

            _furnaceMapById = new Dictionary<int, FurnaceViewModel>();

            Furnaces = new ObservableCollection<FurnaceViewModel>()
            {
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
                new FurnaceViewModel(),
            };
        }

        /// <summary>
        /// Zjištění verze aplikace
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetProductVersion()
        {
            var attribute = (AssemblyFileVersionAttribute)Assembly
              .GetExecutingAssembly()
              .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true)
              .Single();

            var ver = attribute.Version;

            return "v" + ver.Substring(0, ver.Length - 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkIntervalInSeconds">Interval obnovování tabule.</param>
        /// <param name="section">Konfigurace.</param>
        public FurnaceBoardViewModel(int checkIntervalInSeconds, FurnaceBoardSection section)
        {
            _section = section;

            Version = GetProductVersion();

            _database = new DatabaseProvider();

            _checkIntervalInSeconds = checkIntervalInSeconds;

            _refreshDataTimer = new DispatcherTimer
            {
                // prvni nacteni dat bude hned za vterinu
                Interval = TimeSpan.FromSeconds(1)
            };

            _refreshDataTimer.Tick += RefreshDataTimer_Tick;
            _refreshDataTimer.Start();

            Furnaces = new ObservableCollection<FurnaceViewModel>();
            _furnaceMapById = new Dictionary<int, FurnaceViewModel>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RefreshDataTimer_Tick(object sender, EventArgs e)
        {
            if (!_timerInitialized)
            {
                _refreshDataTimer.Stop();
                _refreshDataTimer.Interval = TimeSpan.FromSeconds(_checkIntervalInSeconds);
                _refreshDataTimer.Start();
                _timerInitialized = true;
            }

            var result = await _database.ReadCurrentFurnacesState(CancellationToken.None);

            if (result.IsSuccess)
            {
                MergeResults(result);
            }
        }

        /// <summary>
        /// Aktualizace pracovišt z nově načtených
        /// 
        /// provede přidání/odebrání z kolekce a nakonec aktualizace každé
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void MergeResults(FurnaceResult result)
        {
            try
            {
                if (!result.IsEmpty)
                {
                    // najdi pracoviště k odstranění
                    List<int> toBeRemoved = new List<int>();
                    foreach (KeyValuePair<int, FurnaceViewModel> pair in _furnaceMapById)
                    {
                        if (!result.FurnaceInfosByFurnaceId.ContainsKey(pair.Key))
                        {
                            toBeRemoved.Add(pair.Key);
                        }
                    }

                    // odstraň pracoviště co přestali existovat
                    foreach (int id in toBeRemoved)
                    {
                        FurnaceViewModel vm = _furnaceMapById[id];
                        _furnaceMapById.Remove(id);

                        _log.Debug(string.Format("odebiram ze sledovani pec c: '{0}'", id));
                        Furnaces.Remove(vm);

                    }

                    // přidej nová pracoviště
                    foreach (FurnaceStateInfo info in result.OrderedFurnaceInfos)
                    {
                        if (!_furnaceMapById.ContainsKey(info.IdFurnace))
                        {
                            FurnaceViewModel vm = new FurnaceViewModel(info.IdFurnace.ToString(), _section);
                            _furnaceMapById.Add(info.IdFurnace, vm);

                            _log.Debug(string.Format("pridavam do sledovani pec c: '{0}'", vm.FurnaceNumberStr));
                            Furnaces.Add(vm);
                        }
                    }

                    // aktualizuj stávající pracoviště
                    foreach (FurnaceStateInfo furnaceInfo in result.OrderedFurnaceInfos)
                    {
                        CastingStateInfo castingInfo = null;

                        if (result.CastingInfosByLineId.ContainsKey(furnaceInfo.IdLine))
                        {
                            castingInfo = result.CastingInfosByLineId[furnaceInfo.IdLine];
                        }

                        FurnaceViewModel vm = _furnaceMapById[furnaceInfo.IdFurnace];
                        UpdateFurnaceViewModel(ref vm, furnaceInfo, castingInfo);
                    }
                }
                else
                {
                    _log.Debug("Zadne pece nejsou nenacteny");
                    Furnaces = new ObservableCollection<FurnaceViewModel>();
                }
            }
            catch (Exception ex)
            {
                _log.Error(nameof(MergeResults), ex);
            }
        }


        /// <summary>
        /// Aktualizace konkrétního pracoviště.
        /// 
        /// </summary>
        /// <param name="furnaceViewModel"></param>
        /// <param name="furnaceInfo"></param>
        /// <param name="castingInfo"></param>
        private void UpdateFurnaceViewModel(ref FurnaceViewModel furnaceViewModel, FurnaceStateInfo furnaceInfo, CastingStateInfo castingInfo)
        {
            try
            {
                if (furnaceInfo.MaterialPercentage != null)
                {
                    furnaceViewModel.FillPercentage = furnaceInfo.MaterialPercentage.Value;
                }

                if (furnaceInfo.IsFillingRequired != null)
                {
                    furnaceViewModel.IsMaterialRequired = furnaceInfo.IsFillingRequired.Value;
                }

                if (furnaceInfo.MaterialCode != null)
                {
                    furnaceViewModel.MaterialCode = furnaceInfo.MaterialCode.TrimStart().TrimEnd();
                    furnaceViewModel.Material = GetShortMaterialCode(furnaceInfo.MaterialCode);
                }
                else
                {
                    furnaceViewModel.MaterialCode = null;
                    furnaceViewModel.Material = null;
                }

                if (furnaceInfo.Temperature != null)
                {
                    furnaceViewModel.Temperature = furnaceInfo.Temperature.Value;
                }

                furnaceViewModel.EventErrorCode = furnaceInfo.EventErrorCode;

                furnaceViewModel.EventErrorCodeGroup = furnaceInfo.EventErrorGroup;

                furnaceViewModel.LastUpdate = furnaceInfo.LastUpdate;

                furnaceViewModel.MachineFailure = castingInfo?.MachineFailure;

                furnaceViewModel.DesiredTemperature = castingInfo?.DesiredTemperature;
            }
            catch (Exception ex)
            {
                _log.Error(nameof(UpdateFurnaceViewModel), ex);
            }
        }

        /// <summary>
        /// Vyřízne z kódu identifikátor
        /// 
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        private string GetShortMaterialCode(string materialCode)
        {
            return (materialCode.Trim()).Substring(3, 3);
        }

        #endregion

        #region IDisposable Members

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_refreshDataTimer != null)
                {
                    try
                    {
                        _refreshDataTimer.Stop();
                    }
                    catch { }
                    _refreshDataTimer = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~FurnaceBoardViewModel()
        {
            Dispose(false);
        }

        #endregion
    }
}
