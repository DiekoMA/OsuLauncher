namespace OsuLauncher.Pages;

/// <summary>
/// Surprisingly most of the code here ran with no errors the first time !
/// </summary>
public partial class BeatmapExplorePage : Page
{
    BeatmapClient beatmapClient;
    BeatmapSet currentDownloadingBeatmap;
    List<BeatmapDownload> beatmapQueue;
    public BeatmapExplorePage()
    {
        InitializeComponent();
        beatmapQueue = new List<BeatmapDownload>();
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
        if (BeatmapList.SelectedItem != null)
        {
            if (beatmapQueue == null)
            {
                beatmapQueue = new List<BeatmapDownload>();
            }

            var selectedBeatmap = (BeatmapSet)BeatmapList.SelectedItem;
            BeatmapDownload beatmapDownload = new BeatmapDownload();
            beatmapDownload.Title = selectedBeatmap.Title;
            QueueList.DataContext = new BeatmapDownload();
            QueueList.Items.Add(beatmapDownload);

            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 1, // file parts to download, default value is 1
                ParallelDownload = true // download parts of file as parallel or not. Default value is false
            };

            var downloader = new DownloadService(downloadOpt);
            string file = Path.Combine("C:\\Users\\Mayowa\\Documents\\test", $"{selectedBeatmap.Id} {selectedBeatmap.Artist} - {selectedBeatmap.Title}.osz");
            string url = $"https://chimu.moe/d/{selectedBeatmap.Id}";
            await downloader.DownloadFileTaskAsync(url, file);
        }

    }

    public class BeatmapDownload
    {
        public string Title { get; set; }
        public int Progress { get; set; }
    }
}
