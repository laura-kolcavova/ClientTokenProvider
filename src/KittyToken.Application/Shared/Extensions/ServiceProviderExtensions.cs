using KittyToken.Application.AzureAd.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace KittyToken.Application.Shared.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider InitKittyTokenStorage(
        this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        scope.ServiceProvider
            .GetRequiredService<AzureAdFileDbContext>()
            .Database.EnsureCreated();

        return serviceProvider;
    }
}
