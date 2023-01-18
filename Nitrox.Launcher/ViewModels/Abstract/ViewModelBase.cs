﻿using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;

namespace Nitrox.Launcher.ViewModels.Abstract;

public class ViewModelBase : ReactiveObject
{
    protected Window MainWindow => Locator.MainWindow;

    /// <summary>
    ///     Shows the dialog (interaction) and returns the result when it closes.
    /// </summary>
    protected async Task<TModalViewModel?> ShowDialogAsync<TModalViewModel>(Interaction<TModalViewModel, TModalViewModel?> interaction) where TModalViewModel : ModalViewModelBase, new()
    {
        return await interaction.Handle(new());
    }
}