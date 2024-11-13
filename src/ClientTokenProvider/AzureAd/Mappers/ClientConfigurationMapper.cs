using ClientTokenProvider.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Models;
using Riok.Mapperly.Abstractions;

namespace ClientTokenProvider.AzureAd.Mappers;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
internal static partial class ClientConfigurationMapper
{
    [MapProperty(
        nameof(AzureAdConfigurationModel.AuthorityUrl),
        nameof(ClientTokenProviderConfiguration.AuthorityUri))]
    [MapperIgnoreSource(
        nameof(AzureAdConfigurationModel.Scope))]
    public static partial ClientTokenProviderConfiguration ToClientConfiguration(
        this AzureAdConfigurationModel source);
}
