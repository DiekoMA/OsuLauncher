using CommunityToolkit.Mvvm.Input;
using OsuLauncher.Services;

namespace OsuLauncher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly string _clientID;
    private readonly string _clientSecret;
    private OsuMemoryReader _osuMemoryReader;
    [ObservableProperty] private string _selectedStartupMode;
    [ObservableProperty] private bool _canLaunch;
    [ObservableProperty] private ViewModelBase _currentViewModel;
    [ObservableProperty] private INavigationService _navigation;
    public MainWindowViewModel(INavigationService navService)
    {
        Navigation = navService;
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json");
        using var reader = new StreamReader(stream!);
        var config = System.Text.Json.JsonSerializer.Deserialize<SecretsConfiguration>(reader.ReadToEnd(), new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
        _clientID = config!.ClientId;
        _clientSecret = config.ClientSecret;
        CanLaunch = !string.IsNullOrEmpty(AppSettings.Default.GameDirectory);
    }

    [RelayCommand] public void NavigateToHome() => Navigation.NavigateTo<HomeViewModel>();
    [RelayCommand] public void NavigateToBeatmaps() => Navigation.NavigateTo<BeatmapViewModel>();
    [RelayCommand] public void NavigateToCollections() => Navigation.NavigateTo<CollectionsViewModel>();
    [RelayCommand] public void NavigateToSettings() => Navigation.NavigateTo<SettingsViewModel>();
}