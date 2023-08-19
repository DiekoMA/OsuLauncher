namespace OsuLauncher.Pages;

public partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        NewsBox.MouseDoubleClick += (sender, args) =>
        {
            var selectedNewsItem = (NewsPost)NewsBox.SelectedItem;
            var newsLink = string.Concat("https://osu.ppy.sh/home/news/", selectedNewsItem.Slug);
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