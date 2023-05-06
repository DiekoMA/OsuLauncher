 using System.Diagnostics;
using System.IO;
using System.Windows;
using HandyControl.Controls;
using OsuLauncher.Helpers;
using OsuLauncher.Pages;
using OsuAPI;
using OsuLauncher.Dialogs;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using Window = System.Windows.Window;

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
            WikiNavButton.Click += (sender, args) => MainFrame.Content = new WikiPage(); 
            BeatmapNavButton.Click += (sender, args) => MainFrame.Content = new BeatmapPage();
            MoreGameOptions.Click += (sender, args) => MoreOptionsPopup.IsOpen = true;
            /*CollectionsNavButton.Click += (sender, args) => MainFrame.Content = new CollectionsPage();*/
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
    }
}