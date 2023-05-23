using Newtonsoft.Json;

namespace API.Objects;

public class Wiki
{
    [JsonProperty("available_locales")]
    public string[] AvailabeLocales { get; set; } = null!;

    [JsonProperty("layout")]
    public string Layout { get; set; } = null!;

    [JsonProperty("locale")]
    public string Locale { get; set; } = null!;

    [JsonProperty("markdown")]
    public string Content { get; set; } = null!;

    [JsonProperty("path")]
    public string Path { get; set; } = null!;

    [JsonProperty("subtitle")]
    public string Subtitle { get; set; } = null!;

    [JsonProperty("tags")]
    public string[] Tags { get; set; } = null!;

    [JsonProperty("title")]
    public string Title { get; set; } = null!;
}