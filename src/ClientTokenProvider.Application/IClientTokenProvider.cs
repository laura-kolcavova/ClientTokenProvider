namespace ClientTokenProvider.Application;

public interface IClientTokenProvider
{
    public Task<string> GetAccessToken(
        string scope,
        CancellationToken cancellationToken);
}
