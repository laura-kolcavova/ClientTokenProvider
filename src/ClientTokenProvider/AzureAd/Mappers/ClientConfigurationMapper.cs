using ClientTokenProvider.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Models;
using Riok.Mapperly.Abstractions;

namespace ClientTokenProvider.AzureAd.Mappers;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName, EnumMappingIgnoreCase = true)]
internal static partial class ClientConfigurationMapper
{
    [MapProperty(
        nameof(ClientConfigurationModel.AuthorityUrl),
        nameof(ClientConfiguration.AuthorityUri))]
    [MapperIgnoreSource(
        nameof(ClientConfigurationModel.Scope))]
    public static partial ClientConfiguration ToClientConfiguration(
        this ClientConfigurationModel source);
}
