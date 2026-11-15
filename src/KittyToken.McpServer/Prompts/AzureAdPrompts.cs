using ModelContextProtocol.Server;
using System.ComponentModel;

namespace KittyToken.McpServer.Prompts;

[McpServerPromptType]
internal static class AzureAdPrompts
{
    [
        McpServerPrompt,
        Description("Gets an encoded access token with the client credentials flow, ready to use in an API call.")]
    public static string GetEncodedToken(
        [Description("Azure AD instance, e.g. https://login.microsoftonline.com/")]
        string instance,
        [Description("Directory (tenant) ID of the app registration")]
        string tenantId,
        [Description("Application (client) ID of the app registration")]
        string clientId,
        [Description("Client secret of the app registration")]
        string clientSecret,
        [Description("Resource scope ending with /.default, e.g. api://<app-id>/.default")]
        string scope)
    {
        return $"""
            Get an Azure AD access token:

            1. Get a token with get_access_token using instance "{instance}", tenantId "{tenantId}", clientId "{clientId}", clientSecret "{clientSecret}" and scope "{scope}".
            2. Print only the encoded token in a code block, so I can copy it. No summary.
            3. If I then ask for the decoded token, decode it with decode_jwt and show the decoded header and payload as JSON.
            """;
    }

    [
        McpServerPrompt,
        Description("Gets an encoded access token using the Azure AD configuration of the current project, ready to use in an API call.")]
    public static string GetEncodedTokenFromProject()
    {
        return """
            Get an Azure AD access token using the Azure AD configuration of the current project:

            1. Find the Azure AD client credentials (Instance, TenantId, ClientId, ClientSecret, Scope) in the project, e.g. in appsettings*.json and in user secrets (dotnet user-secrets list --project <csproj>). If you find more candidates (projects, environments or sections), list them and ask me which one to use. If a value is missing, ask me for it.
            2. Get a token with get_access_token.
            3. Print only the encoded token in a code block, so I can copy it. No summary.
            4. If I then ask for the decoded token, decode it with decode_jwt and show the decoded header and payload as JSON.
            """;
    }

    [
        McpServerPrompt,
        Description("Gets an encoded access token for a saved Azure AD configuration, ready to use in an API call.")]
    public static string GetEncodedTokenByConfiguration(
        [Description("Name of the saved configuration")]
        string configurationName)
    {
        return $"""
            Get an Azure AD access token for the saved configuration "{configurationName}":

            1. Find the configuration with get_configurations. If no name matches exactly or more names match, show me the closest names and stop.
            2. Get a token with get_access_token_by_configuration.
            3. Print only the encoded token in a code block, so I can copy it. No summary.
            4. If I then ask for the decoded token, decode it with decode_jwt and show the decoded header and payload as JSON.
            """;
    }
}
