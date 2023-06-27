using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace API.Objects;

public class User
{
    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; } = null!;

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; } = null!;

    [JsonPropertyName("default_group")]
    public string DefaultGroup { get; set; } = null!;

    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
    
    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }
    
    [JsonPropertyName("is_online")]
    public bool IsOnline { get; set; }
    
    [JsonPropertyName("is_supporter")]
    public bool IsSupporter { get; set; }
    
    [JsonPropertyName("last_visit")]
    public DateTime LastVisit { get; set; }
    
    [JsonPropertyName("pm_friends_only")]
    public bool PmFriendsOnly { get; set; }
    
    [JsonPropertyName("profile_colour")]
    public string ProfileColour { get; set; } = null!;

    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("cover_url")]
    public string CoverUrl { get; set; } = null!;

    [JsonPropertyName("discord")]
    public string Discord { get; set; } = null!;

    [JsonPropertyName("has_supported")]
    public bool HasSupported { get; set; }
    
    [JsonPropertyName("join_date")]
    public DateTime JoinDate { get; set; }
    
    [JsonPropertyName("kudosu")]
    public Kudosu Kudosu { get; set; }
    
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [JsonPropertyName("max_blocks")]
    public int MaxBlocks { get; set; }
    
    [JsonPropertyName("max_friends")]
    public int MaxFriends { get; set; }
    
    [JsonPropertyName("occupation")]
    public string Occupation { get; set; } = null!;

    [JsonPropertyName("playmode")]
    public string Playmode { get; set; } = null!;

    [JsonPropertyName("playstyle")]
    public string[] Playstyle { get; set; } = null!;

    [JsonPropertyName("post_count")]
    public int PostCount { get; set; }
    
    [JsonPropertyName("profile_order")]
    public string[] ProfileOrder { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("title_url")]
    public string TitleUrl { get; set; } = null!;

    [JsonPropertyName("twitter")]
    public string Twitter { get; set; } = null!;

    [JsonPropertyName("country")]
    public Country Country { get; set; }
    
    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }
    
    [JsonPropertyName("statistics")]
    public Statistics UserStats { get; set; }
}

public struct Kudosu
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
    
    [JsonPropertyName("available")]
    public int Available { get; set; }
}

public struct Country
{
    [JsonPropertyName("code")]
    public string Code { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public struct Cover
{
    [JsonPropertyName("custom_url")]
    public string CustomUrl { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
}

public struct Statistics
{
    [JsonPropertyName("count_100")]
    public int Count100 { get; set; }
    
    [JsonPropertyName("count_300")]
    public int Count300 { get; set; }
    
    [JsonPropertyName("count_50")]
    public int Count50 { get; set; }
    
    [JsonPropertyName("count_miss")]
    public int CountMiss { get; set; }
    
    [JsonPropertyName("level")]
    public Level Level { get; set; }
    
    [JsonPropertyName("global_rank")]
    public int GlobalRank { get; set; }
    
    [JsonPropertyName("global_rank_exp")]
    public int GlobalRankExp { get; set; }
    
    [JsonPropertyName("pp")]
    public float PP { get; set; }
    
    [JsonPropertyName("pp_exp")]
    public float PPExp { get; set; }
    
    [JsonPropertyName("ranked_score")]
    public int RankedScore { get; set; }
    
    [JsonPropertyName("hit_accuracy")]
    public float HitAccuracy { get; set; }
    
    [JsonPropertyName("play_count")]
    public int PlayCount { get; set; }
    
    [JsonPropertyName("play_time")]
    public int PlayTime { get; set; }
    
    [JsonPropertyName("total_score")]
    public int TotalScore { get; set; }
    
    [JsonPropertyName("total_hits")]
    public int TotalHits { get; set; }
    
    [JsonPropertyName("maximum_combo")]
    public int MaxCombo { get; set; }
    
    [JsonPropertyName("replays_watched_by_others")]
    public int ReplaysViewedByOthers { get; set; }
    
    [JsonPropertyName("is_ranked")]
    public bool IsRanked { get; set; }
    
    [JsonPropertyName("grade_counts")]
    public GradeCounts GradeCounts { get; set; }
    
    [JsonPropertyName("country_rank")]
    public int CountryRank { get; set; }
    
    [JsonPropertyName("rank")]
    public Rank Rank { get; set; }
}

public struct Level
{
    [JsonPropertyName("current")]
    public int Current { get; set; }
    
    [JsonPropertyName("progress")]
    public int Progress { get; set; }
}

public struct GradeCounts {
    [JsonPropertyName("ss")]
    public int SS { get; set; }
    
    [JsonPropertyName("ssh")]
    public int SSH { get; set; }
    
    [JsonPropertyName("s")]
    public int S { get; set; }
    
    [JsonPropertyName("a")]
    public int A { get; set; }
}

public struct Rank
{
    [JsonPropertyName("country")]
    public int Country { get; set; }
}