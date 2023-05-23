using System.Windows;

namespace OsuLauncher.Dialogs;

public partial class WarningDialog
{
    public WarningDialog()
    {
        InitializeComponent();
    }

    private void AcceptButton(object sender, RoutedEventArgs e)
    {
        ConfigHelper.SaveBool("User_Preference", "beatmapmirroroptin", true);
    }

    private void DeclineButton(object sender, RoutedEventArgs e)
    {
        ConfigHelper.SaveBool("User_Preference", "beatmapmirroroptin", false);
    }

    private void DialogOptCB_OnChecked(object sender, RoutedEventArgs e)
    {
        ConfigHelper.SaveBool("User_Preference", "dialogoptin", true);
    }

    private void DialogOptCB_OnUnchecked(object sender, RoutedEventArgs e)
    {
        ConfigHelper.SaveBool("User_Preference", "dialogoptin", false);
    }
}