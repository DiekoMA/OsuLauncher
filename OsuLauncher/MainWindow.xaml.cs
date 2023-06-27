using System.Text.Json;
using System.Text.Json.Serialization;

namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private OsuClient? _client;
        private string tokenLocation;
        public MainWindow()
        {
            InitializeComponent();
            tokenLocation = Path.Combine(Directory.GetCurrentDirectory(), "token.secret");
            if (File.Exists(tokenLocation))
            {
                _client = new OsuClient();
            }
            RetrieveUserInfo();
            PlayButton.Click += PlayButtonOnClick;
            NewsNavButton.Click += (sender, args) => MainFrame.Content = new NewsPage();
            SettingsNavButton.Click += (sender, args) => MainFrame.Content = new SettingsPage();
            BeatmapNavButton.Click += (sender, args) =>
            {
                if (AppUtils.Config.GetBoolItem("User_Preference", "beatmapmirroroptin"))
                {
                    MainFrame.Content = new BeatmapPage();   
                }
                else
                {
                    Growl.Info("This has been disabled for your safety, you can reenable it in the settings");
                }
            };

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
            /*ReplaysNavButton.Click += (sender, args) => MainFrame.Content = new ReplaysPage();*/
        }

        private async void RetrieveUserInfo()
        {
            await _client.TryAuthenticateAsync(File.ReadAllText(tokenLocation));

            if (!_client.IsAuthenticated)
            {
                Growl.Error("Client is not authenticated");
                
                UsernameText.Text = $"Player Name: Unavailable please log in";
                PPRankText.Text = $"PP Count: Unavailable please log in";
                AccuracyText.Text = $"Accuracy: Unavailable please log in";
                LevelText.Text = $"Lv Unavailable please log in";
            }
            else
            {
                var authedUser = await _client.GetAuthenticatedUserAsync();
            
                AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
                UsernameText.Text = $"Player Name: {authedUser?.Username}";
                PPRankText.Text =  $"PP Count: {authedUser?.UserStats.PP.ToString()}";
                AccuracyText.Text = $"Accuracy: {authedUser?.UserStats.HitAccuracy}";
                LevelText.Text = $"Lv {authedUser?.UserStats.Level.Current}";
            }
        }

        private async void InitiateTokenRefresh()
        {
            try
            {
                using var client = new HttpClient();
                var refreshToken = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "refreshtoken.secret"));
                var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");

                var formData = new Dictionary<string, string>
                {
                    { "client_id", "21770" },
                    { "client_secret", "CDMaHyAmbtex7f5Oxl3iUA7POwPESfCjkoP1fG3D" },
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
                RetrieveUserInfo();
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
                    this.Hide();
                }
                else
                {
                    Growl.Info("There is already an instance of osu! running.");
                }
            }
        }
    }
}