using System.Windows.Controls;
using OsuLauncher.Dialogs;
using OsuLauncher.Helpers;

namespace OsuLauncher.Pages.Settings;

public partial class AccountSettings : Page
{
    public AccountSettings()
    {
        InitializeComponent();
        InitAuthButton.Click += (sender, args) =>  DialogHelper.ShowDialog(typeof(AuthDialog));
    }
}