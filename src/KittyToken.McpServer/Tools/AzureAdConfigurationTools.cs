using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Exceptions;
using KittyToken.Application.AzureAd.Models;
using KittyToken.Application.AzureAd.Services.Abstractions;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace KittyToken.McpServer.Tools;

[McpServerToolType]
internal sealed class AzureAdConfigurationTools(
    IAzureAdFileStore azureAdFileStore,
    IAzureAdFileFactory azureAdFileFactory,
    IAzureAdClient azureAdClient)
{
    [
        McpServerTool,
        Description("Gets all stored Azure AD configurations.")]
    public async Task<IReadOnlyCollection<AzureAdFile>> GetConfigurations(
        CancellationToken cancellationToken)
    {
        return await azureAdFileStore.GetAll(cancellationToken);
    }

    [
        McpServerTool,
        Description("Creates a new Azure AD configuration, stores it in the database and returns it including its generated ID.")]
    public async Task<AzureAdFile> CreateConfiguration(
        [Description("Name of the configuration, must not be empty")]
        string name,
        [Description("Azure AD instance, e.g. https://login.microsoftonline.com/")]
        string instance,
        [Description("Directory (tenant) ID of the app registration")]
        string tenantId,
        [Description("Application (client) ID of the app registration")]
        string clientId,
        [Description("Client secret of the app registration")]
        string clientSecret,
        [Description("Resource scope ending with /.default, e.g. api://<app-id>/.default")]
        string scope,
        CancellationToken cancellationToken)
    {
        ThrowIfNameIsEmpty(name);

        var configuration = new AzureAdConfiguration
        {
            Instance = instance,
            TenantId = tenantId,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope,
        };

        var azureAdFile = azureAdFileFactory.Create(
            name,
            configuration);

        await azureAdFileStore.Add(
            azureAdFile,
            cancellationToken);

        await azureAdFileStore.SaveChanges(cancellationToken);

        return azureAdFile;
    }

    [
        McpServerTool,
        Description("Updates (overwrites) the values of an existing Azure AD configuration stored in the database.")]
    public async Task<AzureAdFile> UpdateConfiguration(
        [Description("ID of the configuration")]
        Guid configurationId,
        [Description("Azure AD instance, e.g. https://login.microsoftonline.com/")]
        string instance,
        [Description("Directory (tenant) ID of the app registration")]
        string tenantId,
        [Description("Application (client) ID of the app registration")]
        string clientId,
        [Description("Client secret of the app registration")]
        string clientSecret,
        [Description("Resource scope ending with /.default, e.g. api://<app-id>/.default")]
        string scope,
        CancellationToken cancellationToken)
    {
        var azureAdFile = await GetExistingFile(
            configurationId,
            cancellationToken);

        var configuration = new AzureAdConfiguration
        {
            Instance = instance,
            TenantId = tenantId,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope,
        };

        azureAdFile.UpdateConfiguration(configuration);

        await azureAdFileStore.SaveChanges(cancellationToken);

        return azureAdFile;
    }

    [
        McpServerTool,
        Description("Renames an existing Azure AD configuration.")]
    public async Task<AzureAdFile> RenameConfiguration(
        [Description("ID of the configuration")]
        Guid configurationId,
        [Description("New name of the configuration, must not be empty")]
        string name,
        CancellationToken cancellationToken)
    {
        ThrowIfNameIsEmpty(name);

        var azureAdFile = await GetExistingFile(
            configurationId,
            cancellationToken);

        azureAdFile.UpdateName(name);

        await azureAdFileStore.SaveChanges(cancellationToken);

        return azureAdFile;
    }

    [
        McpServerTool,
        Description("Deletes an existing Azure AD configuration.")]
    public async Task DeleteConfiguration(
        [Description("ID of the configuration")]
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        var azureAdFile = await GetExistingFile(
            configurationId,
            cancellationToken);

        azureAdFileStore.Remove(azureAdFile);

        await azureAdFileStore.SaveChanges(cancellationToken);
    }

    [
        McpServerTool,
        Description("Gets an Azure AD access token using the client credentials flow and a stored Azure AD configuration.")]
    public async Task<string> GetAccessTokenByConfiguration(
        [Description("ID of the configuration")]
        Guid configurationId,
        CancellationToken cancellationToken)
    {
        var azureAdFile = await GetExistingFile(
            configurationId,
            cancellationToken);

        var configuration = azureAdFile.Configuration;

        var requestDto = new GetClientTokenRequestDto
        {
            Instance = configuration.Instance,
            TenantId = configuration.TenantId,
            ClientId = configuration.ClientId,
            ClientSecret = configuration.ClientSecret,
            Scope = configuration.Scope,
        };

        try
        {
            var accessToken = await azureAdClient.GetAccessToken(
                requestDto,
                cancellationToken);

            return accessToken;
        }
        catch (AzureAdClientException ex)
        {
            //  The C# MCP SDK only sends the exception message to the client when the tool throws an McpException:
            throw new McpException(ex.Message, ex);
        }
        catch (HttpRequestException ex)
        {
            throw new McpException($"Azure AD request failed: {ex.Message}", ex);
        }
    }

    private async Task<AzureAdFile> GetExistingFile(
        Guid id,
        CancellationToken cancellationToken)
    {
        var azureAdFile = await azureAdFileStore.GetById(
            id,
            cancellationToken);

        return azureAdFile ??
            throw new McpException($"Configuration '{id}' not found.");
    }

    private static void ThrowIfNameIsEmpty(
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new McpException("Name must not be empty.");
        }
    }
}
