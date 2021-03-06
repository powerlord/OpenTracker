﻿using OpenTracker.Models.Dropdowns;
using System;

namespace OpenTracker.Models.UndoRedo
{
    public class UncheckDropdown : IUndoable
    {
        private readonly IDropdown _dropdown;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dropdown">
        /// The dropdown to be checked.
        /// </param>
        public UncheckDropdown(IDropdown dropdown)
        {
            _dropdown = dropdown ?? throw new ArgumentNullException(nameof(dropdown));
        }

        /// <summary>
        /// Returns whether the action can be executed.
        /// </summary>
        /// <returns>
        /// A boolean representing whether the action can be executed.
        /// </returns>
        public bool CanExecute()
        {
            return _dropdown.Checked;
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        public void Execute()
        {
            _dropdown.Checked = false;
        }

        /// <summary>
        /// Undoes the action.
        /// </summary>
        public void Undo()
        {
            _dropdown.Checked = true;
        }
    }
}
