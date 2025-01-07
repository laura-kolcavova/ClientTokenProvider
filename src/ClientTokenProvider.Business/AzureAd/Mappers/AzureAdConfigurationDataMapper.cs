using ClientTokenProvider.Business.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Models;
using Riok.Mapperly.Abstractions;

namespace ClientTokenProvider.Business.AzureAd.Mappers;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
public static partial class AzureAdConfigurationDataMapper
{
    [MapProperty(
        nameof(AzureAdConfigurationData.AuthorityUrl),
        nameof(ClientTokenProviderConfiguration.AuthorityUri))]
    [MapperIgnoreSource(
        nameof(AzureAdConfigurationData.Scope))]
    public static partial ClientTokenProviderConfiguration ToClientConfiguration(
        this AzureAdConfigurationData source);
}
