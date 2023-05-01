using Newtonsoft.Json;

namespace OsuAPI.Objects;

public class Wiki
{
    [JsonProperty("available_locales")]
    public string[] AvailabeLocales { get; set; }
    
    [JsonProperty("layout")]
    public string Layout { get; set; }
    
    [JsonProperty("locale")]
    public string Locale { get; set; }
    
    [JsonProperty("markdown")]
    public string Content { get; set; }
    
    [JsonProperty("path")]
    public string Path { get; set; }
    
    [JsonProperty("subtitle")]
    public string Subtitle { get; set; }
    
    [JsonProperty("tags")]
    public string[] Tags { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
}