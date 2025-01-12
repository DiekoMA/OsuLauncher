using DotNetConfig;
using HandyControl.Tools;

namespace OsuLauncher.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    #region Generic
    private IConfigurationRoot config;
    private OsuConfigHelper _configHelper;
    private Config appConfig;
    private string clientId;
    private readonly string clientSecret;
    private readonly string redirectUrl;
    
    [ObservableProperty] private bool _loggedIn;
    #endregion

    [ObservableProperty] private User _authedUser;
    
    #region AppSettings
    private string? _gameDirectory;

    public string GameDirectory
    {
        get => _gameDirectory;
        set
        {
            _gameDirectory = value;
            appConfig.SetString("appsettings", "gameDirectory", value);
            OnPropertyChanged();
        }
    }
    private string? _songsDirectory;

    public string SongsDirectory
    {
        get => _songsDirectory;
        set
        {
            _songsDirectory = value;
            appConfig.SetString("appsettings", "songsDirectory", value);
            OnPropertyChanged();
        }
    }
    private string? _trainingClientDirectory;

    public string TrainingClientDirectory
    {
        get => _trainingClientDirectory;
        set
        {
            _trainingClientDirectory = value;
            appConfig.SetString("appsettings", "trainingClientDirectory", value);
            OnPropertyChanged();
        }
    }
    [ObservableProperty] private string _currentTheme;
    private bool _autoUpdate;

    public bool AutoUpdate
    {
        get => _autoUpdate;
        set
        {
            _autoUpdate = value;
            appConfig.SetBoolean("appsettings", "autoUpdate", value);
            OnPropertyChanged();
        }
    }
    private bool _offlineMode;

    public bool OfflineMode
    {
        get => _offlineMode;
        set
        {
            _offlineMode = value;
            appConfig.SetBoolean("appsettings", "offlineMode", value);
            OnPropertyChanged();
        }
    }
    private bool _beatmapOpt;

    public bool BeatmapOpt
    {
        get => _beatmapOpt;
        set
        {
            _beatmapOpt = value;
            appConfig.SetBoolean("appsettings", "beatmapOpt", value);
            OnPropertyChanged();
        }
    }
    #endregion

    #region GameSettings

    [ObservableProperty] private string _keyOsuLeft;
    [ObservableProperty] private int _musicVolume;
    [ObservableProperty] private int _masterVolume;
    [ObservableProperty] private int _effectsVolume;
    [ObservableProperty] private float _mouseSensitivity; 
    [ObservableProperty] private bool _mouseDisableWheel;
    [ObservableProperty] private bool _mouseDisableButtons;
    [ObservableProperty] private  bool _showFPS;
    [ObservableProperty] private bool _fullScreen;
    [ObservableProperty] private bool _richPresence;
    [ObservableProperty] private OsuSettingsPreset _currentOsuSettingsPreset;
    [ObservableProperty] private ObservableCollection<OsuSettingsPreset> _osuSettingsPresets;
    [ObservableProperty] private List<string> _skins;
    [ObservableProperty] private IOsuService _osu;
    #endregion

    private async void Auth()
    {
        AuthedUser = await Osu.Client.GetAuthenticatedUserAsync() ?? throw new InvalidOperationException();
    }
    
    public SettingsViewModel(/*IOsuService osuClient*/)
    {
        //Osu = osuClient;
        //Auth();
        appConfig = Config.Build();
        try
        {
            var osuCfg = Path.Combine(appConfig.GetString("appsettings", "gameDirectory") ?? string.Empty,
                $"osu!.{Environment.UserName}.cfg");
            _configHelper = new OsuConfigHelper(osuCfg);
            //_loggedIn = true;
            /*Skins = Directory.GetDirectories(Path.Combine(AppSettings.Default.GameDirectory, "Skins")).ToList() ??
                    new List<string>();*/
            AutoUpdate = appConfig.GetBoolean("appsettings", "autoUpdate") ?? false;
            OfflineMode = appConfig.GetBoolean("appsettings", "offlineMode") ?? false;
            BeatmapOpt = appConfig.GetBoolean("appsettings", "beatmapOpt") ?? false;
            GameDirectory = appConfig.GetString("appsettings", "gameDirectory") ?? string.Empty;
            SongsDirectory = appConfig.GetString("appsettings", "songsDirectory") ?? string.Empty;
            TrainingClientDirectory = appConfig.GetString("appsettings", "trainingClientDirectory") ?? string.Empty;
            EffectsVolume = _configHelper.ReadInt("VolumeEffect"); 
            MouseSensitivity = _configHelper.ReadFloat("MouseSpeed");
            MouseDisableWheel = _configHelper.ReadInt("MouseDisableWheel") != 0;
            MouseDisableButtons = _configHelper.ReadInt("MouseDisableButtons") != 0;
            RichPresence = _configHelper.ReadInt("DiscordRichPresence") != 0;
            ShowFPS = _configHelper.ReadInt("FpsCounter") != 0;
            FullScreen = _configHelper.ReadInt("Fullscreen") != 0;
            MasterVolume = _configHelper.ReadInt("VolumeUniversal");
            KeyOsuLeft = _configHelper.ReadString("keyOsuLeft");
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json");
            using StreamReader sr = new StreamReader(stream);
            var secretsConfig = JsonSerializer.Deserialize<SecretsConfiguration>(sr.ReadToEnd());
            clientId = secretsConfig!.ClientId;
            clientSecret = secretsConfig!.ClientSecret;
            redirectUrl = secretsConfig!.RedirectUrl;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    #region GameValueCommands
    [RelayCommand] private void EnableMouseWheel() => _configHelper.EditValue("MouseDisableWheel", "1");
    [RelayCommand] private void DisableMouseWheel() => _configHelper.EditValue("MouseDisableWheel", "0");
    
    [RelayCommand] private void EnableMouseButton() => _configHelper.EditValue("MouseDisableButtons", "1");
    [RelayCommand] private void DisableMouseButton() => _configHelper.EditValue("MouseDisableButtons", "0");
    [RelayCommand] private void EnableRichPresence() => _configHelper.EditValue("DiscordRichPresence", "1");
    [RelayCommand] private void DisableRichPresence() => _configHelper.EditValue("DiscordRichPresence", "0");
    
    [RelayCommand] private void EnableFpsCounter() => _configHelper.EditValue("FpsCounter", "1");
    [RelayCommand] private void DisableFpsCounter() => _configHelper.EditValue("FpsCounter", "0");
    
    [RelayCommand] private void EnableFullscreen() => _configHelper.EditValue("Fullscreen", "1");
    [RelayCommand] private void DisableFullscreen() => _configHelper.EditValue("Fullscreen", "0");
    
    [RelayCommand] private void AdjustMasterVolume() => _configHelper.EditValue("VolumeUniversal", CurrentOsuSettingsPreset.MasterVolume.ToString());
    [RelayCommand] private void AdjustEffectsVolume() => _configHelper.EditValue("VolumeEffect", CurrentOsuSettingsPreset.EffectsVolume.ToString());
    [RelayCommand] private void AdjustMusicVolume() => _configHelper.EditValue("VolumeMusic", CurrentOsuSettingsPreset.EffectsVolume.ToString());
    
    [RelayCommand] private void EnableAutoUpdate() {LauncherSettings.Default.CheckForUpdates = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableAutoUpdate() {LauncherSettings.Default.CheckForUpdates = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void EnableOfflineMode() {LauncherSettings.Default.OfflineStartup = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableOfflineMode() {LauncherSettings.Default.OfflineStartup = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void EnableBeatmapOpt() {LauncherSettings.Default.BeatmapOpt = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableBeatmapOpt() {LauncherSettings.Default.BeatmapOpt = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void ChangeSensitivity() => _configHelper.EditValue("MouseSpeed", MouseSensitivity.ToString());

    [RelayCommand]
    private void BindLeftClickOsu()
    {
        var newKey = Console.ReadKey(true);
        _configHelper.EditValue("keyOsuLeft", newKey.Key.ToString());
    }
    #endregion

    [RelayCommand]
    public void SetTheme()
    {
        switch (CurrentTheme)
        {
            case "System":
                break;
            
            case "Light":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                break;
            
            case "Dark":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                break;
            
            case "Nord":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                ThemeManager.Current.AccentColor = Brushes.Navy;
                break;
            
            case "Osu":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                ThemeManager.Current.AccentColor = Brushes.HotPink;
                break;
            
            case "Yotsuba":
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                ThemeManager.Current.AccentColor = Brushes.Orange;
                break;
        }
    }

    [RelayCommand]
    private void ShowView()
    {
        
    }

    [RelayCommand]
    private Task CreateNewPreset()
    {
        if (OsuSettingsPresets != null)
        {
            var newPreset = new OsuSettingsPreset()
            {
                Title = "Test"
            };
            CurrentOsuSettingsPreset = newPreset;
            OsuSettingsPresets.Add(newPreset);   
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RemoveCurrentPreset()
    {
        OsuSettingsPresets.Remove(CurrentOsuSettingsPreset);
        return Task.CompletedTask;
    }
    
    [RelayCommand]
    public async Task InitiateAuth()
    {
        var scope = "public identify friends.read";
        var state = GenerateRandomString();
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName =
                $"https://osu.ppy.sh/oauth/authorize?client_id={clientId}&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify+friends.read&state={state}"
        };
        Process.Start(startInfo);
        using var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:7040/callback/");
        listener.Start();
        
        var context = listener.GetContext();
        string responseText = "Authentication complete";
        byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseText);
        context.Response.ContentLength64 = responseBytes.Length;
        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
        var code = context.Request.QueryString["code"];
        context.Response.Close();
        await InitiateTokenSwap(code!);
    }

    private string GenerateRandomString()
    {
        using (var randomNumberGenerator = new RNGCryptoServiceProvider())
        {
            //  2. Allocate an array to hold the random bytes
            var randomBytes = new byte[12];

            //  3. Generate random bytes
            randomNumberGenerator.GetBytes(randomBytes);

            //  4. Convert random bytes to a string using Base64 encoding (URL safe)
            return Convert.ToBase64String(randomBytes);
        }
    }
    
    private async Task InitiateTokenSwap(string code)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://osu.ppy.sh/oauth/token"),
            Headers =
            {
                { "Accept", "application/json" },
            },
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl }
            })
        };

        using var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to swap osu! API tokens.");
        }

        response.EnsureSuccessStatusCode();

        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        }) ?? throw new InvalidOperationException();

        var tokenJson = JsonSerializer.Serialize<TokenResponse>(tokenResponse, new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            WriteIndented = true
        });

        await File.WriteAllTextAsync("token.secret", tokenJson);
        //Osu.Client = await ApiHelper.Instance.RetrieveClient();
        
        //AuthedUser = await Osu.Client.GetAuthenticatedUserAsync() ?? throw new InvalidOperationException();

        //LoggedIn = Osu.Client.IsAuthenticated;
    }
}