using System.Windows.Documents;
using MdXaml;

namespace OsuLauncher.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<NewsPost> _newsPosts;
    [ObservableProperty] private NewsPost _selectedNewsPost;
    [ObservableProperty] private FlowDocument _newsFlowDocument;
    private HttpClient client;
    private OsuClient _osuClient;
    private Markdown engine;
    public HomeViewModel()
    {
        client = new HttpClient();
        _osuClient = new OsuClient();
        engine = new Markdown();
        GetClient();
        GetPosts();
    }

    private async void GetClient()
    {
        _osuClient = await ApiHelper.Instance.RetrieveClient();
    }

    private async void GetPosts()
    {
        NewsPosts = await _osuClient.GetNewsListingsAsync();
    }

    [RelayCommand]
    private async void GetNewsPostMarkdownContent()
    {
        try
        {
            var postYear = SelectedNewsPost.PublishedAt.Year;
            var rawMarkdown = await client.GetAsync($"https://raw.githubusercontent.com/ppy/osu-wiki/refs/heads/master/news/{postYear}/{SelectedNewsPost.Slug}.md");
            var content = await rawMarkdown.Content.ReadAsStringAsync();
            NewsFlowDocument = engine.Transform(content);
        }
        catch (Exception e)
        {
            Growl.Error(e.Message);
        }
    }
}