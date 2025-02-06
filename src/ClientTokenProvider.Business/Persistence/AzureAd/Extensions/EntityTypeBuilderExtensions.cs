using ClientTokenProvider.Business.Persistence.AzureAd.Converters;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using ClientTokenProvider.Business.Shared.Serializaiton;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ClientTokenProvider.Business.Persistence.AzureAd.Extensions;

internal static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder<IConfigurationData> HasAzureAdConfigurationDataConversion(
        this PropertyBuilder<IConfigurationData> propertyBuilder,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        propertyBuilder.HasConversion(
            new ConfigurationDataConverter(
                jsonSerializerOptions ?? DefaultJsonSerializer.Options));

        return propertyBuilder;
    }
}

