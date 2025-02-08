using ClientTokenProvider.Business.Shared.Models;

namespace ClientTokenProvider.Business.Shared.Services.Abstractions;

public interface IJwtDecoder
{
    public DecodedJwt Decode(
        string jwtToken);
}
