﻿using System;
using System.ComponentModel;

namespace OpenTracker.Models.AutoTracking.AutotrackValues
{
    /// <summary>
    /// This is the class for representing the autotracking result value of a memory address value.
    /// </summary>
    public class AutoTrackAddressValue : AutoTrackValue
    {
        private readonly MemoryAddress _address;
        private readonly byte _maximum;
        private readonly int _adjustment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="address">
        /// The memory address for the comparison.
        /// </param>
        /// <param name="maximum">
        /// The maximum valid value of the memory address.
        /// </param>
        /// <param name="adjustment">
        /// The amount that the result value should be adjusted from the true value.
        /// </param>
        public AutoTrackAddressValue(MemoryAddress address, byte maximum, int adjustment)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _maximum = maximum;
            _adjustment = adjustment;

            _address.PropertyChanged += OnMemoryChanged;
        }

        /// <summary>
        /// Subscribes to the PropertyChanged event on the MemoryAddress class.
        /// </summary>
        /// <param name="sender">
        /// The sending object of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the PropertyChanged event.
        /// </param>
        private void OnMemoryChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MemoryAddress.Value))
            {
                UpdateCurrentValue();
            }
        }

        /// <summary>
        /// Updates the current value of this value.
        /// </summary>
        private void UpdateCurrentValue()
        {
            int newValue = _address.Value + _adjustment;

            if (newValue > _maximum)
            {
                CurrentValue = null;
            }
            else
            {
                CurrentValue = newValue;
            }
        }
    }
}
