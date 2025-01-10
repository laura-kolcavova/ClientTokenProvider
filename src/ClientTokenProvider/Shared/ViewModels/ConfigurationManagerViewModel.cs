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
    private IReadOnlyCollection<Configuration> _configurations = [];

    [ObservableProperty]
    private Configuration? _activeConfiguration;

    protected override async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurations = await configurationService.GetAll(cancellationToken);

        Configurations = configurations;
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

    private void AddConfigurationToList(Configuration configuration)
    {
        var newConfigurations = Configurations.ToList();
        newConfigurations.Add(configuration);
        Configurations = newConfigurations;
    }

    private void SetActiveConfiguration(Configuration configuration)
    {
        ActiveConfiguration = configuration;
    }
}
