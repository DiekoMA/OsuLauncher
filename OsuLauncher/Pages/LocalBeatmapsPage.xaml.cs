using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OsuLauncher.Pages
{
    /// <summary>
    /// Interaction logic for LocalBeatmapsPage.xaml
    /// </summary>
    public partial class LocalBeatmapsPage : Page
    {
        public LocalBeatmapsPage()
        {
            string formattedNames = string.Empty;
            InitializeComponent();
            RefreshSongListBtn.Click += (sender, e) =>
            {
                
            };

            DeleteSongListBtn.Click += (sender, e) =>
            {
                var selectedSong = (DirectoryInfo)SongsList.SelectedItem;
                selectedSong.Delete();
            };

            LocateSongListBtn.Click += (sender, e) =>
            {
                
            };
            DirectoryInfo songsDirectory = new DirectoryInfo(AppSettings.Default.SongsDirectory);
            List<string> fileNames = new List<string>();
            List<string> cleanedNames = new List<string>();
            string pattern = @"^\d+\s";
            foreach (var fileName in songsDirectory.GetDirectories())
            {
                string cleanedName = Regex.Replace(fileName.Name, pattern, "");
                cleanedNames.Add(cleanedName);
            }
            
            SongsList.ItemsSource = cleanedNames;
        }
    }
}
