﻿using OpenTracker.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel;

namespace OpenTracker.Models
{
    public class BossDictionary : Dictionary<BossType, Boss>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; 

        private AccessibilityLevel _unknownBossAccessibility;
        public AccessibilityLevel UnknownBossAccessibility
        {
            get => _unknownBossAccessibility;
            private set
            {
                if (_unknownBossAccessibility != value)
                {
                    _unknownBossAccessibility = value;
                    OnPropertyChanged(nameof(UnknownBossAccessibility));
                }
            }
        }

        public BossDictionary(int capacity) : base(capacity)
        {
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnRequirementChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateUnknownBossAccessibility();
        }

        private void UpdateUnknownBossAccessibility()
        {
            bool BossInaccessibility = false;

            foreach (Boss boss in Values)
            {
                if (boss.Type != BossType.Aga)
                {
                    if (boss.Accessibility < AccessibilityLevel.SequenceBreak)
                    {
                        BossInaccessibility = true;
                        break;
                    }
                }
            }

            if (BossInaccessibility)
                UnknownBossAccessibility = AccessibilityLevel.SequenceBreak;
            else
                UnknownBossAccessibility = AccessibilityLevel.Normal;
        }

        public void Initialize()
        {
            foreach (Boss boss in Values)
            {
                if (boss.Type != BossType.Aga)
                    boss.PropertyChanged += OnRequirementChanged;
            }

            UpdateUnknownBossAccessibility();
        }
    }
}
