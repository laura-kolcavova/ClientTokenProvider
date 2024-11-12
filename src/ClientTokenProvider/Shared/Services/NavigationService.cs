namespace ClientTokenProvider.Shared.Services;

internal sealed class NavigationService : INavigationService
{
    public async Task GoToAzureAdConfigurationDetail(Guid configurationId)
    {
        var queryParameters = new ShellNavigationQueryParameters()
        {
            [Routes.AzureAdConfigurationDetail.QueryParamConfigurationDetail] = configurationId
        };

        await Shell.Current.GoToAsync(
            Routes.AzureAdConfigurationDetail.Path,
            queryParameters);
    }
}
