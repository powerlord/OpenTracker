﻿using OpenTracker.Models.Enums;
using OpenTracker.Models.Interfaces;
using OpenTracker.Models.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace OpenTracker.Models.Actions
{
    public class ClearLocation : IUndoable
    {
        private readonly Game _game;
        private readonly Location _location;
        private readonly bool _force;
        private readonly List<int?> _previousLocationCounts;
        private readonly List<MarkingType?> _previousMarkings;
        private readonly List<bool?> _previousUserManipulated;
        private readonly List<Item> _markedItems;
        private readonly List<Item> _prizes;
        private readonly List<int?> _prizePreviousCurrents;

        public ClearLocation(Game game, Location location, bool force = false)
        {
            _game = game;
            _location = location;
            _force = force;
            _previousLocationCounts = new List<int?>();
            _previousMarkings = new List<MarkingType?>();
            _previousUserManipulated = new List<bool?>();
            _markedItems = new List<Item>();
            _prizes = new List<Item>();
            _prizePreviousCurrents = new List<int?>();
        }

        public void Execute()
        {
            _previousLocationCounts.Clear();
            _previousMarkings.Clear();
            _previousUserManipulated.Clear();
            _markedItems.Clear();
            _prizes.Clear();
            _prizePreviousCurrents.Clear();

            foreach (ISection section in _location.Sections)
            {
                if (section.IsAvailable() && (section is EntranceSection || _force ||
                    section.Accessibility > AccessibilityLevel.Inspect ||
                    (section.Accessibility == AccessibilityLevel.Inspect && section.Marking == null)))
                {
                    _previousLocationCounts.Add(section.Available);
                    _previousMarkings.Add(section.Marking);
                    _previousUserManipulated.Add(section.UserManipulated);

                    section.UserManipulated = true;

                    if (section is ItemSection && section.Marking.HasValue)
                    {
                        if (Enum.TryParse(section.Marking.Value.ToString(), out ItemType itemType))
                        {
                            Item item = _game.Items[itemType];

                            if (item.Current < item.Maximum)
                                _markedItems.Add(item);
                            else
                                _markedItems.Add(null);
                        }
                        else
                            _markedItems.Add(null);
                    }
                    else
                        _markedItems.Add(null);

                    if (section is BossSection bossSection)
                    {
                        _prizes.Add(bossSection.Prize);

                        if (bossSection.Prize != null)
                            _prizePreviousCurrents.Add(bossSection.Prize.Current);
                        else
                            _prizePreviousCurrents.Add(null);
                    }
                    else
                    {
                        _prizes.Add(null);
                        _prizePreviousCurrents.Add(null);
                    }

                    section.Clear(_force);

                    if (section.IsAvailable())
                        _markedItems[^1] = null;
                }
                else
                {
                    _previousLocationCounts.Add(null);
                    _previousMarkings.Add(null);
                    _previousUserManipulated.Add(null);
                    _markedItems.Add(null);
                    _prizes.Add(null);
                    _prizePreviousCurrents.Add(null);
                }
            }
        }

        public void Undo()
        {
            for (int i = 0; i < _previousLocationCounts.Count; i++)
            {
                if (_previousLocationCounts[i] != null)
                    _location.Sections[i].Available = _previousLocationCounts[i].Value;

                if (_previousMarkings[i] != null)
                    _location.Sections[i].Marking = _previousMarkings[i];

                if (_previousUserManipulated[i] != null)
                    _location.Sections[i].UserManipulated = _previousUserManipulated[i].Value;

                if (_markedItems[i] != null)
                    _markedItems[i].Change(-1);

                if (_prizes[i] != null && _prizes[i].Current != _prizePreviousCurrents[i])
                    _prizes[i].SetCurrent(_prizePreviousCurrents[i].Value);
            }
        }
    }
}
