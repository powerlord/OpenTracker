﻿using System.ComponentModel;

namespace OpenTracker.Models.AutoTracking.AutotrackValues
{
    /// <summary>
    /// This is the interface of the autotracking value.
    /// </summary>
    public interface IAutoTrackValue : INotifyPropertyChanged
    {
        int? CurrentValue { get; }
    }
}