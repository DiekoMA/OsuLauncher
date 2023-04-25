using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace OsuAPI.Objects;

public class User
{
    [JsonProperty("avatar_url")]
    public string AvatarURL { get; set; }
    
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }
    
    [JsonProperty("default_group")]
    public string DefaultGroup { get; set; }
    
    [JsonProperty("id")]
    public int ID { get; set; }
    
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    
    [JsonProperty("is_bot")]
    public bool IsBot { get; set; }
    
    [JsonProperty("is_online")]
    public bool IsOnline { get; set; }
    
    [JsonProperty("is_supporter")]
    public bool IsSupporter { get; set; }
    
    [JsonProperty("last_visit")]
    public DateTime LastVisit { get; set; }
    
    [JsonProperty("pm_friends_only")]
    public bool PmFriendsOnly { get; set; }
    
    [JsonProperty("profile_colour")]
    public string ProfileColour { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("cover_url")]
    public string CoverUrl { get; set; }
    
    [JsonProperty("discord")]
    public string Discord { get; set; }
    
    [JsonProperty("has_supported")]
    public bool HasSupported { get; set; }
    
    [JsonProperty("join_date")]
    public DateTime JoinDate { get; set; }
    
    [JsonProperty("kudosu")]
    public Kudosu Kudosu { get; set; }
    
    [JsonProperty("location")]
    public string Location { get; set; }
    
    [JsonProperty("max_blocks")]
    public int MaxBlocks { get; set; }
    
    [JsonProperty("max_friends")]
    public int MaxFriends { get; set; }
    
    [JsonProperty("occupation")]
    public string Occupation { get; set; }
    
    [JsonProperty("playmode")]
    public string Playmode { get; set; }
    
    [JsonProperty("playstyle")]
    public string[] Playstyle { get; set; }
    
    [JsonProperty("post_count")]
    public int PostCount { get; set; }
    
    [JsonProperty("profile_order")]
    public string[] ProfileOrder { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("title_url")]
    public string TitleUrl { get; set; }
    
    [JsonProperty("twitter")]
    public string Twitter { get; set; }
    
    [JsonProperty("country")]
    public Country Country { get; set; }
    
    [JsonProperty("cover")]
    public Cover Cover { get; set; }
    
    [JsonProperty("statistics")]
    public Statistics UserStats { get; set; }
}

public struct Kudosu
{
    [JsonProperty("total")]
    public int Total { get; set; }
    
    [JsonProperty("available")]
    public int Available { get; set; }
}

public struct Country
{
    [JsonProperty("code")]
    public string Code { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
}

public struct Cover
{
    [JsonProperty("custom_url")]
    public string CustomUrl { get; set; }
    
    [JsonProperty("url")]
    public string Url { get; set; }
    
    [JsonProperty("id")]
    public int ID { get; set; }
}

public struct Statistics
{
    [JsonProperty("count_100")]
    public int Count100 { get; set; }
    
    [JsonProperty("count_300")]
    public int Count300 { get; set; }
    
    [JsonProperty("count_50")]
    public int Count50 { get; set; }
    
    [JsonProperty("count_miss")]
    public int CountMiss { get; set; }
    
    [JsonProperty("level")]
    public Level Level { get; set; }
    
    [JsonProperty("global_rank")]
    public int GlobalRank { get; set; }
    
    [JsonProperty("global_rank_exp")]
    public int GlobalRankExp { get; set; }
    
    [JsonProperty("pp")]
    public float PP { get; set; }
    
    [JsonProperty("pp_exp")]
    public float PPExp { get; set; }
    
    [JsonProperty("ranked_score")]
    public int RankedScore { get; set; }
    
    [JsonProperty("hit_accuracy")]
    public float HitAccuracy { get; set; }
    
    [JsonProperty("play_count")]
    public int PlayCount { get; set; }
    
    [JsonProperty("play_time")]
    public int PlayTime { get; set; }
    
    [JsonProperty("total_score")]
    public int TotalScore { get; set; }
    
    [JsonProperty("total_hits")]
    public int TotalHits { get; set; }
    
    [JsonProperty("maximum_combo")]
    public int MaxCombo { get; set; }
    
    [JsonProperty("replays_watched_by_others")]
    public int ReplaysViewedByOthers { get; set; }
    
    [JsonProperty("is_ranked")]
    public bool IsRanked { get; set; }
    
    [JsonProperty("grade_counts")]
    public GradeCounts GradeCounts { get; set; }
    
    [JsonProperty("country_rank")]
    public int CountryRank { get; set; }
    
    [JsonProperty("rank")]
    public Rank Rank { get; set; }
}

public struct Level
{
    [JsonProperty("current")]
    public int Current { get; set; }
    
    [JsonProperty("progress")]
    public int Progress { get; set; }
}

public struct GradeCounts {
    [JsonProperty("ss")]
    public int SS { get; set; }
    
    [JsonProperty("ssh")]
    public int SSH { get; set; }
    
    [JsonProperty("s")]
    public int S { get; set; }
    
    [JsonProperty("a")]
    public int A { get; set; }
}

public struct Rank
{
    [JsonProperty("country")]
    public int Country { get; set; }
}