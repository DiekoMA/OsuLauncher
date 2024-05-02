using OsuLauncher.ViewModels;

namespace OsuLauncher.Services;

public interface INavigationService
{
    ViewModelBase CurrentView { get; }
    void NavigateTo<T>() where T : ViewModelBase;
}

public partial class NavigationService : ObservableObject, INavigationService
{
    [ObservableProperty] private ViewModelBase _currentView;
    private readonly Func<Type, ViewModelBase> _viewModelFactory;

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }
    
    public void NavigateTo<TViewModelBase>() where TViewModelBase : ViewModelBase
    {
        ViewModelBase viewModel = _viewModelFactory.Invoke(typeof(TViewModelBase));
        CurrentView = viewModel;
    }
}