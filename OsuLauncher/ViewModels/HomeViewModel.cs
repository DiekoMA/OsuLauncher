using System.Collections.ObjectModel;

namespace OsuLauncher.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<NewsPost> _newsPosts;
    private OsuClient _osuClient;
    public HomeViewModel()
    {
        _osuClient = new OsuClient();
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
}