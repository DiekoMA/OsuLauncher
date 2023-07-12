using System.Windows.Controls;

namespace OsuLauncher.Pages;

public partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();
        GeneralLbItem.Selected += (sender, args) => SettingsFrame.Content = new GeneralSettings();
        GameLbItem.Selected += (sender, args) => SettingsFrame.Content = new OsuGameSettings();
        RpcLbItem.Selected += (sender, args) => SettingsFrame.Content = new RichPresencePage();
        
        
    }
}