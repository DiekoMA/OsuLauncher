using DotNetConfig;
using HandyControl.Tools;
using OsuLauncher.Controls;

namespace OsuLauncher.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    #region Generic
    private IConfigurationRoot config;
    private OsuConfigHelper _configHelper;
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
            AppUtils.AppConfig.SetString("appsettings", "gameDirectory", value);
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
            AppUtils.AppConfig.SetString("appsettings", "songsDirectory", value);
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
            AppUtils.AppConfig.SetString("appsettings", "trainingClientDirectory", value);
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
            AppUtils.AppConfig.SetBoolean("appsettings", "autoUpdate", value);
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
            AppUtils.AppConfig.SetBoolean("appsettings", "offlineMode", value);
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
            AppUtils.AppConfig.SetBoolean("appsettings", "beatmapOpt", value);
            OnPropertyChanged();
        }
    }
    #endregion

    #region GameSettings

    private string _keyOsuLeft;

    public string KeyOsuLeft
    {
        get => _keyOsuLeft;
        set
        {
            _keyOsuLeft = value;
            _configHelper.EditValue("keyOsuLeft", value);
            OnPropertyChanged();
        }
    }
    private int _musicVolume;

    public int MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            _configHelper.EditValue("VolumeMusic", value.ToString());
            OnPropertyChanged();
        }
    }
    private int _masterVolume;

    public int MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            _configHelper.EditValue("VolumeMaster", value.ToString());
            OnPropertyChanged();
        }
    }
    private int _effectsVolume;

    public int EffectsVolume
    {
        get => _effectsVolume;
        set
        {
            _effectsVolume = value;
            _configHelper.EditValue("VolumeEffects", value.ToString());
            OnPropertyChanged();
        }
    }
    
    private float _mouseSensitivity;

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set
        {
            _mouseSensitivity = value;
            _configHelper.EditValue("MouseSpeed", value.ToString());
            OnPropertyChanged();
        }
    }
    
    private bool _mouseDisableWheel;

    public bool MouseDisableWheel
    {
        get => _mouseDisableWheel;
        set
        {
            _mouseDisableWheel = value;
            _configHelper.EditValue("MouseDisableWheel", value.ToString());
            OnPropertyChanged();
        }
    }
    
    private bool _mouseDisableButtons;

    public bool MouseDisableButtons
    {
        get => _mouseDisableButtons;
        set
        {
            _mouseDisableButtons = value;
            _configHelper.EditValue("MouseDisableButtons", value.ToString());
            OnPropertyChanged();
        }
    }
    
    private bool _showFPS;

    public bool ShowFPS
    {
        get => _showFPS;
        set
        {
            _showFPS = value;
            _configHelper.EditValue("ShowFPS", value.ToString());
            OnPropertyChanged();
        }
    }
    
    private bool _fullScreen;

    public bool FullScreen
    {
        get => _fullScreen;
        set
        {
            _fullScreen = value;
            _configHelper.EditValue("Fullscreen", value.ToString());
            OnPropertyChanged();
        }
    }
    [ObservableProperty] private bool _richPresence;
    [ObservableProperty] private OsuSettingsPreset _currentOsuSettingsPreset;
    [ObservableProperty] private ObservableCollection<OsuSettingsPreset> _osuSettingsPresets;
    [ObservableProperty] private List<string> _skins;
    [ObservableProperty] private IOsuService _osu;
    [ObservableProperty] private ObservableCollection<OsuLauncherProfile> _localProfiles;
    #endregion

    private async void Auth()
    {
        AuthedUser = await Osu.Client.GetAuthenticatedUserAsync() ?? throw new InvalidOperationException();
    }
    
    public SettingsViewModel(/*IOsuService osuClient*/)
    {
        //Osu = osuClient;
        //Auth();
        try
        {
            var osuCfg = Path.Combine(AppUtils.AppConfig.GetString("appsettings", "gameDirectory"),
                $"osu!.{Environment.UserName}.cfg");
            _configHelper = new OsuConfigHelper(osuCfg);
            //_loggedIn = true;
            /*Skins = Directory.GetDirectories(Path.Combine(AppSettings.Default.GameDirectory, "Skins")).ToList() ??
                    new List<string>();*/
            AutoUpdate = AppUtils.AppConfig.GetBoolean("appsettings", "autoUpdate");
            OfflineMode = AppUtils.AppConfig.GetBoolean("appsettings", "offlineMode");
            BeatmapOpt = AppUtils.AppConfig.GetBoolean("appsettings", "beatmapOpt");
            GameDirectory = AppUtils.AppConfig.GetString("appsettings", "gameDirectory");
            SongsDirectory = AppUtils.AppConfig.GetString("appsettings", "songsDirectory");
            TrainingClientDirectory = AppUtils.AppConfig.GetString("appsettings", "trainingClientDirectory");
            EffectsVolume = _configHelper.ReadInt("VolumeEffect"); 
            MouseSensitivity = _configHelper.ReadFloat("MouseSpeed");
            MouseDisableWheel = _configHelper.ReadInt("MouseDisableWheel") != 0;
            MouseDisableButtons = _configHelper.ReadInt("MouseDisableButtons") != 0;
            RichPresence = _configHelper.ReadInt("DiscordRichPresence") != 0;
            ShowFPS = _configHelper.ReadInt("FpsCounter") != 0;
            FullScreen = _configHelper.ReadInt("Fullscreen") != 0;
            MasterVolume = _configHelper.ReadInt("VolumeUniversal");
            MusicVolume = _configHelper.ReadInt("VolumeMusic");
            KeyOsuLeft =  _configHelper.ReadString("keyOsuLeft");
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
    
    /*[RelayCommand] private void EnableAutoUpdate() {LauncherSettings.Default.CheckForUpdates = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableAutoUpdate() {LauncherSettings.Default.CheckForUpdates = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void EnableOfflineMode() {LauncherSettings.Default.OfflineStartup = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableOfflineMode() {LauncherSettings.Default.OfflineStartup = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void EnableBeatmapOpt() {LauncherSettings.Default.BeatmapOpt = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableBeatmapOpt() {LauncherSettings.Default.BeatmapOpt = false; LauncherSettings.Default.Save();}*/
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
    private void RetrieveKey()
    {
        var selectedKey = Console.ReadKey(true);
        _configHelper.EditValue("KeyOsuLeft", selectedKey.Key.ToString());
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