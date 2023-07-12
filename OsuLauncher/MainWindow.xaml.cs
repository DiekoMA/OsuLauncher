namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private DiscordRpcClient rpcClient;
        public MainWindow()
        {
            InitializeComponent();
            HomePage homePage = new HomePage();
            MainFrame.Content = homePage;
            // GeneralSettings generalSettingsPage = new GeneralSettings();
            // generalSettingsPage.UpdateUIEvent += GeneralSettingsPageOnUpdateUIEvent;
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json");
            using var reader = new StreamReader(stream!);
            var config = System.Text.Json.JsonSerializer.Deserialize<SecretsConfiguration>(reader.ReadToEnd(), new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });
            _clientId = config!.ClientId;
            _clientSecret = config.ClientSecret;
            PlayButton.Click += PlayButtonOnClick;
            HomeNavButton.Click += (sender, args) => MainFrame.Content = homePage;//new HomePage();
            SettingsNavButton.Click += (sender, args) => MainFrame.Content = new SettingsPage();
            AccountNavButton.Click += (sender, args) =>
            {
                try
                {
                    if (ReferenceEquals(MainFrame.Content, typeof(AccountPage)))
                    {
                        MessageBox.Show("Already here");
                    }
                    else
                    {
                        MainFrame.Content = new AccountPage();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            };
            // CollectionsNavButton.Click += (sender, args) => MainFrame.Content = new CollectionsPage();
        }
        
        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var osuClient = await ApiHelper.Instance.RetrieveClient();
            RetrieveUserInfo(osuClient);
        }

        // private void GeneralSettingsPageOnUpdateUIEvent()
        // {
        //     RetrieveUserInfo();
        // }

        private async void RetrieveUserInfo(OsuClient osuClient)
        {
            if (!osuClient.IsAuthenticated)
            {
                Growl.Error("Client is not authenticated");
                
                UsernameText.Text = $"Player Name: Unavailable please log in";
                PPRankText.Text = $"PP Count: Unavailable please log in";
                AccuracyText.Text = $"Accuracy: Unavailable please log in";
                LevelText.Text = $"Lv Unavailable please log in";
                InitiateTokenRefresh(osuClient);
            }
            else
            {
                var authedUser = await osuClient.GetAuthenticatedUserAsync();
            
                AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
                UsernameText.Text = $"Player Name: {authedUser?.Username}";
                PPRankText.Text =  $"PP Count: {authedUser?.UserStats.PP.ToString()}";
                AccuracyText.Text = $"Accuracy: {authedUser?.UserStats.HitAccuracy}";
                LevelText.Text = $"Lv {authedUser?.UserStats.Level.Current}";
            }
        }

        private async void InitiateTokenRefresh(OsuClient osuClient)
        {
            try
            {
                using var client = new HttpClient();
                var refreshToken = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "refreshtoken.secret"));
                var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");

                var formData = new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshToken },
                };
                
                request.Content = new FormUrlEncodedContent(formData);
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to swap osu! API tokens.");
                }

                var json = await response.Content.ReadAsStringAsync();
                TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                }) ?? throw new InvalidOperationException();

                await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "refreshtoken.secret"), tokenResponse.RefreshToken);
                await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenResponse.AccessToken);
                RetrieveUserInfo(osuClient);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void PlayButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AppUtils.Config.GetStringItem("preferences", "gamedir")))
            {
                Growl.Error("Osu Game path is not set!");
            }
            else
            {
                Process[] pname = Process.GetProcessesByName("osu!");
                if (pname.Length == 0)
                {
                    Process.Start(Path.Combine(AppUtils.Config.GetStringItem("preferences", "gamedir"), "osu!.exe"));
                    Hide();
                    if (AppUtils.Config.GetBoolItem("preferences", "customRpc"))
                    {
                        rpcClient = new DiscordRpcClient("1117114635821268992");
                        rpcClient.Initialize();
                        
                        rpcClient.SetPresence(new RichPresence()
                        {
                            Details = "Osu",
                            State = "",
                        });	
                        
                    }
                }
                else
                {
                    Growl.Info("There is already an instance of osu! running.");
                }
            }
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            if (rpcClient.IsInitialized)
                rpcClient.Dispose();
        }
    }
}