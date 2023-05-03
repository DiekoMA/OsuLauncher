using System.Windows;
using System.Windows.Controls;

namespace OsuLauncher.Pages;

public partial class BeatmapPage : Page
{
    public BeatmapPage()
    {
        InitializeComponent();
        DownloadPanelButton.Click += (sender, args) =>
        {
            if (DownloadsBox.Visibility == Visibility.Collapsed)
            {
                DownloadsBox.Visibility = Visibility.Visible;
            }
            else
            {
                DownloadsBox.Visibility = Visibility.Collapsed;
            }
        };
    }
}