namespace OsuLauncher.Pages;

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
        /*DirectoryInfo songsDirectory = new DirectoryInfo(LauncherSettings.Default.SongsDirectory);
        List<string> fileNames = new List<string>();
        List<string> cleanedNames = new List<string>();
        string pattern = @"^\d+\s";
        foreach (var fileName in songsDirectory.GetDirectories())
        {
            string cleanedName = Regex.Replace(fileName.Name, pattern, "");
            cleanedNames.Add(cleanedName);
        }
        
        SongsList.ItemsSource = cleanedNames;
        SongsList.Items.Filter = FilterResults;*/
    }
    private bool FilterResults(object obj)
    {
        if (string.IsNullOrEmpty(SongSearchBox.Text))
        {
            return true;
        }else 
        {
            return (obj.ToString().IndexOf(SongSearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        //match items here with your TextBox value.. obj is an item from the list    
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        //CollectionViewSource.GetDefaultView(SongsList.ItemsSource).Refresh();
    }
}
