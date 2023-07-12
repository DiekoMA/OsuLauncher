using System.Text.Json.Serialization;

namespace API.Objects;

public class Wiki
{
    [JsonPropertyName("available_locales")]
    public string[] AvailabeLocales { get; set; } = null!;

    [JsonPropertyName("layout")]
    public string Layout { get; set; } = null!;

    [JsonPropertyName("locale")]
    public string Locale { get; set; } = null!;

    [JsonPropertyName("markdown")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("path")]
    public string Path { get; set; } = null!;

    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; } = null!;

    [JsonPropertyName("tags")]
    public string[] Tags { get; set; } = null!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
}