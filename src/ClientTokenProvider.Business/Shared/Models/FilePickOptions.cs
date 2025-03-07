namespace ClientTokenProvider.Business.Shared.Models;

public sealed class FilePickOptions
{
    public string? Title { get; init; }

    public required IEnumerable<string> FileExtensions { get; init; }
}
