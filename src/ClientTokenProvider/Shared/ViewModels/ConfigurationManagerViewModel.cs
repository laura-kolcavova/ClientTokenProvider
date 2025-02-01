using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Business.Shared.Services.Abstractions;
using ClientTokenProvider.Shared.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CSharpFunctionalExtensions;

namespace ClientTokenProvider.Shared.ViewModels;

public partial class ConfigurationManagerViewModel(
    IConfigurationRepository configurationRepository,
    IConfigurationFactory configurationFactory) :
    ViewModelBase
{
    private List<ConfigurationModel> _configurations = [];

    protected override async Task Load(CancellationToken cancellationToken)
    {
        WeakReferenceMessenger.Default.RegisterAll(this);

        var configurations = await configurationRepository.GetAll(cancellationToken);

        _configurations = configurations
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

    [RelayCommand]
    private void AddNewConfiguration(
        ConfigurationKind configurationKind)
    {
        var configuration = CreateAndAddConfiguration_Internal(
                configurationKind);

        var configurationListItem = CreateAndAddConfigurationListItem_Internal(
            configuration);

        var configurationDetail = CreateAndAddConfigurationDetail_Internal(
            configuration);

        SetActiveConfigurationListItem_Internal(configurationListItem);

        SetActiveConfigurationDetail_Internal(configurationDetail);
    }

    private ConfigurationModel CreateAndAddConfiguration_Internal(
        ConfigurationKind configurationKind)
    {
        var newConfiguration = configurationFactory.Create(
                configurationKind);

        _configurations.Add(newConfiguration);

        return newConfiguration;
    }

    private async Task<Result> RenameConfiguration_Internal(
        ConfigurationModel configuration,
        string newName,
        CancellationToken cancellationToken)
    {
        try
        {
            configuration.Rename(newName);

            await configurationRepository.Update(configuration, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
