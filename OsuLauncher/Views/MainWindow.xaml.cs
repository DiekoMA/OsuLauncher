﻿namespace OsuLauncher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        /*/*switch (LauncherSettings.Default.LaunchPreference)
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
        }#1#
        /*HomeNavButton.Click += (sender, args) => MainFrame.Navigate(new Uri("Pages/HomePage.xaml", UriKind.RelativeOrAbsolute));
        SettingsNavButton.Click += (sender, args) => MainFrame.Navigate(new Uri("Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        OnlineBeatmapsNavButton.Click += (sender, args) => MainFrame.Navigate(new Uri("Pages/BeatmapExplorePage.xaml", UriKind.RelativeOrAbsolute));#1#
        /*AccountNavButton.Click += (sender, args) =>
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
        };#1#
        //LocalBeatmapsNavButton.Click += (sender, args) => MainFrame.Content = new LocalBeatmapsPage();
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        /*osuClient = await ApiHelper.Instance.RetrieveClient();
        if (!osuClient.IsAuthenticated)
            await InitiateTokenRefresh();

        await RetrieveUserInfo();#1#
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
                byte[] readEncryptedTokenResponse = File.ReadAllBytes("token.secret");
                byte[] decryptedTokenResponse = ProtectedData.Unprotect(readEncryptedTokenResponse, null, DataProtectionScope.CurrentUser);
                var decryptedAccesstoken = JsonSerializer.Deserialize<TokenResponse>(decryptedTokenResponse);

                if (decryptedAccesstoken.AccessToken == string.Empty)
                {
                    Growl.Info("Access token not found");
                }
                else if (await osuClient.ValidateToken(decryptedAccesstoken.AccessToken))
                {
                    Growl.Info("Token Expired Please login");
                    await InitiateTokenRefresh();
                }
            }
        }
        else
        {
            var authedUser = await osuClient.GetAuthenticatedUserAsync();
            //AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
        }
    }

    private async Task InitiateTokenRefresh()
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");
            /*var token = File.ReadAllText(ApiHelper.Instance.GetTokenLocation());#1#
            /*TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(token, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            });#1#

            byte[] readEncryptedTokenResponse = File.ReadAllBytes("token.secret");
            byte[] decryptedTokenResponse = ProtectedData.Unprotect(readEncryptedTokenResponse, null, DataProtectionScope.CurrentUser);
            var decryptedAccesstoken = JsonSerializer.Deserialize<TokenResponse>(decryptedTokenResponse);

            var formData = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "grant_type", "refresh_token" },
                { "refresh_token", decryptedAccesstoken.RefreshToken },
            };

            request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to swap osu! API tokens.");
            }

            var json = await response.Content.ReadAsStringAsync();

            /*var tokenJson = System.Text.Json.JsonSerializer.Serialize<TokenResponse>(tokenResponse, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                WriteIndented = true,
            });#1#

            TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            }) ?? throw new InvalidOperationException();

            var tokenResponseLocal = JsonSerializer.SerializeToUtf8Bytes(tokenResponse);
            byte[] encryptedToken = ProtectedData.Protect(tokenResponseLocal, null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes("token.secret", encryptedToken);
            //await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenJson);
            await RetrieveUserInfo();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
        }
    }

    private async void PlayButtonOnClick(object sender, RoutedEventArgs e)
    {
        /*switch (StartupPreferenceCB.SelectedIndex)
        {
            case 0:
                if (string.IsNullOrEmpty(LauncherSettings.Default.TrainingClientDirectory))
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
                if (string.IsNullOrEmpty(LauncherSettings.Default.GameDirectory))
                    Growl.Error("Osu Game path is not set!");

                Process[] osuProcess = Process.GetProcessesByName("osu!");
                if (osuProcess.Length != 0)
                    Growl.Info("There is already an instance of osu! running.");

                Process.Start(Path.Combine(LauncherSettings.Default.GameDirectory, "osu!.exe"));
                Growl.Info("Launched");
                var currentBeatmap = osuMemoryReader.GetCurrentBeatmap().Result;
                Growl.Info(currentBeatmap.Beatmap.Id.ToString());
                if (LauncherSettings.Default.UseCustomRPC)
                {
                    //await AppUtils.RPC.Start(currentBeatmap.Beatmap.Id.ToString());
                }
                break;
        }#1#
    }

    private async void MainWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        await AppUtils.RPC.Stop();
        Log.CloseAndFlush();
    }*/
    }
}