using System.Text.Json.Serialization;

namespace API.Objects;

public class BeatmapSet
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string TitleUnicode { get; set; }

    public string Artist { get; set; }

    public string Creator { get; set; }

    public string Source { get; set; }

    public string Tags { get; set; }

    public ImageCovers Covers { get; set; }

    public int FavouriteCount { get; set; }

    public BeatmapHype Hype { get; set; }

    public bool NSFW { get; set; }

    public int Offset { get; set; }

    public int PlayCount { get; set; }

    public string PreviewUrl { get; set; }

    public bool Spotlight { get; set; }

    public string Status { get; set; }

    public int TrackId { get; set; }

    public int UserId { get; set; }

    public bool Video { get; set; }

    public float BPM { get; set; }

    public bool CanBeHyped { get; set; }

    public string DeletedAt { get; set; }

    public bool DiscussionEnabled { get; set; }

    public bool DiscussionLocked { get; set; }

    public bool IsScoreable { get; set; }

    public string LastUpdated { get; set; }

    public string LegacyThreadUrl { get; set; }

    public NominationSummary NominationSummary { get; set; }

    public int Ranked { get; set; }

    public string RankedDate { get; set; }

    public bool Storyboard { get; set; }

    public string SubmittedDate { get; set; }

    public DownloadAvailability Availability { get; set; }

    public bool HasFavourited { get; set; }

    public Beatmap[] Beatmaps { get; set; }

    public string[] PackTags { get; set; }

    public int[] Modes { get; set; }

    public string LastChecked { get; set; }

}


public class Beatmap
{
    public int BeatmapSetId { get; set; }
    public float DifficultyRating { get; set; }
    public int Id { get; set; }
    public string Mode { get; set; }
    public string Status { get; set; }
    public int TotalLength { get; set; }
    public int UserId { get; set; }
    public string Version { get; set; }
    public float Accuracy { get; set; }
    public float AR { get; set; }
    public float BPM { get; set; }
    public bool Convert { get; set; }
    public int CountCircles { get; set; }
    public int CountSliders { get; set; }
    public int CountSpinners { get; set; }
    public float CS { get; set; }
    public string DeletedAt { get; set; }
    public float Drain { get; set; }
    public int HitLength { get; set; }
    public bool IsScoreable { get; set; }
    public string LastUpdated { get; set; }
    public int ModeInt { get; set; }
    public int Passcount { get; set; }
    public int Playcount { get; set; }
    public int Ranked { get; set; }
    public string Url { get; set; }
    public string Checksum { get; set; }
    public int MaxCombo { get; set; }
}


public class BeatmapHype
{
    public int Current { get; set; }

    public int Required { get; set; }
}

public class DownloadAvailability
{
    public bool DownloadDisabled { get; set; }

    public bool MoreInformation { get; set; }
}

public class NominationSummary
{
    public int Current { get; set; }

    public int Required { get; set; }
}

public class ImageCovers
{
    public string Cover { get; set; }

    [JsonPropertyName("cover@2x")]
    public string Cover2x { get; set; }

    [JsonPropertyName("card")]
    public string Card { get; set; }

    [JsonPropertyName("card@2x")]
    public string Card2x { get; set; }

    [JsonPropertyName("list")]
    public string List { get; set; }

    [JsonPropertyName("list@2x")]
    public string List2x { get; set; }

    [JsonPropertyName("slimcovercover")]
    public string SlimCover { get; set; }

    [JsonPropertyName("slimcover@2x")]
    public string SlimCover2x { get; set; }
}

public class BeatmapSetResponse
{
    public List<BeatmapSet> Data { get; set; }
}
