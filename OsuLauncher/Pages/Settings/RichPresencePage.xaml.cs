using System.Windows.Controls;

namespace OsuLauncher.Pages;

public partial class RichPresencePage : Page
{
    private OsuConfigHelper _configHelper;
    public RichPresencePage()
    {
        InitializeComponent();
        var osuCfg = Path.Combine(AppSettings.Default.GameDirectory,
            $"osu!.{Environment.UserName}.cfg");
        _configHelper = new OsuConfigHelper(osuCfg);
        if (AppSettings.Default.UseCustomRPC)
            CustomRpcCB.IsChecked = true;
    }

    private void CustomRpcCB_OnChecked(object sender, RoutedEventArgs e)
    {
        if (AppSettings.Default.GameDirectory != string.Empty)
        {
            _configHelper.EditValue("DiscordRichPresence", "0");
            AppSettings.Default.UseCustomRPC = true;
        }
    }
}