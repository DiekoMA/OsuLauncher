namespace OsuLauncher.Models;

public class SecretsConfiguration
{
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
    
    [JsonPropertyName("clientSecret")]
    public string ClientSecret { get; set; }
    
    [JsonPropertyName("redirectUrl")]
    public string RedirectUrl { get; set; }
}