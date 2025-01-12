namespace OsuLauncher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly string _clientID;
    private readonly string _clientSecret;
    private OsuMemoryReader _osuMemoryReader;
    private OsuClient osuClient;
    [ObservableProperty] private UserControl _currentPage;
    [ObservableProperty] private string _selectedStartupMode;
    [ObservableProperty] private bool _canLaunch;
    [ObservableProperty] private ViewModelBase _currentViewModel;
    [ObservableProperty] private INavigationService _navigation;
    [ObservableProperty] private IOsuService _osu;
    [ObservableProperty] private User? _authedUser;

    public MainWindowViewModel()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json");
        using var reader = new StreamReader(stream!);
        var config = JsonSerializer.Deserialize<SecretsConfiguration>(reader.ReadToEnd(), new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
        if (!Directory.Exists("configs"))
        {
            Directory.CreateDirectory("configs");
            ObservableCollection<OsuSettingsPreset> presets = new ObservableCollection<OsuSettingsPreset>();
            ObservableCollection<OsuLauncherProfile> profiles = new ObservableCollection<OsuLauncherProfile>();
            var presetsJson = JsonSerializer.Serialize(presets);
            var profilesJson = JsonSerializer.Serialize(profiles);
            File.WriteAllText("configs//presets.json", presetsJson);
            File.WriteAllText("configs//profiles.json", profilesJson);
        }
        _clientID = config!.ClientId;
        _clientSecret = config.ClientSecret;
        var osuProcesses = Process.GetProcessesByName("osu!");
        CanLaunch = !string.IsNullOrEmpty(AppSettings.Default.GameDirectory) && osuProcesses.Length == 0;
        osuClient = new OsuClient();
        //AuthUser();
    }

    private async void AuthUser() => AuthedUser = await ApiHelper.Instance.RetrieveClient().Result.GetAuthenticatedUserAsync();

    [RelayCommand]
    public void NavigateToHome() => CurrentPage = new HomeView();
    [RelayCommand] public void NavigateToBeatmaps() => CurrentPage = new BeatmapsView();
    [RelayCommand] public void NavigateToCollections() => CurrentPage = new CollectionsView();

    [RelayCommand] public void NavigateToSettings() => CurrentPage = new SettingsView();

    [RelayCommand]
    public void ShowNotifications()
    {
        
    }

    [RelayCommand]
    public void NavigateToAccount()
    {
        
    }


    [RelayCommand]
    public void LaunchOsu()
    {
        
    }

    [RelayCommand]
    public void RunOsuRepair()
    {
        var repairProcessInfo = new ProcessStartInfo()
        {
            FileName = Path.Combine(AppSettings.Default.GameDirectory, "osu!.exe"),
            Arguments = "-config"
        };
        repairProcessInfo.UseShellExecute = true;
        Process.Start(repairProcessInfo);
    }

    [RelayCommand]
    public void RunOsuUpdate()
    {
        // Do Later
    }

    [RelayCommand]
    public void RunMcOsuClient()
    {
        ProcessStartInfo mcOsuStartInfo = new ProcessStartInfo();
        mcOsuStartInfo.UseShellExecute = true;
        mcOsuStartInfo.FileName = Path.Combine(AppSettings.Default.TrainingClientDirectory, "McOsu.exe");
        Process.Start(mcOsuStartInfo);
    }
}