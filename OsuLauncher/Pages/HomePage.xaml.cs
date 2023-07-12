namespace OsuLauncher.Pages;

public partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        NewsBox.MouseDoubleClick += (sender, args) =>
        {
            var selectedNewsItem = (NewsPost)NewsBox.SelectedItem;
            var adjustedDate = $"{selectedNewsItem.PublishedAt.Year}-{selectedNewsItem.PublishedAt.Month.ToString("00")}-{selectedNewsItem.PublishedAt.Day.ToString("00")}-";
            var adjustedTitle = selectedNewsItem.Title.Replace(" ", "-");
            var newsLink = string.Concat("https://osu.ppy.sh/home/news/", adjustedDate, adjustedTitle.ToLower());
            Process.Start(new ProcessStartInfo(newsLink)
            {
                UseShellExecute = true
            });
        };
    }
    
    private async void HomePage_OnLoaded(object sender, RoutedEventArgs e)
    {
        var osuClient = await ApiHelper.Instance.RetrieveClient();
        if (osuClient.IsAuthenticated)
        {
            var listings = await osuClient.GetNewsListingsAsync();
            NewsBox.ItemsSource = listings;
        }
            
    }
}