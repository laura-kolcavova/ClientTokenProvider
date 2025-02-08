using ClientTokenProvider.Core.AzureAd.Dto;
using ClientTokenProvider.Core.AzureAd.Exceptions;
using ClientTokenProvider.Core.AzureAd.Models;
using ClientTokenProvider.Core.AzureAd.Services;
using FluentAssertions;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using Xunit.Categories;
using XunitTests.Shared;

namespace XunitTests.Application.AzureAd;

[Category("unit")]
[Category("coverage")]
public sealed class AzureAdClientHandlerTests
{
    [Fact]
    public async Task GetClientToken_ShouldReturnResponse_WhenStatusCodeIsOk()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var scope = "test_scope";
        var expectedToken = "test_token";

        var response = new GetClientTokenResponse
        {
            TokenType = "Bearer",
            ExpiresIn = 3599,
            ExtExpiresIn = 3599,
            AccessToken = "test_token"
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(response)
        };

        var fakeHttpMessageHandler = new FakeHttpMessageHandler(responseMessage);

        var azureAdClientHandler = CreateAzureAdClientHandler(fakeHttpMessageHandler);

        // Act
        var result = await azureAdClientHandler
            .GetClientToken(scope, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedToken);
    }

    [Fact]
    public async Task GetClientToken_ShouldThrowClientHandlerException_WhenStatusCodeIsNotOk()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var scope = "test_scope";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Bad Request"),
        };

        var fakeHttpMessageHandler = new FakeHttpMessageHandler(responseMessage);

        var azureAdClientHandler = CreateAzureAdClientHandler(fakeHttpMessageHandler);

        // Act
        var act = async () =>
            await azureAdClientHandler.GetClientToken(scope, cancellationToken);

        // Assert
        await act
            .Should()
            .ThrowAsync<ClientHandlerException>();
    }

    [Fact]
    public async Task GetClientToken_ShouldThrowClientHandlerException_WhenResponseIsNull()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var scope = "test_scope";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = null
        };

        var fakeHttpMessageHandler = new FakeHttpMessageHandler(responseMessage);

        var azureAdClientHandler = CreateAzureAdClientHandler(fakeHttpMessageHandler);

        // Act
        var act = async () =>
            await azureAdClientHandler.GetClientToken(scope, cancellationToken);

        // Assert
        await act
            .Should()
            .ThrowAsync<ClientHandlerException>();
    }

    [Fact]
    public async Task GetClientToken_ShouldThrowClientHandlerException_WhenDeserializationFails()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var scope = "test_scope";

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Invalid JSON") // Simulate invalid JSON
        };

        var fakeHttpMessageHandler = new FakeHttpMessageHandler(responseMessage);

        var azureAdClientHandler = CreateAzureAdClientHandler(fakeHttpMessageHandler);

        // Act
        var act = async () =>
            await azureAdClientHandler.GetClientToken(scope, cancellationToken);

        // Assert
        await act
            .Should()
            .ThrowAsync<ClientHandlerException>();
    }

    private static ClientTokenProviderConfiguration CreateClientConfiguration()
    {
        return new ClientTokenProviderConfiguration
        {
            AuthorityUri = "https://example.com",
            Audience = "test_audience",
            ClientId = "test_client_id",
            ClientSecret = "test_client_secret"
        };
    }

    private static AzureAdClientHandler CreateAzureAdClientHandler(
        FakeHttpMessageHandler fakeHttpMessageHandler)
    {
        var clientConfiguration = CreateClientConfiguration();

        var mockHttpClientFactory = Substitute.For<IHttpClientFactory>();

        var httpClient = new HttpClient(fakeHttpMessageHandler);

        mockHttpClientFactory
            .CreateClient()
            .Returns(httpClient);

        return new AzureAdClientHandler(mockHttpClientFactory, clientConfiguration);
    }
}