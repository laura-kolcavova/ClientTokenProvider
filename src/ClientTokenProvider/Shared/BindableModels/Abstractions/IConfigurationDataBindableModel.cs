namespace ClientTokenProvider.Shared.BindableModels.Abstractions;

public interface IConfigurationDataBindableModel
{
    public IEnumerable<object?> GetDataComponents();

    public IConfigurationDataBindableModel Copy();

    public bool AreDataValid();
}
