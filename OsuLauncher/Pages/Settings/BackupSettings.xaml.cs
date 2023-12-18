namespace OsuLauncher.Pages.Settings;

/// <summary>
/// Interaction logic for BackupSettings.xaml
/// </summary>
public partial class BackupSettings : UserControl
{
    bool Songs;
    bool Skins;
    bool Settings;
    public BackupSettings()
    {
        InitializeComponent();
    }

    private void InitiateBackupButton_Click(object sender, RoutedEventArgs e)
    {
        if (!Directory.Exists("Backups"))
        {
            DirectoryInfo backupDir = new DirectoryInfo("Backups");
            
        }
    }
}
