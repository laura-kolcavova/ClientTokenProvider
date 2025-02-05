namespace ClientTokenProvider.Core.Shared.Abstractions;

public interface IClientTokenProvider
{
    public Task<string> GetAccessToken(
        string scope,
        CancellationToken cancellationToken);
}
