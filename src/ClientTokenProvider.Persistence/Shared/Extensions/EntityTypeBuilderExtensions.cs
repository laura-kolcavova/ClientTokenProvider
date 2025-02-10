using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClientTokenProvider.Persistence.Shared.Extensions;

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
}

