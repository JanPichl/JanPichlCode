//-----------------------------------------------------------------------
// <copyright file="FurnaceBoardSection.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Sekce konfiguračního souboru.
    /// 
    /// </summary>
    public class FurnaceBoardSection : ConfigurationSection
    {
        public static FurnaceBoardSection GetSection()
        {
            return ConfigurationManager.GetSection("furnaceBoard") as FurnaceBoardSection;
        }

        /// <summary>
        /// Interval obnovování dat.
        /// 
        /// </summary>
        [ConfigurationProperty("checkIntervalInSeconds", IsRequired = false)]
        public int? CheckIntervalInSeconds
        {
            get { return (int?)base["checkIntervalInSeconds"]; }
        }

        /// <summary>
        /// Horní maximální povolená odchylka od nastavené teploty pece.
        /// 
        /// Při překročení této hodnoty je signalizován stav vysoká teplota pece
        /// 
        /// </summary>
        [ConfigurationProperty("maxUpDesiredTemperatureOffset", IsRequired = false, DefaultValue = 10d)]
        public double MaxUpDesiredTemperatureOffset
        {
            get { return (double)base["maxUpDesiredTemperatureOffset"]; }
        }

        /// <summary>
        /// Spodní maximální povolená odchylka od nastavené teploty pece
        /// 
        /// Při poklesu pod tuto hodnotu je signalizován nedostatečná teplota pece
        /// 
        /// </summary>
        [ConfigurationProperty("maxDownDesiredTemperatureOffset", IsRequired = false, DefaultValue = 10d)]
        public double MaxDownDesiredTemperatureOffset
        {
            get { return (double)base["maxDownDesiredTemperatureOffset"]; }
        }

        /// <summary>
        /// Limitní spodní teplota pece. 
        /// 
        /// Při poklesu pod tuto hodnotu je signalizován stav pec mimo provoz
        /// 
        /// </summary>
        [ConfigurationProperty("downDesiredTemperatureLimit", IsRequired = false, DefaultValue = 660d)]
        public double DownDesiredTemperatureLimit
        {
            get { return (double)base["downDesiredTemperatureLimit"]; }
        }
    }
}
