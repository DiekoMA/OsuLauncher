namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private OsuClient _client;
        public MainWindow()
        {
            InitializeComponent();
            SettingsTitleAreaButton.Click += (sender, args) => DialogHelper.ShowDialog(typeof(SettingsDialog));
            var tokenLocation = Path.Combine(Directory.GetCurrentDirectory(), "token.secret");
            if (File.Exists(tokenLocation))
            {
                _client = new OsuClient(File.ReadAllText(tokenLocation));
            }
            if (_client != null && _client.IsAuthenticated())
            {
                var authedUser = _client.GetAuthenticatedUserAsync();
                UsernameText.Text = $"Player Name: {authedUser?.Result.Username}";
                PPRankText.Text =  $"PP Count: {authedUser?.Result?.UserStats.PP.ToString()}";
                AccuracyText.Text = $"Accuracy: {authedUser?.Result.UserStats.HitAccuracy}";
                LevelText.Text = $"Lv {authedUser?.Result.UserStats.Level.Current}";
            }
            else
            {
                UsernameText.Text = $"Player Name: Unavailable please log in";
                PPRankText.Text = $"PP Count: Unavailable please log in";
                AccuracyText.Text = $"Accuracy: Unavailable please log in";
                LevelText.Text = $"Lv Unavailable please log in";
            }
            PlayButton.Click += PlayButtonOnClick;
            NewsNavButton.Click += (sender, args) => MainFrame.Content = new NewsPage();
            /*WikiNavButton.Click += (sender, args) => MainFrame.Content = new WikiPage(); */
            BeatmapNavButton.Click += (sender, args) =>
            {
                if (ConfigHelper.GetBoolItem("User_Preference", "beatmapmirroroptin"))
                {
                    MainFrame.Content = new BeatmapPage();   
                }
                else
                {
                    Growl.Info("This has been disabled for your safety, you can reenable it in the settings");
                }
            };
            MoreGameOptions.Click += (sender, args) => MoreOptionsPopup.IsOpen = true;
            CollectionsNavButton.Click += (sender, args) => MainFrame.Content = new CollectionsPage();
            /*ReplaysNavButton.Click += (sender, args) => MainFrame.Content = new ReplaysPage();*/
        }

        private void PlayButtonOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigHelper.GetStringItem("preferences", "gamedir")))
            {
                Growl.Error("Osu Game path is not set!");
            }
            else
            {
                Process.Start(Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"), "osu!.exe"));
            }
        }

        private void MainWindow_OnGotFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Focus received");
        }
    }
}