namespace ClientTokenProvider.Core.Shared.Providers;

public interface IClientTokenProvider
{
    public Task<string> GetAccessToken(
        string scope,
        CancellationToken cancellationToken);
}
