using System.Windows.Controls;
using OsuLauncher.Helpers;

namespace OsuLauncher.Pages.Settings;

public partial class GeneralSettings : Page
{
    public GeneralSettings()
    {
        InitializeComponent();
        GameDirectoryBox.Text = ConfigHelper.GetStringItem("preferences", "gamedir");
        SongsDirectoryBox.Text = ConfigHelper.GetStringItem("preferences", "songsdir");
        
        GameDirectoryBox.TextChanged += (sender, args) =>  ConfigHelper.SaveStringItem("preferences", "gamedir",GameDirectoryBox.Text); 
        SongsDirectoryBox.TextChanged += (sender, args) => ConfigHelper.SaveStringItem("preferences", "songsdir",SongsDirectoryBox.Text);
        UpdateCB.Checked += (sender, args) => ConfigHelper.SaveBool("preferences", "checkforupdates", true);
    }
}