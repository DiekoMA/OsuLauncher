namespace OsuLauncher.Pages.Settings;

public partial class GeneralSettings : Page
{
    public GeneralSettings()
    {
        InitializeComponent();
        GameDirectoryBox.Text = AppUtils.Config.GetStringItem("preferences", "gamedir");
        SongsDirectoryBox.Text = AppUtils.Config.GetStringItem("preferences", "songsdir");
        
        GameDirectoryBox.TextChanged += (sender, args) =>  AppUtils.Config.SaveStringItem("preferences", "gamedir",GameDirectoryBox.Text); 
        SongsDirectoryBox.TextChanged += (sender, args) => AppUtils.Config.SaveStringItem("preferences", "songsdir",SongsDirectoryBox.Text);
        UpdateCB.Checked += (sender, args) => AppUtils.Config.SaveBool("preferences", "checkforupdates", true);
    }

    private void DialogOptCB_OnChecked(object sender, RoutedEventArgs e)
    {
        AppUtils.Config.SaveBool("User_Preference", "beatmapmirroroptin", true);
    }

    private void DialogOptCB_OnUnchecked(object sender, RoutedEventArgs e)
    {
        AppUtils.Config.SaveBool("User_Preference", "beatmapmirroroptin", false);
    }
}