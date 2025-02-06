using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using ClientTokenProvider.Core.AzureAd.Exceptions;
using ClientTokenProvider.Shared.Messages;
using ClientTokenProvider.Shared.Services.Abstractions;
using ClientTokenProvider.Shared.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel(
    IConfigurationRepository configurationRepository,
    IConfigurationFactory configurationFactory,
    IConfigurationDataMapper configurationDataMapper,
    IConfigurationDataBackupStore configurationDataBackupStore,
    IClientTokenProviderFactory clientTokenProviderFactory,
    ILogger<ConfigurationManagerViewModel> logger) :
    ViewModelBase
{
    private List<ConfigurationModel> _configurations = [];

    protected override async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurationsResult = await GetAllConfigurations_Internal(cancellationToken);

        if (configurationsResult.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
                new GettingConfigurationsFailedMessage());

            return;
        }

        _configurations = configurationsResult
            .Value
            .ToList();

        foreach (var configuration in _configurations)
        {
            CreateAndAddConfigurationListItem_Internal(configuration);
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
        var configurationResult = await CreateConfiguration_Internal(
                configurationKind,
                cancellationToken);

        if (configurationResult.IsFailure)
        {
            WeakReferenceMessenger.Default.Send(
                new GettingConfigurationsFailedMessage());

            return;
        }

        var configuration = configurationResult.Value;

        _configurations.Add(configuration);

        var configurationListItem = CreateAndAddConfigurationListItem_Internal(
            configuration);

        var configurationDetail = CreateAndAddConfigurationDetail_Internal(
            configuration);

        SetActiveConfigurationListItem_Internal(configurationListItem);

        SetActiveConfigurationDetail_Internal(configurationDetail);
    }

    private async Task<Result<IReadOnlyCollection<ConfigurationModel>>> GetAllConfigurations_Internal(
    CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService
        try
        {
            var configurations = await configurationRepository.GetAll(
                cancellationToken);

            return Result.Success(configurations);
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while getting all configurations");

            return Result.Failure<IReadOnlyCollection<ConfigurationModel>>("An unexpected error occurred");
        }
    }

    private async Task<Result<ConfigurationModel>> CreateConfiguration_Internal(
        ConfigurationKind configurationKind,
        CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService
        try
        {
            var newConfiguration = configurationFactory.Create(
                configurationKind);

            await configurationRepository.Add(
                newConfiguration,
                cancellationToken);

            return newConfiguration;
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while creating a configuration");

            return Result.Failure<ConfigurationModel>("An unexpected error occurred");
        }
    }

    private async Task<Result> RenameConfiguration_Internal(
        Guid configurationId,
        string newName,
        CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService
        try
        {
            var configuration = _configurations
                .FirstOrDefault(configuration => configuration.Id == configurationId);

            if (configuration is null)
            {
                return Result.Failure("Configuration not found");
            }

            configuration.Rename(newName);

            await configurationRepository.Update(configuration, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while renaming a configuration");

            return Result.Failure("An unexpected error occurred");
        }
    }

    private async Task<Result> SaveConfigurationData_Internal(
        Guid configurationId,
        IConfigurationData configurationData,
        CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService

        try
        {
            var configuration = _configurations
               .FirstOrDefault(configuration => configuration.Id == configurationId);

            if (configuration is null)
            {
                return Result.Failure("Configuration not found");
            }

            configuration.UpdateData(configurationData);

            // Maybe it will be better to have REST ConfigurationService
            await configurationRepository.Update(configuration, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while saving configuration data");

            return Result.Failure("An unexpected error occurred");
        }
    }

    private async Task<AccessTokenResult> GetAccessToken_Internal(
        ConfigurationKind configurationKind,
        IConfigurationData configurationData,
        CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService
        try
        {
            var clientTokenProviderConfiguration = configurationData.ToClientTokenProviderConfiguration();

            var clientTokenProvider = clientTokenProviderFactory.Create(
                configurationKind,
                clientTokenProviderConfiguration);

            var accessToken = await clientTokenProvider.GetAccessToken(
                configurationData.Scope,
                cancellationToken);

            return AccessTokenResult.Succeeded(accessToken);

        }
        catch (ClientHandlerException clientHandlerException)
        {
            return AccessTokenResult.Failed(clientHandlerException.Message);
        }
        catch (OperationCanceledException)
        {
            return AccessTokenResult.Cancelled();
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while saving configuration data");

            return AccessTokenResult.Failed(ex.Message);
        }
    }

    private async Task<Result> RemoveConfiguration_Internal(
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService
        try
        {
            var configuration = _configurations.FirstOrDefault(
                configuration => configuration.Id == configurationId);

            if (configuration is null)
            {
                return Result.Failure("Configuration not found");
            }

            await configurationRepository.Delete(
                configuration,
                cancellationToken);

            return Result.Success();

        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while deleting a configuration");

            return Result.Failure("An unexpected error occurred");
        }
    }
}
