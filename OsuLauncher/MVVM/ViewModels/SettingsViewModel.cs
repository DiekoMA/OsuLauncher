using CommunityToolkit.Mvvm.Input;
using Beatmap = API.Objects.Beatmap;

namespace OsuLauncher.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    #region Generic

    private OsuClient osuClient;
    private IConfigurationRoot config;
    private OsuConfigHelper _configHelper;
    [ObservableProperty] private bool _loggedIn;
    #endregion
    
    #region AppSettings
    [ObservableProperty] private string? _gameDirectory;
    [ObservableProperty] private string? _songsDirectory;
    [ObservableProperty] private string? _trainingClientDirectory;
    [ObservableProperty] private string _currentTheme;
    [ObservableProperty] private bool _autoUpdate;
    [ObservableProperty] private bool _offlineMode;
    [ObservableProperty] private bool _beatmapOpt;
    #endregion

    #region GameSettings
    [ObservableProperty] private string _currentSkin;
    [ObservableProperty] private int _masterVolume;
    [ObservableProperty] private int _effectsVolume;
    [ObservableProperty] private int _musicVolume;
    [ObservableProperty] private float _mouseSensitivity;
    [ObservableProperty] private bool _mouseWheel;
    [ObservableProperty] private bool _mouseButton;
    [ObservableProperty] private bool _rpc;
    [ObservableProperty] private bool _showFps;
    [ObservableProperty] private bool _fullscreen;
    #endregion

    public SettingsViewModel()
    {
        var osuCfg = Path.Combine(LauncherSettings.Default.GameDirectory,
            $"osu!.{Environment.UserName}.cfg");
        _configHelper = new OsuConfigHelper(osuCfg);
        _loggedIn = true;
        AutoUpdate = LauncherSettings.Default.CheckForUpdates;
        OfflineMode = LauncherSettings.Default.OfflineStartup;
        BeatmapOpt = LauncherSettings.Default.BeatmapOpt;
        GameDirectory = LauncherSettings.Default.GameDirectory;
        SongsDirectory = LauncherSettings.Default.SongsDirectory;
        TrainingClientDirectory = LauncherSettings.Default.TrainingClientDirectory;
        MouseSensitivity = _configHelper.ReadFloat("MouseSpeed");
        MouseWheel = _configHelper.ReadInt("MouseDisableWheel") != 0;
        MouseButton = _configHelper.ReadInt("MouseDisableButtons") != 0;
        Rpc = _configHelper.ReadInt("DiscordRichPresence") != 0;
        ShowFps = _configHelper.ReadInt("FpsCounter") != 0;
        Fullscreen = _configHelper.ReadInt("Fullscreen") != 0;
        MasterVolume = _configHelper.ReadInt("VolumeUniversal");
        EffectsVolume = _configHelper.ReadInt("VolumeEffect");

        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json");
        using StreamReader sr = new StreamReader(stream);
        var config = JsonSerializer.Deserialize<SecretsConfiguration>(sr.ReadToEnd());
        osuClient = new OsuClient();
        config.ClientId = "";
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
    
    [RelayCommand] private void EnableAutoUpdate() {LauncherSettings.Default.CheckForUpdates = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableAutoUpdate() {LauncherSettings.Default.CheckForUpdates = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void EnableOfflineMode() {LauncherSettings.Default.OfflineStartup = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableOfflineMode() {LauncherSettings.Default.OfflineStartup = false; LauncherSettings.Default.Save();}
    [RelayCommand] private void EnableBeatmapOpt() {LauncherSettings.Default.BeatmapOpt = true; LauncherSettings.Default.Save();}
    [RelayCommand] private void DisableBeatmapOpt() {LauncherSettings.Default.BeatmapOpt = false; LauncherSettings.Default.Save();}

    [RelayCommand]
    private void ChangeSensitivity() => _configHelper.EditValue("MouseSpeed", MouseSensitivity.ToString());
    #endregion
    
    [RelayCommand]
    private void SaveSettings()
    {
        LauncherSettings.Default.CheckForUpdates = AutoUpdate;
        LauncherSettings.Default.OfflineStartup = OfflineMode;
        LauncherSettings.Default.BeatmapOpt = BeatmapOpt;
        LauncherSettings.Default.GameDirectory = GameDirectory ?? String.Empty;
        LauncherSettings.Default.SongsDirectory = SongsDirectory ?? String.Empty;
        LauncherSettings.Default.TrainingClientDirectory = TrainingClientDirectory ?? String.Empty;
        LauncherSettings.Default.OfflineStartup = !OfflineMode;
        LauncherSettings.Default.Save();
    }
    
    [RelayCommand]
    public void InitiateAuth()
    {
        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName =
                $"https://osu.ppy.sh/oauth/authorize?client_id={config["clientId"]}&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify+friends.read&state=randomval"
        };
        Process.Start(startInfo);
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:7040/callback/");
        listener.Start();
        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            string responseText = "Authentication complete";
            byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseText);
            context.Response.ContentLength64 = responseBytes.Length;
            context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
            context.Response.Close();
            var code = context.Request.Url?.AbsoluteUri;
            var tokenRegex = new Regex("(?<=code=)(.*)(?=&state)");
            if (!tokenRegex.IsMatch(code))
                return;
            var authCode = tokenRegex.Match(code).Value;
            InitiateTokenSwap(authCode);
            listener.Stop();
        }
    }

    private async void InitiateTokenSwap(string code)
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
                { "client_id", config["clientId"]! },
                { "client_secret", config["clientSecret"]! },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", config["redirectUrl"]! }
            })
        };

        using var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to swap osu! API tokens.");
        }

        response.EnsureSuccessStatusCode();

        TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        }) ?? throw new InvalidOperationException();

        var tokenResponseLocal = JsonSerializer.SerializeToUtf8Bytes(tokenResponse);
        byte[] encryptedToken = ProtectedData.Protect(tokenResponseLocal, null, DataProtectionScope.CurrentUser);

        File.WriteAllBytes("token.secret", encryptedToken);

        osuClient = await ApiHelper.Instance.RetrieveClient();
        var authedUser = await osuClient.GetAuthenticatedUserAsync();

        LoggedIn = true;
    }
}