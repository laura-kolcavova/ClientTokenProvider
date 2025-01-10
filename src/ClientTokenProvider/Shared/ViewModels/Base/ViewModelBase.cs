using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClientTokenProvider.Shared.ViewModels.Base;

public abstract partial class ViewModelBase : ObservableObject
{
    protected ViewModelBase() { }

    [RelayCommand]
    protected abstract Task Load(CancellationToken cancellationToken);

    [RelayCommand]
    protected abstract Task Unload(CancellationToken cancellationToken);
}
