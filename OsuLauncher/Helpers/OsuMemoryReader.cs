using OsuMemoryDataProvider;
using OsuMemoryDataProvider.OsuMemoryModels;
using OsuMemoryDataProvider.OsuMemoryModels.Direct;

namespace OsuLauncher.Helpers;

public class OsuMemoryReader
{
    private int _readDelay = 100;
    private double _memoryReadTimeMin = double.PositiveInfinity;
    private double _memoryReadTimeMax = double.NegativeInfinity;
    private bool osuOpen;

    private readonly StructuredOsuMemoryReader _sreader;
    private CancellationTokenSource cts = new CancellationTokenSource();
    public OsuMemoryReader()
    {
        _sreader = StructuredOsuMemoryReader.Instance.GetInstanceForWindowTitleHint("osu!");

        /*Loaded += OnLoaded;
        Closing += OnClosing;*/
    }

    public async Task<OsuBaseAddresses> GetCurrentBeatmap()
    {
        var baseAddresses = new OsuBaseAddresses();
        baseAddresses.Beatmap.Id = ReadInt(baseAddresses.Beatmap, nameof(CurrentBeatmap.Id));
        baseAddresses.Beatmap.SetId = ReadInt(baseAddresses.Beatmap, nameof(CurrentBeatmap.SetId));
        baseAddresses.Beatmap.MapString = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.MapString));
        baseAddresses.Beatmap.FolderName = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.FolderName));
        baseAddresses.Beatmap.OsuFileName = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.OsuFileName));
        baseAddresses.Beatmap.Md5 = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.Md5));
        baseAddresses.Beatmap.Ar = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Ar));
        baseAddresses.Beatmap.Cs = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Cs));
        baseAddresses.Beatmap.Hp = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Hp));
        baseAddresses.Beatmap.Od = ReadFloat(baseAddresses.Beatmap, nameof(CurrentBeatmap.Od));
        baseAddresses.Beatmap.Status = ReadShort(baseAddresses.Beatmap, nameof(CurrentBeatmap.Status));
        baseAddresses.Skin.Folder = ReadString(baseAddresses.Skin, nameof(Skin.Folder));
        baseAddresses.GeneralData.RawStatus = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.RawStatus));
        baseAddresses.GeneralData.GameMode = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.GameMode));
        baseAddresses.GeneralData.Retries = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.Retries));
        baseAddresses.GeneralData.AudioTime = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.AudioTime));
        baseAddresses.GeneralData.Mods = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.Mods));

        return baseAddresses;
    }

    private T ReadProperty<T>(object readObj, string propName, T defaultValue = default) where T : struct
    {
        if (_sreader.TryReadProperty(readObj, propName, out var readResult))
            return (T)readResult;

        return defaultValue;
    }

    private T ReadClassProperty<T>(object readObj, string propName, T defaultValue = default) where T : class
    {
        if (_sreader.TryReadProperty(readObj, propName, out var readResult))
            return (T)readResult;

        return defaultValue;
    }

    private int ReadInt(object readObj, string propName)
        => ReadProperty<int>(readObj, propName, -5);
    private short ReadShort(object readObj, string propName)
        => ReadProperty<short>(readObj, propName, -5);

    private float ReadFloat(object readObj, string propName)
        => ReadProperty<float>(readObj, propName, -5f);

    private string ReadString(object readObj, string propName)
        => ReadClassProperty<string>(readObj, propName, "INVALID READ");
}
