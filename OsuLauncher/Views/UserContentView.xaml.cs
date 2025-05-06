using OsuLauncher.ViewModels;

namespace OsuLauncher.Views;

public partial class UserContentView : UserControl
{
    public UserContentView()
    {
        InitializeComponent();
        this.DataContext = new UserContentViewModel();
    }
}