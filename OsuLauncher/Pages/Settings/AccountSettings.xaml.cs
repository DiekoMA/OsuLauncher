using System.Diagnostics;
using System.Windows.Controls;
using OsuLauncher.Dialogs;
using OsuLauncher.Helpers;

namespace OsuLauncher.Pages.Settings;

public partial class AccountSettings : Page
{
    public AccountSettings()
    {
        InitializeComponent();
        InitAuthButton.Click += (sender, args) =>
        {
            Process.Start(new ProcessStartInfo("https://osu.ppy.sh/oauth/authorize?client_id=21770&redirect_uri=OsuLauncher%3A%2F%2Flocalhost%3A7040&response_type=code&scope=public+identify&state=randomval")
            {
                UseShellExecute = true
            });
        }; //DialogHelper.ShowDialog(typeof(AuthDialog));
    }
}