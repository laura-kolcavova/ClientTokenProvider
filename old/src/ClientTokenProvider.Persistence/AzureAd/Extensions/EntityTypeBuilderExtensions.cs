using ClientTokenProvider.Business.Shared.Models.Abstractions;
using ClientTokenProvider.Business.Shared.Serializaiton;
using ClientTokenProvider.Persistence.AzureAd.Converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ClientTokenProvider.Persistence.AzureAd.Extensions;

internal static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder<IConfigurationData> HasAzureAdConfigurationDataConversion(
        this PropertyBuilder<IConfigurationData> propertyBuilder,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        propertyBuilder.HasConversion(
            new AzureAdConfigurationDataConverter(
                jsonSerializerOptions ?? DefaultJsonSerializer.Options));

        return propertyBuilder;
    }
}

