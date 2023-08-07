﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Nitrox.Launcher.Models;
using Nitrox.Launcher.Models.Messages;
using Nitrox.Launcher.ViewModels.Abstract;
using NitroxModel.Serialization;
using NitroxModel.Server;
using ReactiveUI;

namespace Nitrox.Launcher.ViewModels;

public partial class ServersViewModel : RoutableViewModelBase
{
    public static readonly string SavesFolderDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Nitrox", "saves");

    public AvaloniaList<ServerEntry> Servers { get; } = new();

    public ServersViewModel(IScreen hostScreen) : base(hostScreen)
    {
        WeakReferenceMessenger.Default.Register<ServerEntryPropertyChangedMessage>(this, (sender, message) =>
        {
            if (message.PropertyName == nameof(ServerEntry.IsOnline))
            {
                ManageServerCommand.NotifyCanExecuteChanged();
            }
        });


        Servers = new AvaloniaList<ServerEntry>(GetSavesOnDisk());
    }

    [RelayCommand]
    public async Task CreateServer()
    {
        CreateServerViewModel result = await MainViewModel.ShowDialogAsync<CreateServerViewModel>();
        if (result == null)
        {
            return;
        }

        AddServer(result.Name, result.SelectedGameMode);
    }

    [RelayCommand]
    public void ManageServer(ServerEntry server)
    {
        ManageServerViewModel viewModel = AppViewLocator.GetSharedViewModel<ManageServerViewModel>();
        viewModel.LoadFrom(server);
        MainViewModel.Router.Navigate.Execute(viewModel);
    }

    private void AddServer(string name, ServerGameMode gameMode)
    {
        Servers.Insert(0, new ServerEntry
        {
            Name = name,
            GameMode = gameMode,
            Seed = ""
        });
    }

    public IEnumerable<ServerEntry> GetSavesOnDisk()
    {
        static bool ValidateSave(string saveDir) => !File.Exists(Path.Combine(saveDir, "server.cfg")) || File.Exists(Path.Combine(saveDir, "Version.json"));

        foreach (string folder in Directory.EnumerateDirectories(SavesFolderDir))
        {
            // Don't add the file to the list if it doesn't validate
            if (!ValidateSave(folder))
            {
                continue;
            }

            string saveName = Path.GetFileName(folder);
            string saveDir = Path.Combine(SavesFolderDir, saveName);
            SubnauticaServerConfig server = SubnauticaServerConfig.Load(saveDir);
            yield return new ServerEntry
            {
                Name = saveName,
                Password = server.ServerPassword,
                Seed = server.Seed,
                GameMode = server.GameMode,
                PlayerPermissions = server.DefaultPlayerPerm,
                AutoSaveInterval = server.SaveInterval / 1000,
                MaxPlayers = server.MaxConnections,
                Port = server.ServerPort,
                AutoPortForward = server.AutoPortForward,
                AllowLanDiscovery = server.LANDiscoveryEnabled,
                AllowCommands = !server.DisableConsole,
                IsNewServer = !File.Exists(Path.Combine(saveDir, "WorldData.json")),
            };
        }
    }
}
