using KittyToken.Application.AzureAd.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace KittyToken.Application.AzureAd.Persistence;

internal sealed class AzureAdConfigurationValueConverter :
    ValueConverter<AzureAdConfiguration, string>
{
    public AzureAdConfigurationValueConverter()
        : base(
            value => Serialize(value),
            value => Deserialize(value))
    {
    }

    private static string Serialize(
        AzureAdConfiguration value)
    {
        return JsonSerializer.Serialize(
            value);
    }

    private static AzureAdConfiguration Deserialize(
        string value)
    {
        return JsonSerializer.Deserialize<AzureAdConfiguration>(
            value)!;
    }
}