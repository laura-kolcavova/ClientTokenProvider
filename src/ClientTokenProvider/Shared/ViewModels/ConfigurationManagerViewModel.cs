using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel(
    IConfigurationService configurationService) :
    ViewModelBase
{
    [ObservableProperty]
    private Configuration? _activeConfiguration;

    private readonly ObservableCollection<Configuration> _configurations = [];

    public IReadOnlyCollection<Configuration> Configurations => _configurations;

    protected override async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurations = await configurationService.GetAll(cancellationToken);

        foreach (var configuration in configurations)
        {
            _configurations.Add(configuration);
        }
    }

    protected override Task Unload(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);

        return Task.CompletedTask;
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
        Configuration configuration,
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
        Configuration configuration,
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
    private void SetActiveConfiguration(Configuration configuration)
    {
        SetActiveConfiguration_Internal(configuration);
    }

    private void AddConfigurationToList_Internal(Configuration configuration)
    {
        _configurations.Add(configuration); ;
    }

    private void RemoveConfigurationFromList_Internal(Configuration configuration)
    {
        _configurations.Remove(configuration);
    }

    private void SetActiveConfiguration_Internal(Configuration configurationToBeActive)
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
