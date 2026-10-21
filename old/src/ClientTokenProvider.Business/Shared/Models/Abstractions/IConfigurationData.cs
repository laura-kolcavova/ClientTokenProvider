using ClientTokenProvider.Core.AzureAd.Models;

namespace ClientTokenProvider.Business.Shared.Models.Abstractions;

//[JsonDerivedType(
//    typeof(AzureAdConfigurationData),
//    typeDiscriminator: nameof(AzureAdConfigurationData))]
public interface IConfigurationData
{
    public string Scope { get; }

    public IAzureAdClientTokenProviderConfiguration ToClientTokenProviderConfiguration();
}
