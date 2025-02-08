using ClientTokenProvider.Business.Shared.Errors;
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
    IJwtDecoder jwtDecoder,
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
                new AddingConfigurationFailedMessage());

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

    private async Task<Result<IReadOnlyCollection<ConfigurationModel>, Error>> GetAllConfigurations_Internal(
    CancellationToken cancellationToken)
    {
        // Maybe it will be better to have REST ConfigurationService
        try
        {
            var configurations = await configurationRepository.GetAll(
                cancellationToken);

            return Result.Success<IReadOnlyCollection<ConfigurationModel>, Error>(configurations);
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while getting all configurations");

            return GeneralErrors.General.Unexpected();
        }
    }

    private async Task<Result<ConfigurationModel, Error>> CreateConfiguration_Internal(
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

            return GeneralErrors.General.Unexpected();
        }
    }

    private async Task<UnitResult<Error>> RenameConfiguration_Internal(
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
                return ConfigurationErrors.Configuration.NotFound();
            }

            configuration.Rename(newName);

            await configurationRepository.Update(configuration, cancellationToken);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while renaming a configuration");

            return GeneralErrors.General.Unexpected();
        }
    }

    private async Task<UnitResult<Error>> SaveConfigurationData_Internal(
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
                return ConfigurationErrors.Configuration.NotFound();
            }

            configuration.UpdateData(configurationData);

            // Maybe it will be better to have REST ConfigurationService
            await configurationRepository.Update(configuration, cancellationToken);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while saving configuration data");

            return GeneralErrors.General.Unexpected();
        }
    }

    private async Task<Result<string, Error>> GetAccessToken_Internal(
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

            return accessToken;

        }
        catch (ClientHandlerException clientHandlerException)
        {
            return AccessTokenErrors.AccessToken.RequestFailed(clientHandlerException.Message);
        }
        catch (OperationCanceledException)
        {
            return GeneralErrors.General.Cancelled();
        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while saving configuration data");

            return GeneralErrors.General.Unexpected();
        }
    }

    private async Task<UnitResult<Error>> RemoveConfiguration_Internal(
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
                return ConfigurationErrors.Configuration.NotFound();
            }

            await configurationRepository.Delete(
                configuration,
                cancellationToken);

            return UnitResult.Success<Error>();

        }
        catch (Exception ex)
        {
            logger.LogError(
               ex,
               "An unexpected error occurred while deleting a configuration");

            return GeneralErrors.General.Unexpected();
        }
    }
}
