namespace KittyToken.Application.Shared.JwtDecoding;

public sealed record DecodedJwt
{
    public required string Header { get; init; }

    public required string Payload { get; init; }

    public required string Signature { get; init; }
}
