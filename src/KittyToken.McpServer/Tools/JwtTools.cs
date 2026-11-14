using KittyToken.Application.Shared.JwtDecoding;
using KittyToken.Application.Shared.JwtDecoding.Abstractions;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace KittyToken.McpServer.Tools;

[McpServerToolType]
internal sealed class JwtTools(
    IJwtDecoder jwtDecoder)
{
    [
        McpServerTool,
        Description("Decodes a JWT into its header and payload JSON without validating the signature.")]
    public DecodedJwt DecodeJwt(
        [Description("The encoded JWT, e.g. an access token")]
        string jwtToken)
    {
        return jwtDecoder.Decode(jwtToken);
    }
}
