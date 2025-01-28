using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Models;
using ClientTokenProvider.Shared.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel(
    IConfigurationService configurationService,
    IConfigurationActionStateStore configurationActionStateStore) :
    ViewModelBase
{
    [ObservableProperty]
    private ConfigurationModel? _activeConfiguration;

    [ObservableProperty]
    private ObservableCollection<ConfigurationModel> _configurations = [];

    //public IReadOnlyList<ConfigurationModel> Configurations => _configurations;

    [ObservableProperty]
    private ConfigurationManagerState _currentState = ConfigurationManagerState.NoContent;

    [ObservableProperty]
    private ConfigurationActionState _configurationActionState = ConfigurationActionState.Idle;

    public bool CanChangeState => true;

    protected override async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurations = await configurationService.GetAll(cancellationToken);

        foreach (var configuration in configurations)
        {
            Configurations.Add(configuration);
        }
    }

    protected override Task Unload(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);

        return Task.CompletedTask;
    }

    partial void OnActiveConfigurationChanged(ConfigurationModel? value)
    {
        if (value is null)
        {
            CurrentState = ConfigurationManagerState.NoContent;
        }
        else
        {
            CurrentState = ConfigurationManagerState.ShowConfigurationPresenter;

            ConfigurationActionState = configurationActionStateStore.Get(value);
        }
    }

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task AddNewConfiguration(
        ConfigurationKind configurationKind,
        CancellationToken cancellationToken)
    {
        try
        {
            var newConfiguration = await configurationService.Create(
                configurationKind,
                cancellationToken);

            AddConfigurationToList_Internal(newConfiguration);

            SetActiveConfiguration_Internal(newConfiguration);
        }
        catch
        {
            WeakReferenceMessenger.Default.Send(
                new AddingNewConfigurationFailedMessage());
        }
    }

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task DeleteConfiguration(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        try
        {
            await configurationService.Delete(
                configuration.Id,
                cancellationToken);

            RemoveConfigurationFromList_Internal(configuration);

            SetLastOpenedConfigurationAsActive_Internal();
        }
        catch
        {
            WeakReferenceMessenger.Default.Send(
                new DeletingConfigurationFailedMessage());
        }
    }

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task SaveConfiguration(
        ConfigurationModel configuration,
        CancellationToken cancellationToken)
    {
        try
        {
            await configurationService.Save(
                configuration,
                cancellationToken);
        }
        catch
        {
            WeakReferenceMessenger.Default.Send(
                new SavingConfigurationFailedMessage());
        }
    }

    [RelayCommand]
    private void SetActiveConfiguration(ConfigurationModel configuration)
    {
        SetActiveConfiguration_Internal(configuration);
    }

    [RelayCommand]
    private void RenameConfiguration(RenameConfigurationRequest renameConfigurationRequest)
    {
        var configuration = renameConfigurationRequest.Configuration;
        var newName = renameConfigurationRequest.NewName;

        if (configuration.Name == newName)
        {
            return;
        }

        var index = Configurations.IndexOf(configuration);

        if (index == -1)
        {
            return;
        }

        configuration
            .Rename(renameConfigurationRequest.NewName);

        //Configurations[index] = configuration;

        //if (ActiveConfiguration == configuration)
        //{
        //    OnPropertyChanged(nameof(ActiveConfiguration));
        //}
    }

    private void AddConfigurationToList_Internal(ConfigurationModel configuration)
    {
        Configurations.Add(configuration);
    }

    private void RemoveConfigurationFromList_Internal(ConfigurationModel configuration)
    {
        Configurations.Remove(configuration);
    }

    private void SetActiveConfiguration_Internal(ConfigurationModel configurationToBeActive)
    {
        if (ActiveConfiguration?.Id == configurationToBeActive.Id)
        {
            return;
        }

        ActiveConfiguration = configurationToBeActive;
    }

    private void SetLastOpenedConfigurationAsActive_Internal()
    {
        // TODO List of opened configurations

        ActiveConfiguration = null;
    }
}
