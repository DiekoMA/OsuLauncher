using Onova;
using Onova.Services;

namespace OsuLauncher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private DiscordRpcClient rpcClient;
    private OsuClient osuClient;
    public MainWindow()
    {
        InitializeComponent();
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
        switch (AppUtils.Config.GetStringItem("preferences", "startuppreference"))
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
        /*rpcClient = new DiscordRpcClient("1117114635821268992");
        rpcClient.Initialize();*/
        _clientId = config!.ClientId;
        _clientSecret = config.ClientSecret;
        PlayButton.Click += PlayButtonOnClick;
        HomeNavButton.Click += (sender, args) => MainFrame.Content = homePage;//new HomePage();
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
            InitiateTokenRefresh();

        RetrieveUserInfo();
    }

    public async void RetrieveUserInfo()
    {
        if (!osuClient.IsAuthenticated)
        {
            if (!File.Exists(ApiHelper.Instance.GetTokenLocation()))
            {
                Growl.Error("Please log in from the settings page");
            }
            else
            {
                Growl.Error("Client is not authenticated");
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
                    Growl.Info("Access token not here");
                }
                else
                {
                    Growl.Info("Do refresh");
                    InitiateTokenRefresh();
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

    private async void InitiateTokenRefresh()
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
            RetrieveUserInfo();
        }
        catch (Exception e)
        {
            Growl.Error(e.Message);
        }
    }

    private void PlayButtonOnClick(object sender, RoutedEventArgs e)
    {
        switch (StartupPreferenceCB.SelectedIndex)
        {
            case 0:
                if (string.IsNullOrEmpty(AppUtils.Config.GetStringItem("preferences", "trainingclientdir")))
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
                if (string.IsNullOrEmpty(AppUtils.Config.GetStringItem("preferences", "gamedir")))
                    Growl.Error("Osu Game path is not set!");

                Process[] osuProcess = Process.GetProcessesByName("osu!");
                if (osuProcess.Length != 0)
                    Growl.Info("There is already an instance of osu! running.");


                Process.Start(Path.Combine(AppUtils.Config.GetStringItem("preferences", "gamedir"), "osu!.exe"));
                Hide();
                if (AppUtils.Config.GetBoolItem("preferences", "customRpc"))
                {
                    rpcClient.SetPresence(new RichPresence()
                    {
                        Details = "Osu",
                        State = "",
                    });
                }
                break;
        }
    }

    private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        /*if (rpcClient.IsInitialized)
            rpcClient.Dispose();*/
    }

    private void StartupPreferenceCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (StartupPreferenceCB.SelectedIndex)
        {
            case 0:
                AppUtils.Config.SaveStringItem("preferences", "startuppreference", "McOsu");
                break;

            case 1:
                AppUtils.Config.SaveStringItem("preferences", "startuppreference", "osu");
                break;
        }
    }
}