using KittyToken.Application.AzureAd.Dto;
using KittyToken.Application.AzureAd.Services.Abstractions;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace KittyToken.McpServer.Tools;

[McpServerToolType]
internal sealed class AzureAdClientTools(IAzureAdClient azureAdClient)
{
    [McpServerTool, Description("Gets an Azure AD access token using the client credentials flow.")]
    public Task<string> GetAccessToken(
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
        var requestDto = new GetClientTokenRequestDto
        {
            Instance = instance,
            TenantId = tenantId,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scope,
        };

        return azureAdClient.GetAccessToken(
            requestDto,
            cancellationToken);
    }
}
