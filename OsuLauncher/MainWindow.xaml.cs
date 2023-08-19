namespace OsuLauncher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private OsuMemoryReader osuMemoryReader;
    private OsuClient osuClient;
    public MainWindow()
    {
        InitializeComponent();
        osuMemoryReader = new OsuMemoryReader();
        AutoUpdater.ExecutablePath = Path.Combine(Directory.GetCurrentDirectory(), "OsuLauncher.exe");
        AutoUpdater.SetOwner(this);
        AutoUpdater.Start(AppSettings.Default.UpdateUrl);
        VersionText.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        HomePage homePage = new HomePage();
        MainFrame.Content = homePage;
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json");
        using var reader = new StreamReader(stream!);
        var config = System.Text.Json.JsonSerializer.Deserialize<SecretsConfiguration>(reader.ReadToEnd(), new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
        switch (AppSettings.Default.LaunchPreference)
        {
            case "McOsu":
                StartupPreferenceCB.SelectedIndex = 0;
                break;

            case "osu!":
                StartupPreferenceCB.SelectedIndex = 1;
                break;
            default:
                StartupPreferenceCB.SelectedIndex = 1;
                break;
        }
        _clientId = config!.ClientId;
        _clientSecret = config.ClientSecret;
        PlayButton.Click += PlayButtonOnClick;
        HomeNavButton.Click += (sender, args) => MainFrame.Content = homePage;
        SettingsNavButton.Click += (sender, args) => MainFrame.Content = new SettingsPage();
        AccountNavButton.Click += (sender, args) =>
        {
            try
            {
                if (!ReferenceEquals(MainFrame.Content, typeof(AccountPage)))
                    MainFrame.Content = new AccountPage();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        };
        CollectionsNavButton.Click += (sender, args) => MainFrame.Content = new CollectionsPage();
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        osuClient = await ApiHelper.Instance.RetrieveClient();
        if (!osuClient.IsAuthenticated)
            await InitiateTokenRefresh();

        await RetrieveUserInfo();
    }

    public async Task RetrieveUserInfo()
    {
        if (!osuClient.IsAuthenticated)
        {
            if (!File.Exists(ApiHelper.Instance.GetTokenLocation()))
            {
                Growl.Error("Please log in from the AppSettings page");
            }
            else
            {
                UsernameText.Text = $"Player Name: Unavailable please log in";
                PPRankText.Text = $"PP Count: Unavailable please log in";
                AccuracyText.Text = $"Accuracy: Unavailable please log in";
                LevelText.Text = $"Lv Unavailable please log in";

                var tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(File.ReadAllText(ApiHelper.Instance.GetTokenLocation()), new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                });

                if (tokenResponse.AccessToken == string.Empty)
                {
                    Growl.Info("Access token not found");
                }
                else
                {
                    Growl.Info("Token Expired Please login");
                    await InitiateTokenRefresh();
                }
            }
        }
        else
        {
            var authedUser = await osuClient.GetAuthenticatedUserAsync();
            AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
            UsernameText.Text = $"Player Name: {authedUser?.Username}";
            PPRankText.Text = $"PP Count: {authedUser?.UserStats.PP.ToString()}";
            AccuracyText.Text = $"Accuracy: {authedUser?.UserStats.HitAccuracy}";
            LevelText.Text = $"Lv {authedUser?.UserStats.Level.Current}";
        }
    }

    private async Task InitiateTokenRefresh()
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");
            var token = File.ReadAllText(ApiHelper.Instance.GetTokenLocation());
            TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(token, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            });
            var formData = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", tokenResponse.RefreshToken },
            };

            request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to swap osu! API tokens.");
            }

            var json = await response.Content.ReadAsStringAsync();

            var tokenJson = System.Text.Json.JsonSerializer.Serialize<TokenResponse>(tokenResponse, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                WriteIndented = true,
            });
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenJson);
            await RetrieveUserInfo();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
        }
    }

    private async void PlayButtonOnClick(object sender, RoutedEventArgs e)
    {
        switch (StartupPreferenceCB.SelectedIndex)
        {
            case 0:
                if (string.IsNullOrEmpty(AppSettings.Default.TrainingClientDirectory))
                    Growl.Error("McOsu Game path is not set!");

                Process[] mcOsuProcess = Process.GetProcessesByName("McEngine");
                if (mcOsuProcess.Length != 0)
                    Growl.Info("There is already an instance of McOsu running.");

                Process.Start(new ProcessStartInfo("steam://rungameid/607260")
                {
                    UseShellExecute = true
                });
                Hide();
                break;

            case 1:
                if (string.IsNullOrEmpty(AppSettings.Default.GameDirectory))
                    Growl.Error("Osu Game path is not set!");

                Process[] osuProcess = Process.GetProcessesByName("osu!");
                if (osuProcess.Length != 0)
                    Growl.Info("There is already an instance of osu! running.");

                Process.Start(Path.Combine(AppSettings.Default.GameDirectory, "osu!.exe"));
                Growl.Info("Launched");
                var currentBeatmap = osuMemoryReader.GetCurrentBeatmap().Result;
                Growl.Info(currentBeatmap.Beatmap.Id.ToString());
                if (AppSettings.Default.UseCustomRPC)
                {

                    //await AppUtils.RPC.Start(currentBeatmap.Beatmap.Id.ToString());
                }
                break;
        }
    }

    private async void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        await AppUtils.RPC.Stop();
        Log.CloseAndFlush();
    }

    private void StartupPreferenceCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (StartupPreferenceCB.SelectedIndex)
        {
            case 0:
                AppSettings.Default.LaunchPreference = "McOsu";
                AppSettings.Default.Save();
                break;

            case 1:
                AppSettings.Default.LaunchPreference = "osu";
                AppSettings.Default.Save();
                break;
        }
    }
}