namespace OsuLauncher.Pages;

public partial class AccountPage : Page
{
    private OsuClient osuClient;
    public AccountPage()
    {
        InitializeComponent();
        GetUserInfo();
    }
    public async void GetUserInfo()
    {
        osuClient = await ApiHelper.Instance.RetrieveClient();
        if (osuClient.IsAuthenticated)
        {
            var authedUser = await osuClient.GetAuthenticatedUserAsync();
            //authedUser.UserStats.GlobalRank;
            AvatarBlock.Source = new BitmapImage(new Uri(authedUser.AvatarUrl));
            UsernameText.Text = authedUser.Username;
            PPRankText.Text = Math.Round(authedUser.UserStats.PP).ToString();
            AccuracyText.Text = Math.Round(authedUser.UserStats.HitAccuracy).ToString();
            LevelText.Text = authedUser.UserStats.Level.Current.ToString();
        }
    }
}