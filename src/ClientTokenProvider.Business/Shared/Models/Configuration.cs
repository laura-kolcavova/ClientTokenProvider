namespace ClientTokenProvider.Business.Shared.Models;

public abstract record ConfigurationBase
{
    public required ConfigurationIdentity Identity { get; init; }
}

public sealed record Configuration<TConfigurationDataType>
    : ConfigurationBase
    where TConfigurationDataType : notnull
{
    public required TConfigurationDataType Data { get; init; }
}


