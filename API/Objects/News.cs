using System.Collections;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace API.Objects;

public class NewsPost
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("author")]
    public string? Author { get; set; }
    
    [JsonPropertyName("edit_url")]
    public string? EditUrl { get; set; }
    
    [JsonPropertyName("first_image")]
    public string? FirstImageUrl { get; set; }
    
    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
    
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("preview")]
    public string? Preview { get; set; }
}

public class NewsResponse
{
    [JsonPropertyName("news_posts")]
    public NewsResponse? Data { get; set; }
}
