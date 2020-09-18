//-----------------------------------------------------------------------
// <copyright file="BaseViewModel.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Základní třída pro ViewModely
    /// 
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Nastaví hodnotu do property, v případě že je rozdílná a poté vyvolá PropertyChanged
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyBackingField">reference na field.</param>
        /// <param name="value">Nastavovaná hodnota.</param>
        /// <param name="propertyName">Nepovinné vezme se z volajícího.</param>
        /// <returns>Příznak zda byla hodnota nastavena.</returns>
        protected virtual bool SetPropertyValue<T>(ref T propertyBackingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(propertyBackingField, value))
            {
                propertyBackingField = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
