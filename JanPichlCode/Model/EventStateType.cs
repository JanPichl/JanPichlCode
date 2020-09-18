//-----------------------------------------------------------------------
// <copyright file="EventStateType.cs" company="I&C Energo a.s.">
//     Copyright (c) 
// </copyright>
// <author>
//     Jan Pichl
// </author>
//-----------------------------------------------------------------------

namespace JanPichlCode.Model
{
    /// <summary>
    /// Stav pracoviště
    /// 
    /// </summary>
    public enum EventStateType
    {
        /// <summary>
        /// Stroj je bez poruchy.
        /// 
        /// </summary>
        Ok,

        /// <summary>
        /// Na stroji je porucha.
        /// 
        /// </summary>
        Error,

    }
}
