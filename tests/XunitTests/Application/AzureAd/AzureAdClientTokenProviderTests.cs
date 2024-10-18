using ClientTokenProvider.Application.AzureAd;
using ClientTokenProvider.Application.AzureAd.Dto;
using ClientTokenProvider.Application.AzureAd.Exceptions;
using ClientTokenProvider.Application.AzureAd.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using System.Reflection;
using Xunit.Categories;

namespace XunitTests.Application.AzureAd;

[Category("unit")]
[Category("coverage")]
public class AzureAdClientTokenProviderTests
{
    [Fact]
    public async Task GetAccessToken_ShouldReturnAccessToken_WhenSuccess()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var scope = "test_scope";

        var response = new GetClientTokenResponse
        {
            AccessToken = "test_access_token"
        };

        var mockAzureAdClientHandler = Substitute.For<IAzureAdClientHandler>();

        mockAzureAdClientHandler
            .GetClientToken(scope, Arg.Any<CancellationToken>())
            .Returns(response);

        var azureAdClientTokenProvider = CreateAzureAdClientTokenProvider(
            mockAzureAdClientHandler);

        // Act
        var result = await azureAdClientTokenProvider
            .GetAccessToken(scope, cancellationToken);

        // Assert
        result
            .Should()
            .Be("test_access_token");
    }

    [Fact]
    public async Task GetAccessToken_ShouldThrowException_WhenHandlerThrowsClientHandlerException()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var scope = "test_scope";

        var response = new GetClientTokenResponse
        {
            AccessToken = "test_access_token"
        };

        var exception = new ClientHandlerException(
            "Error getting token",
            (int)HttpStatusCode.InternalServerError,
            string.Empty,
            new Dictionary<string, IEnumerable<string>>(),
            null);

        var mockAzureAdClientHandler = Substitute.For<IAzureAdClientHandler>();

        mockAzureAdClientHandler
            .GetClientToken(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Throws(exception);

        var azureAdClientTokenProvider = CreateAzureAdClientTokenProvider(
            mockAzureAdClientHandler);

        // Act
        var act = async () =>
            await azureAdClientTokenProvider.GetAccessToken(scope, cancellationToken);

        // Assert
        await act
            .Should()
            .ThrowAsync<ClientHandlerException>();
    }

    private static ClientConfiguration CreateClientConfiguration()
    {
        return new ClientConfiguration
        {
            AuthorityUri = "https://example.com",
            Scope = "test_scope",
            Audience = "test_audience",
            ClientId = "test_client_id",
            ClientSecret = "test_client_secret"
        };
    }

    private static AzureAdClientTokenProvider CreateAzureAdClientTokenProvider(
        IAzureAdClientHandler azureAdClientHandler)
    {
        var clientConfiguration = CreateClientConfiguration();

        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();

        var azureAdClientTokenProvider = new AzureAdClientTokenProvider(
            mockHttpClientFactory,
            clientConfiguration);

        typeof(AzureAdClientTokenProvider)
           .GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance)?
           .SetValue(azureAdClientTokenProvider, azureAdClientHandler);

        return azureAdClientTokenProvider;
    }
}