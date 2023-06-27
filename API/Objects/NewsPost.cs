using Newtonsoft.Json;

namespace API.Objects;

public class News
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("author")]
    public string Author { get; set; } = null!;

    [JsonProperty("edit_url")]
    public string EditUrl { get; set; } = null!;

    [JsonProperty("first_image")]
    public string FirstImageUrl { get; set; } = null!;

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [JsonProperty("slug")]
    public string Slug { get; set; } = null!;

    [JsonProperty("title")]
    public string Title { get; set; } = null!;

    [JsonProperty("content")]
    public string Content { get; set; } = null!;
}