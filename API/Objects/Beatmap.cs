using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace API.Objects;

public class BeatmapSet
{
    [JsonProperty("SetId")]
    public int SetId { get; set; }
    
    [JsonProperty("Title")]
    public string? Title { get; set; }
    
    [JsonProperty("Artist")]
    public String? Artist { get; set; }
    
    [JsonProperty("Creator")]
    public string? Creator { get; set; }

    [JsonProperty("Source")]
    public string? Source { get; set; }
    
    [JsonProperty("Tags")]
    public string? Tags { get; set; }
    
    [JsonProperty("RankedStatus")]
    public int RankedStatus { get; set; }

    [JsonProperty("Genre")]
    public int Genre { get; set; }

    [JsonProperty("Language")]
    public int Language { get; set; }

    [JsonProperty("Favourites")]
    public int Favourites { get; set; }

    [JsonProperty("HasVideo")]
    public bool HasVideo { get; set; }

    [JsonProperty("DownloadUnavailable")]
    public bool DownloadUnavailable { get; set; }

    [JsonProperty("ApprovedDate")]
    public string? ApprovedDate { get; set; }

    [JsonProperty("LastUpdate")]
    public string? LastUpdate { get; set; }

    [JsonProperty("LastChecked")]
    public string? LastChecked { get; set; }

    [JsonProperty("ChildrenBeatmaps")]
    public Beatmap[]? Beatmaps { get; set; }
}

public class Beatmap
{
    [JsonProperty("ParentSetID")]
    public int ParentSetId { get; set; }
    
    [JsonProperty("BeatmapID")]
    public int BeatmapId { get; set; }
    
    [JsonProperty("TotalLength")]
    public int TotalLength { get; set; }
    
    [JsonProperty("HitLength")]
    public int HitLength { get; set; }
    
    [JsonProperty("DiffName")]
    public string? DifficultyName { get; set; }
    
    [JsonProperty("FileMD5")]
    public string? FileMd5 { get; set; }
    
    [JsonProperty("CS")]
    public float CS { get; set; }
    
    [JsonProperty("AR")]
    public float AR { get; set; }
    
    [JsonProperty("HP")]
    public float HP { get; set; }
    
    [JsonProperty("OD")]
    public float OverallDifficulty { get; set; }
    
    [JsonProperty("Mode")]
    public float Mode { get; set; }
    
    [JsonProperty("BPM")]
    public float Bpm { get; set; }
    
    [JsonProperty("Playcount")]
    public int PlayCount { get; set; }
    
    [JsonProperty("Passcount")]
    public int PassCount { get; set; }
    
    [JsonProperty("MaxCombo")]
    public int MaxCombo { get; set; }
    
    [JsonProperty("DifficultyRating")]
    public float DifficultyRating { get; set; }
}

public class BeatmapResponse
{
    [JsonProperty("data")]
    public List<BeatmapSet>? Data { get; set; }
}
