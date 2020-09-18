//-----------------------------------------------------------------------
// <copyright file="TestConverter.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Assets.Convertors
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(object))]
    public class TestConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
#if DEBUG
            Debug.WriteLine(value);

            Debug.WriteLine(parameter);
#endif
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
