using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Shared.BindableModels;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel
{
    [ObservableProperty]
    private ObservableCollection<ConfigurationDetailBindableModel> _configurationDetails = [];

    [ObservableProperty]
    private ConfigurationDetailBindableModel? _activeConfigurationDetail;

    [ObservableProperty]
    private ConfigurationManagerState _currentState = ConfigurationManagerState.NoContent;

    public bool CanChangeState => true;

    partial void OnActiveConfigurationDetailChanged(
        ConfigurationDetailBindableModel? value)
    {
        if (value is null)
        {
            CurrentState = ConfigurationManagerState.NoContent;
        }
        else
        {
            CurrentState = ConfigurationManagerState.ShowConfigurationPresenter;
        }
    }

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task RenameConfiguration(
        ConfigurationDetailBindableModel configurationDetail,
        CancellationToken cancellationToken)
    {
        var configuration = _configurations
            .FirstOrDefault(configuration => configuration.Id == configurationDetail.Id);

        if (configuration is null)
        {
            return;
        }

        var renameResult = await RenameConfiguration_Internal(
               configuration,
               configurationDetail.Name,
               cancellationToken);

        if (renameResult.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
               new SavingConfigurationFailedMessage());

            return;
        }

        RenameConfigurationListItem_Internal(
            configuration.Id,
            configurationDetail.Name);
    }

    private ConfigurationDetailBindableModel CreateAndAddConfigurationDetail_Internal(
        ConfigurationModel configuration)
    {
        var configurationDetail = new ConfigurationDetailBindableModel(
            configuration.Id,
            configuration.Kind,
            configuration.Name,
            null!);

        ConfigurationDetails.Add(configurationDetail);

        return configurationDetail;
    }

    private void SetActiveConfigurationDetail_Internal(
       ConfigurationDetailBindableModel configurationDetail)
    {
        ActiveConfigurationDetail = configurationDetail;
    }

    private void SetActiveConfigurationDetail_Internal(
        Guid configurationId)
    {
        var activeConfigurationDetail = ConfigurationDetails
            .FirstOrDefault(configurationDetail => configurationDetail.Id == configurationId);

        if (activeConfigurationDetail is null)
        {
            var configuration = _configurations
                .FirstOrDefault(configuration => configuration.Id == configurationId);

            if (configuration is not null)
            {
                activeConfigurationDetail = CreateAndAddConfigurationDetail_Internal(configuration);
            }
        }

        ActiveConfigurationDetail = activeConfigurationDetail;
    }
}
