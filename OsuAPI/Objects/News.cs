using Newtonsoft.Json;

namespace OsuAPI.Objects;

public class News
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
    
    [JsonProperty("preview")]
    public string Preview { get; set; }
}

public class NewsResponse
{
    [JsonProperty("news_posts")]
    public List<News> Data { get; set; }
}
