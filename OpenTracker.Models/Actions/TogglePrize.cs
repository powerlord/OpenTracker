﻿using OpenTracker.Models.Interfaces;
using OpenTracker.Models.Sections;

namespace OpenTracker.Models.Actions
{
    public class TogglePrize : IUndoable
    {
        private readonly BossSection _prizeSection;

        public TogglePrize(BossSection prizeSection)
        {
            _prizeSection = prizeSection;
        }

        public void Execute()
        {
            if (_prizeSection.IsAvailable())
                _prizeSection.Available = 0;
            else
                _prizeSection.Available = 1;
        }

        public void Undo()
        {
            if (_prizeSection.IsAvailable())
                _prizeSection.Available = 0;
            else
                _prizeSection.Available = 1;
        }
    }
}
