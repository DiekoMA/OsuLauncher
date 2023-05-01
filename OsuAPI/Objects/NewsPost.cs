using Newtonsoft.Json;

namespace OsuAPI.Objects;

public class NewsPost
{
    [JsonProperty("id")]
    public int ID { get; set; }
    
    [JsonProperty("author")]
    public string Author { get; set; }
    
    [JsonProperty("edit_url")]
    public string EditUrl { get; set; }
    
    [JsonProperty("first_image")]
    public string FirstImageUrl { get; set; }
    
    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }
    
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    [JsonProperty("slug")]
    public string Slug { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("content")]
    public string Content { get; set; }
}