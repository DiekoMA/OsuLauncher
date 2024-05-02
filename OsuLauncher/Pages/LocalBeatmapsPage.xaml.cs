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
