//-----------------------------------------------------------------------
// <copyright file="FurnaceViewModel.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.ViewModel
{
    using JanPichlCode.Configuration;
    using JanPichlCode.Model;
    using System;

    /// <summary>
    /// VievModel pro jednotlivou pec na tabuli.
    /// 
    /// </summary>
    public class FurnaceViewModel : BaseViewModel
    {
        #region Constants

        // Přeseřízení
        private const int _switchingErrorCode = 77;

        // čištění
        private const int _cleaningErrorCode = 63;

        // Upínání licí formy
        private const int _clampingErrorCode = 79;

        // Prodleva mezi upínáním licí formy a vzorkováním
        private const int _lagErrorCode = 80;

        // Vzorkování
        private const int _samplingErrorCode = 81;

        //Skupina atributů
        private const int _planedDownTimeGroup = 12;

        #endregion

        #region Properties

        private readonly FurnaceBoardSection _section;

        /// <summary>
        /// Horní odchylka teploty.
        /// 
        /// </summary>
        private readonly double _maxUpDesiredTemperatureOffset;

        /// <summary>
        /// Spodní odchylka teploty.
        /// 
        /// </summary>
        private readonly double _maxDownDesiredTemperatureOffset;

        /// <summary>
        /// Spodní limit teploty aktivity pece
        /// 
        /// </summary>
        private readonly double _downDesiredTemperatureLimit;

        /// <summary>
        /// Kód pracoviště
        /// 
        /// </summary>
        public string FurnaceNumberStr
        {
            get;
            private set;
        }

        private string _material = "266";

        /// <summary>
        /// Zkratka pro kód materiálu 
        /// 
        /// </summary>
        public string Material
        {
            get => _material;
            set => SetPropertyValue(ref _material, value);
        }

        private string _materialCode;

        /// <summary>
        /// Kód materiálu
        /// 
        /// </summary>
        public string MaterialCode
        {
            get => _materialCode;
            set => SetPropertyValue(ref _materialCode, value);
        }

        private double _temperature;

        /// <summary>
        /// Aktuální teplota.
        /// 
        /// </summary>
        public double Temperature
        {
            get => _temperature;
            set
            {
                if (SetPropertyValue(ref _temperature, value))
                {
                    OnPropertyChanged(nameof(Temperature));
                    OnPropertyChanged(nameof(TemperatureStr));
                    RecountTemperaturesState();
                }
            }
        }

        /// <summary>
        /// View formát požadované teploty.
        /// 
        /// </summary>
        public string TemperatureStr
        {
            get
            {
                int iTemperature = (int)Temperature;
                return string.Format("{0}°C", iTemperature);
            }
        }

        private double? _desiredTemperature;

        /// <summary>
        /// Požadovaná teplota
        /// 
        /// </summary>
        public double? DesiredTemperature
        {
            get
            {
                return _desiredTemperature;
            }
            set
            {
                if (SetPropertyValue(ref _desiredTemperature, value))
                {
                    OnPropertyChanged(nameof(DesiredTemperature));
                    OnPropertyChanged(nameof(DesiredTemperatureStr));
                    RecountTemperaturesState();
                }
            }
        }

        /// <summary>
        /// View formát požadované teploty.
        /// 
        /// </summary>
        public string DesiredTemperatureStr
        {
            get
            {
                if (DesiredTemperature.HasValue)
                {
                    int iTemperature = (int)(DesiredTemperature);
                    return string.Format("{0}°C", iTemperature);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private bool _isDesiredTemperatureNok = false;

        /// <summary>
        /// Příznak zda není požadovaná teplota v pořádku.
        /// 
        /// </summary>
        public bool IsDesiredTemperatureNok
        {
            get => _isDesiredTemperatureNok;
            set => SetPropertyValue(ref _isDesiredTemperatureNok, value);
        }

        private bool _isActualTemperatureNok = false;

        /// <summary>
        /// Příznak zda není aktuální teplota v pořádku.
        /// 
        /// </summary>
        public bool IsActualTemperatureNok
        {
            get => _isActualTemperatureNok;
            set => SetPropertyValue(ref _isActualTemperatureNok, value);
        }

        /// <summary>
        /// Přepočítání stavů Ok/Nok teplot.
        /// 
        /// </summary>
        private void RecountTemperaturesState()
        {
            /// <summary>
            /// (požadovaná teplota) pokud nastane odchylka mezi požadovanou teplotou a skutečnou teplotou větší jak 10°C 
            /// a to jak do plusu, tak do mínusu,rozblikají se naráz obě teploty ostrou svítivou barvou
            /// 
            /// </summary>
            if (DesiredTemperature.HasValue)
            {
                if ((Temperature > (DesiredTemperature + _maxUpDesiredTemperatureOffset)) || (Temperature < (DesiredTemperature - _maxDownDesiredTemperatureOffset)))
                {
                    IsDesiredTemperatureNok = true;
                }
                else
                {
                    IsDesiredTemperatureNok = false;
                }
            }
            else
            {
                IsDesiredTemperatureNok = false;
            }

            /// <summary>
            /// (skutečná teplota) pokud teplota klesne pod 660°C, okénko se rozbliká ostrou svítivou barvou, nejlépe žluto zelenou, interval blikání bude opačný než teď, 
            /// to znamená delší impuls svícení než nesvícení
            /// </summary>
            if ((Temperature < _downDesiredTemperatureLimit) || IsDesiredTemperatureNok == true)
            {
                IsActualTemperatureNok = true;
            }
            else
            {
                IsActualTemperatureNok = false;
            }
        }

        private int _fillPercentage;

        /// <summary>
        /// Procento naplnění pece
        /// 
        /// </summary>
        public int FillPercentage
        {
            get
            {
                return _fillPercentage;
            }
            set
            {
                if (SetPropertyValue(ref _fillPercentage, value))
                {
                    OnPropertyChanged(nameof(FillPercentage));
                    OnPropertyChanged(nameof(FillPercentageStr));
                    OnPropertyChanged(nameof(FillLevel));
                    OnPropertyChanged(nameof(IsFillLevel1Active));
                    OnPropertyChanged(nameof(IsFillLevel2Active));
                    OnPropertyChanged(nameof(IsFillLevel3Active));
                    OnPropertyChanged(nameof(IsFillLevel4Active));
                    OnPropertyChanged(nameof(IsFillLevel5Active));
                }
            }
        }

        /// <summary>
        /// View formát Procenta naplnění pece
        /// 
        /// </summary>
        public string FillPercentageStr
        {
            get
            {
                return string.Format("{0}%", FillPercentage);
            }
        }

        /// <summary>
        /// Naplnění hladiny
        /// 
        /// </summary>
        public int FillLevel
        {
            get
            {
                if (FillPercentage <= 11)
                {
                    return 0;
                }
                else if (FillPercentage <= 34)
                {
                    return 1;
                }
                else if (FillPercentage <= 58)
                {
                    return 2;
                }
                else if (FillPercentage <= 81)
                {
                    return 3;
                }
                else if (FillPercentage <= 94)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// Aktivní hladina 1
        /// 
        /// </summary>
        public bool IsFillLevel1Active
        {
            get => FillLevel >= 1;
        }

        /// <summary>
        /// Aktivní hladina 2
        /// 
        /// </summary>
        public bool IsFillLevel2Active
        {
            get => FillLevel >= 2;
        }

        /// <summary>
        /// Aktivní hladina 3
        /// 
        /// </summary>
        public bool IsFillLevel3Active
        {
            get => FillLevel >= 3;
        }

        /// <summary>
        /// Aktivní hladina 4
        /// 
        /// </summary>
        public bool IsFillLevel4Active
        {
            get => FillLevel >= 4;
        }

        /// <summary>
        /// Aktivní hladina 5
        /// 
        /// </summary>
        public bool IsFillLevel5Active
        {
            get => FillLevel >= 5;
        }

        private bool _isMaterialRequired;

        /// <summary>
        /// Požadavek na materiál
        /// 
        /// </summary>
        public bool IsMaterialRequired
        {
            get => _isMaterialRequired;
            set => SetPropertyValue(ref _isMaterialRequired, value);
        }

        private int? _eventErrorCode;

        /// <summary>
        /// Chybový kód
        /// 
        /// </summary>
        public int? EventErrorCode
        {
            get => _eventErrorCode;
            set
            {
                if (SetPropertyValue(ref _eventErrorCode, value))
                {
                    OnPropertyChanged(nameof(EventErrorCode));
                    OnPropertyChanged(nameof(EventStateType));
                }
            }
        }

        private int? _eventErrorCodeGroup;

        /// <summary>
        /// Skupina poruch
        /// 
        /// </summary>
        public int? EventErrorCodeGroup
        {
            get => _eventErrorCodeGroup;
            set
            {
                if (SetPropertyValue(ref _eventErrorCodeGroup, value))
                {
                    OnPropertyChanged(nameof(EventErrorCodeGroup));
                    OnPropertyChanged(nameof(EventStateType));
                }
            }
        }

        private bool? _machineFailure;

        /// <summary>
        /// Chyba stroje.
        /// 
        /// </summary>
        public bool? MachineFailure
        {
            get => _machineFailure;

            set
            {
                if (SetPropertyValue(ref _machineFailure, value))
                {
                    OnPropertyChanged(nameof(MachineFailure));
                    OnPropertyChanged(nameof(EventState));
                }
            }
        }


        /// <summary>
        /// Stav pracoviště
        /// 
        /// </summary>
        public EventStateType EventState
        {
            get
            {
                if (MachineFailure == true)
                {
                    return EventStateType.Error;
                }

                if (EventErrorCodeGroup == _planedDownTimeGroup)
                {
                    return EventStateType.Error;
                }

                if (EventErrorCode == _cleaningErrorCode
                    || EventErrorCode == _clampingErrorCode
                    || EventErrorCode == _lagErrorCode
                    || EventErrorCode == _samplingErrorCode
                    || EventErrorCode == _switchingErrorCode
                   )
                {
                    return EventStateType.Error;
                }

                return EventStateType.Ok;
            }
        }

        private DateTime _lastUpdate;

        /// <summary>
        /// Poslední aktualizace
        /// 
        /// </summary>
        public DateTime LastUpdate
        {
            get => _lastUpdate;
            set => SetPropertyValue(ref _lastUpdate, value);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Konstruktor pro potřeby designTime
        /// 
        /// </summary>
        public FurnaceViewModel()
        {
            FurnaceNumberStr = "-1";
            Temperature = 333;
            DesiredTemperature = 222;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="furnaceNumberStr">id pece k zobrazení</param>
        /// <param name="section">konfigurace</param>
        public FurnaceViewModel(string furnaceNumberStr, FurnaceBoardSection section)
        {
            _section = section;

            _maxUpDesiredTemperatureOffset = _section.MaxUpDesiredTemperatureOffset;

            _maxDownDesiredTemperatureOffset = _section.MaxDownDesiredTemperatureOffset;

            _downDesiredTemperatureLimit = _section.DownDesiredTemperatureLimit;

            FurnaceNumberStr = furnaceNumberStr;
        }

        #endregion
    }
}
