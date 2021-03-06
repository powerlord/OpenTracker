﻿using Avalonia.Controls;
using System.Threading.Tasks;

namespace OpenTracker.Interfaces
{
    /// <summary>
    /// This is the interface for the dialog service.
    /// </summary>
    public interface IDialogService
    {
        Window Owner { get; }

        void Register<TViewModel, TView>()
            where TViewModel : IDialogRequestClose
            where TView : IDialog;
        Task<bool?> ShowDialog<TViewModel>(TViewModel viewModel)
            where TViewModel : IDialogRequestClose;
    }
}
