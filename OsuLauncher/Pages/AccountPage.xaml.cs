using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OsuLauncher.Pages;

public partial class AccountPage : Page
{
    private OsuClient osuClient;
    public AccountPage()
    {
        InitializeComponent();
        osuClient = ApiHelper.Instance.RetrieveClient().Result;
    }
    public async void GetUserInfo()
    {
        if (osuClient.IsAuthenticated)
        {
            var authedUser = await osuClient.GetAuthenticatedUserAsync();
            AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
            UsernameText.Text = authedUser.Username;
            PPRankText.Text = Math.Round(authedUser.UserStats.PP).ToString();
            AccuracyText.Text = Math.Round(authedUser.UserStats.HitAccuracy).ToString();
            LevelText.Text = authedUser.UserStats.Level.Current.ToString();
        }
    }
}