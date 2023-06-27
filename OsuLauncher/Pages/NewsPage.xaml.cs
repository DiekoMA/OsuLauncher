namespace OsuLauncher.Pages;

public partial class NewsPage : Page
{
    public NewsPage()
    {
        InitializeComponent();
        try
        {
            OsuClient client = new OsuClient();
            var listings = client.GetNewsListingsAsync().Result;
            foreach (var newsListing in listings.)
            {
                NewsListBox.Items.Add(newsListing.Data.Title);
            }
        }
        catch (Exception e)
        {
            Growl.Error(e.Message);
            MessageBox.Show(e.Message);
        }
    }
}