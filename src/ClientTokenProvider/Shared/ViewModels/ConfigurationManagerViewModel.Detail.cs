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
        var renameResult = await RenameConfiguration_Internal(
               configurationDetail.Id,
               configurationDetail.Name,
               cancellationToken);

        if (renameResult.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
               new RenamingConfigurationFailedMessage());

            return;
        }

        RenameConfigurationListItem_Internal(
            configurationDetail.Id,
            configurationDetail.Name);
    }

    [RelayCommand]
    private void HandleConfigurationDataChanged(
        ConfigurationDetailBindableModel configurationDetail)
    {
        var anyChanges = configurationDataBackupStore.AnyChanges(
            configurationDetail.Id,
            configurationDetail.Data);

        if (anyChanges)
        {
            configurationDetail.CanGetAccessToken = configurationDetail
                .Data
                .AreDataValid();
        }

        configurationDetail.CanBeSaved = anyChanges;
    }

    [RelayCommand(
        IncludeCancelCommand = true,
        AllowConcurrentExecutions = false)]
    private async Task SaveConfigurationData(
        ConfigurationDetailBindableModel configurationDetail,
        CancellationToken cancellationToken)
    {
        var configurationData = configurationDataMapper.ToModel(
            configurationDetail.Data,
            configurationDetail.Kind);

        var saveConfigurationDataResult = await SaveConfigurationData_Internal(
            configurationDetail.Id,
            configurationData,
            cancellationToken);

        if (saveConfigurationDataResult.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
               new SavingConfigurationDataFailedMessage());

            return;
        }

        configurationDataBackupStore.Set(
          configurationDetail.Id,
          configurationDetail.Data);

        configurationDetail.CanBeSaved = false;
    }

    [RelayCommand(
       IncludeCancelCommand = true,
       AllowConcurrentExecutions = false)]
    private async Task GetAccessToken(
       ConfigurationDetailBindableModel configurationDetail,
       CancellationToken cancellationToken)
    {
        var configurationData = configurationDataMapper.ToModel(
            configurationDetail.Data,
            configurationDetail.Kind);

        configurationDetail.IsLoading = true;

        var accessTokenResult = await GetAccessToken_Internal(
            configurationDetail.Kind,
            configurationData,
            cancellationToken);

        configurationDetail.IsLoading = false;

        if (accessTokenResult.State == AccessTokenResultState.Cancelled)
        {
            return;
        }

        configurationDetail.AccessTokenResult = accessTokenResult;
    }

    [RelayCommand]
    private void ShowAccessTokenErrorDetail(
        ConfigurationDetailBindableModel configurationDetail)
    {
        if (configurationDetail.AccessTokenResult.State == AccessTokenResultState.Failed)
        {
            WeakReferenceMessenger.Default.Send(new ShowAccessTokenErrorDetailMessage
            {
                ErrorMessage = configurationDetail.AccessTokenResult.Text!
            });
        }
    }

    private ConfigurationDetailBindableModel CreateAndAddConfigurationDetail_Internal(
        ConfigurationModel configuration)
    {
        var configurationData = configurationDataMapper.ToBindableModel(
           configuration.Data,
           configuration.Kind);

        var configurationDetail = new ConfigurationDetailBindableModel(
            configuration.Id,
            configuration.Kind,
            configuration.Name,
            configurationData);

        configurationDataBackupStore.Set(
            configurationDetail.Id,
            configurationDetail.Data);

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
