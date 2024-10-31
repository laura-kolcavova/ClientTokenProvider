namespace ClientTokenProvider.Core.Shared;

public interface IClientTokenProvider
{
    public Task<string> GetAccessToken(
        string scope,
        CancellationToken cancellationToken);
}
