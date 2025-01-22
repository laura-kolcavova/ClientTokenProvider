using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel(
    IConfigurationService configurationService) :
    ViewModelBase
{
    [ObservableProperty]
    private List<Configuration> _configurations = [];

    [ObservableProperty]
    private Configuration? _activeConfiguration;

    protected override async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurations = await configurationService.GetAll(cancellationToken);

        Configurations = configurations.ToList();
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

            AddConfigurationToList(newConfiguration);

            SetActiveConfiguration(newConfiguration);
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
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        try
        {
            await configurationService.Delete(
                configurationId,
                cancellationToken);

            RemoveConfigurationFromList(configurationId);

            SetLastOpenedConfigurationAsActive();
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

            UpdateConfigurationInList(configuration);
        }
        catch
        {
            WeakReferenceMessenger.Default.Send(
                new SavingConfigurationFailedMessage());
        }
    }

    private void AddConfigurationToList(Configuration configuration)
    {
        var newConfigurations = Configurations.ToList();
        newConfigurations.Add(configuration);
        Configurations = newConfigurations;
    }

    private void RemoveConfigurationFromList(Guid configurationGuid)
    {
        var index = Configurations
            .FindIndex(configuration => configuration.Id == configurationGuid);

        if (index == -1)
        {
            return;
        }

        var newConfigurations = Configurations.ToList();
        newConfigurations.RemoveAt(index);
        Configurations = newConfigurations;
    }

    private void UpdateConfigurationInList(Configuration configurationToUpdate)
    {
        var index = Configurations
            .FindIndex(configuration => configuration.Id == configurationToUpdate.Id);

        if (index == -1)
        {
            return;
        }

        var newConfigurations = Configurations.ToList();
        newConfigurations[index] = configurationToUpdate;
        Configurations = newConfigurations;
    }

    private void SetActiveConfiguration(Configuration configuration)
    {
        ActiveConfiguration = configuration;
    }

    private void SetLastOpenedConfigurationAsActive()
    {
        // TODO List of opened configurations

        ActiveConfiguration = null;
    }
}
