using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using HandyControl.Controls;
using osu_database_reader.BinaryFiles;
using OsuLauncher.Helpers;
using OsuLauncher.Pages;
using OsuAPI;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
using Window = System.Windows.Window;

namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OsuDb db;
        private OsuClient _client;
        public MainWindow()
        {
            InitializeComponent();
            var tokenLocation = Path.Combine(Directory.GetCurrentDirectory(), "token.secret");
            if (File.Exists(tokenLocation))
            {
                _client = new OsuClient(File.ReadAllText(tokenLocation));
            }

            //db = OsuDb.Read(stream);
            if (_client != null && _client.IsAuthenticated())
            {
                UsernameText.Text = $"Player Name: {_client.GetAuthenticatedUser()?.Username}";
                PPRankText.Text =  $"PP Count: {_client.GetAuthenticatedUser()?.UserStats.PP.ToString()}";
            }
            else
            {
                UsernameText.Text = $"Player Name: Unavailabe";
                PPRankText.Text = $"PP Count: Unavailable";
            }
            switch (AppUtils.CheckForInternetConnection())
            {
                case true:
                    ConnectionStateText.Text = "Internet Connected";
                    break;
                
                case false:
                    ConnectionStateText.Text = "Internet Disconnected";
                    break;
            }
            PlayButton.Click += PlayButtonOnClick;
            NewsNavButton.Click += (sender, args) => MainFrame.Content = new NewsPage();;
            BeatmapNavButton.Click += (sender, args) => MainFrame.Content = new BeatmapPage();
            SettingsNavButton.Click += (sender, args) => MainFrame.Content = new SettingsPage();
            CollectionsNavButton.Click += (sender, args) => MainFrame.Content = new CollectionsPage();
            ReplaysNavButton.Click += (sender, args) => MainFrame.Content = new ReplaysPage();
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