namespace KittyToken.Application.Shared.JwtDecoding.Abstractions;

public interface IJwtDecoder
{
    public DecodedJwt Decode(
      string jwtToken);
}
