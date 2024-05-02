using System.Collections.ObjectModel;

namespace OsuLauncher.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<NewsPost> _newsPosts;
    public HomeViewModel()
    {
        
    }

    
    
}