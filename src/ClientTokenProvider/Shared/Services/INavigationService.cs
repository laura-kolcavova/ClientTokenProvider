namespace ClientTokenProvider.Shared.Services;

public interface INavigationService
{
    public Task GoToAzureAdConfigurationDetail(Guid configurationId);
}
