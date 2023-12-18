namespace OsuLauncher.Pages.Settings;

public partial class GeneralSettings
{
    private OsuClient osuClient;
    private IConfigurationRoot config;
    private string clientId;
    private string clientSecret;
    private string redirectUrl;

    public GeneralSettings()
    {
        InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json"))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                var config = System.Text.Json.JsonSerializer.Deserialize<SecretsConfiguration>(reader.ReadToEnd());
                clientId = config.ClientId;
                clientSecret = config.ClientSecret;
                redirectUrl = config.RedirectUrl;
            }
        }
        osuClient = new OsuClient();
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        GameDirectoryBox.Text = LauncherSettings.Default.GameDirectory;
        SongsDirectoryBox.Text = LauncherSettings.Default.SongsDirectory;
        MCOsuDirectoryBox.Text = LauncherSettings.Default.TrainingClientDirectory;
        if (LauncherSettings.Default.CheckForUpdates){
            UpdateCB.IsChecked = true;
        }
            
        if (LauncherSettings.Default.OfflineStartup){
            OfflineStartupCB.IsChecked = true;
        }
            
        GameDirectoryBox.TextChanged += (sender, args) => LauncherSettings.Default.GameDirectory = GameDirectoryBox.Text; LauncherSettings.Default.Save();
        SongsDirectoryBox.TextChanged += (sender, args) => LauncherSettings.Default.SongsDirectory = SongsDirectoryBox.Text; LauncherSettings.Default.Save();
        MCOsuDirectoryBox.TextChanged += (sender, args) => LauncherSettings.Default.TrainingClientDirectory = MCOsuDirectoryBox.Text; LauncherSettings.Default.Save();
        ThemeCb.Text = LauncherSettings.Default.ThemeBase;
        ThemeCb.SelectionChanged += (sender, args) =>
        {
            switch (ThemeCb.SelectedIndex)
            {
                case 0:
                    ThemeManager.Current.UsingSystemTheme = true;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    LauncherSettings.Default.ThemeBase = "System";
                    LauncherSettings.Default.ThemeAccent = SystemParameters.WindowGlassBrush.ToString();
                    LauncherSettings.Default.Save();
                    break;

                case 1:
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    LauncherSettings.Default.ThemeBase = "Dark";
                    LauncherSettings.Default.ThemeAccent = SystemParameters.WindowGlassBrush.ToString();
                    LauncherSettings.Default.Save();
                    break;

                case 2:
                    var converter = new BrushConverter();
                    var brush = converter.ConvertFromString("#2E3440") as Brush;
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    ThemeManager.Current.AccentColor = brush;
                    LauncherSettings.Default.ThemeBase = "Nord";
                    LauncherSettings.Default.ThemeAccent = brush.ToString();
                    LauncherSettings.Default.Save();
                    break;

                case 3:
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    LauncherSettings.Default.ThemeBase = "Light";
                    LauncherSettings.Default.ThemeAccent = SystemParameters.WindowGlassBrush.ToString();
                    LauncherSettings.Default.Save();
                    break;
            }
        };
        UpdateCB.Checked += (sender, args) => LauncherSettings.Default.CheckForUpdates = true; LauncherSettings.Default.Save();
        InitAuthButton.Click += (sender, args) =>
        {
            try
            {
                Process.Start(new ProcessStartInfo(
                    $"https://osu.ppy.sh/oauth/authorize?client_id={clientId}&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify+friends.read&state=randomval")
                {
                    UseShellExecute = true
                });

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
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

        };
    }

    private void DialogOptCB_OnChecked(object sender, RoutedEventArgs e)
    {
        AppUtils.Config.SaveBool("User_Preference", "beatmapmirroroptin", true);
    }

    private void DialogOptCB_OnUnchecked(object sender, RoutedEventArgs e)
    {
        AppUtils.Config.SaveBool("User_Preference", "beatmapmirroroptin", false);
    }

    private async void InitiateTokenSwap(string code)
    {
        try
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

            TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            }) ?? throw new InvalidOperationException();

            var tokenResponseLocal = JsonSerializer.SerializeToUtf8Bytes(tokenResponse);
            byte[] encryptedToken = ProtectedData.Protect(tokenResponseLocal, null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes("token.secret", encryptedToken);

            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                osuClient = await ApiHelper.Instance.RetrieveClient();
                var authedUser = await osuClient.GetAuthenticatedUserAsync();

                InitAuthButton.Visibility = Visibility.Collapsed;
                LoginText.Text = $"Logged in as {authedUser.Username}";
                mainWindow.AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
            });
        }
        catch (Exception e)
        {
            Growl.Error(e.Source);
            Growl.Error(e.InnerException.StackTrace);
            Growl.Error(e.Message);
        }
    }
}
