using ClientTokenProvider.Business.Shared.Models;
using ClientTokenProvider.Persistence.AzureAd.Extensions;
using ClientTokenProvider.Persistence.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientTokenProvider.Persistence.Shared.EntityTypeConfigurations;

internal sealed class ConfigurationEntityTypeConfiguration :
    IEntityTypeConfiguration<ConfigurationModel>
{
    public void Configure(EntityTypeBuilder<ConfigurationModel> builder)
    {
        builder.ToTable(
            DatabaseConstants.ConfigurationsTableName,
            DatabaseConstants.DefaultSchemaName);

        builder.HasKey(configuration => configuration.Id);

        builder
            .Property(configuration => configuration.Id)
            .HasColumnType("TEXT")
            .IsRequired();

        builder
            .Property(configuration => configuration.Name)
            .HasColumnType("TEXT")
            .IsRequired();

        builder
            .Property(configuration => configuration.Kind)
            .HasColumnType("TEXT")
            .HasEnumToStringConversion()
            .IsRequired();

        builder
            .Property(configuration => configuration.Data)
            .HasColumnType("TEXT")
            .HasAzureAdConfigurationDataConversion()
            .IsRequired();
    }
}
