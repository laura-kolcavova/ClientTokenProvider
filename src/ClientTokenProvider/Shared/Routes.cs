using ClientTokenProvider.AzureAd.Views;

namespace ClientTokenProvider.Shared;

internal static class Routes
{
    public static class AzureAdConfigurationDetail
    {
        public const string Path = "azureAdConfiguration";

        public static readonly Type PageType = typeof(ConfigurationDetailView);

        public const string QueryParamConfigurationDetail = "configurationDetail";
    }

    public static void RegisterRoutes()
    {
        Routing.RegisterRoute(AzureAdConfigurationDetail.Path, AzureAdConfigurationDetail.PageType);
    }
}
