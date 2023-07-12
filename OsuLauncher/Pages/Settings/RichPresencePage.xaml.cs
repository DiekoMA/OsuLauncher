using System.Windows.Controls;

namespace OsuLauncher.Pages;

public partial class RichPresencePage : Page
{
    private OsuConfigHelper _configHelper;
    public RichPresencePage()
    {
        InitializeComponent();
        var osuCfg = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"),
            $"osu!.{Environment.UserName}.cfg");
        _configHelper = new OsuConfigHelper(osuCfg);
    }

    private void CustomRpcCB_OnChecked(object sender, RoutedEventArgs e)
    {
        if (ConfigHelper.GetStringItem("preferences", "gamedir") != string.Empty)
        {
            _configHelper.EditValue("DiscordRichPresence", "0");
            AppUtils.Config.SaveBool("preferences", "customRpc", true);
        }
    }
}