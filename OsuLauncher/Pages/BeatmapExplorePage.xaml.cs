namespace OsuLauncher.Pages;

/// <summary>
/// Surprisingly most of the code here ran with no errors the first time !
/// </summary>
public partial class BeatmapExplorePage : Page
{
    BeatmapClient beatmapClient;
    public BeatmapExplorePage()
    {
        InitializeComponent();
    }

    private async void SearchBar_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
    {
        var beatmapResults = await beatmapClient.SearchBeatmapAsync(SearchBox.Text);
        BeatmapList.ItemsSource = beatmapResults;
    }

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        beatmapClient = new BeatmapClient();
        BeatmapList.MouseDoubleClick += BeatmapList_MouseDoubleClick;
    }

    private async void BeatmapList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var selectedBeatmap = (BeatmapSet)BeatmapList.SelectedItem;

        var downloadOpt = new DownloadConfiguration()
        {
            ChunkCount = 8, // file parts to download, default value is 1
            ParallelDownload = true // download parts of file as parallel or not. Default value is false
        };

        var downloader = new DownloadService(downloadOpt);

        string file = Path.Combine("C:\\Users\\Mayowa\\Downloads\\test", $"{selectedBeatmap.Id} {selectedBeatmap.Artist} - {selectedBeatmap.Title}.osz");
        string url = $"https://chimu.moe/d/{selectedBeatmap.Id}";
        await downloader.DownloadFileTaskAsync(url, file);

        
        Growl.Info(selectedBeatmap.Title);
    }
}
