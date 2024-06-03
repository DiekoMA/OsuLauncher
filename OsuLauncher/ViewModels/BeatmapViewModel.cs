using System.Collections.ObjectModel;
using HandyControl.Tools;
using Beatmap = API.Objects.Beatmap;

namespace OsuLauncher.ViewModels;

public partial class BeatmapViewModel : ViewModelBase
{
    [ObservableProperty] private ObservableCollection<string> _skins;
    [ObservableProperty] private ObservableCollection<BeatmapSet> _beatmaps;
    [ObservableProperty] private ObservableCollection<string> _songs;
    public BeatmapViewModel()
    {
        try
        {
            DirectoryInfo songsDirectory = new DirectoryInfo(LauncherSettings.Default.SongsDirectory);
            List<string> fileNames = new List<string>();
            ObservableCollection<string> cleanedNames = new ObservableCollection<string>();
            string pattern = @"^\d+\s";
            foreach (var fileName in songsDirectory.GetDirectories())
            {
                string cleanedName = Regex.Replace(fileName.Name, pattern, "");
                cleanedNames.Add(cleanedName);
            }

            Songs = cleanedNames;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}