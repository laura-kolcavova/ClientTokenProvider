using ClientTokenProvider.Business.Persistence.Converters;
using ClientTokenProvider.Business.Shared.Models.Abstractions;
using ClientTokenProvider.Business.Shared.Serializaiton;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;


namespace ClientTokenProvider.Business.Persistence.Extensions;
internal static class EntityTypeBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasEnumToStringConversion<TProperty>(
        this PropertyBuilder<TProperty> propertyBuilder)
        where TProperty : struct, Enum
    {
        // If string cannot be converted to enum the default value will be used.
        propertyBuilder.HasConversion(new EnumToStringConverter<TProperty>());

        return propertyBuilder;
    }

    public static PropertyBuilder<IConfigurationData> HasConfigurationDataConversion(
        this PropertyBuilder<IConfigurationData> propertyBuilder,
        JsonSerializerOptions? jsonSerializerOptions = null)
    {
        propertyBuilder.HasConversion(
            new ConfigurationDataConverter(
                jsonSerializerOptions ?? DefaultJsonSerializer.Options));

        return propertyBuilder;
    }

}

