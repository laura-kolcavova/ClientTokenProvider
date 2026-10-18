using KittyToken.Application.AzureAd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KittyToken.Application.AzureAd.Persistence;

internal sealed class AzureAdFileEntityConfiguration :
    IEntityTypeConfiguration<AzureAdFile>
{
    public const string TableName = "Configurations";

    public const string SchemaName = "dbo";

    public void Configure(
        EntityTypeBuilder<AzureAdFile> builder)
    {
        builder.ToTable(
            TableName,
            SchemaName);

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
            .Property(configuration => configuration.Configuration)
            .HasColumnType("TEXT")
            .HasConversion(
                new AzureAdConfigurationValueConverter())
            .IsRequired();
    }
}