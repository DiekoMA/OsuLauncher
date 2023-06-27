using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OsuLauncher.Pages;

public partial class AccountPage : Page
{
    public AccountPage()
    {
        InitializeComponent();
        // if (ApiHelper.Instance.IsAuthenticated())
        // {
        //     var authedUser = ApiHelper.Instance.GetSelf();
        //     AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
        //     UsernameText.Text = authedUser.Username;
        //     PPRankText.Text = Math.Round(authedUser.UserStats.PP).ToString();
        //     AccuracyText.Text = Math.Round(authedUser.UserStats.HitAccuracy).ToString();
        //     LevelText.Text = authedUser.UserStats.Level.Current.ToString();
        // }
    }
}