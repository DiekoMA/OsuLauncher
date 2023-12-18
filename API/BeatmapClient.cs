using API.Objects;
using System.Text.Json;
// using Newtonsoft.Json;
// using RestSharp;

namespace API;

public class BeatmapClient
{
    private HttpClient client;
    private readonly string baseUrl = "https://osu.direct/api/v2";
    private JsonSerializerOptions serializerOptions;
    public BeatmapClient()
    {
        
        client = new HttpClient
        {
            /*BaseAddress = new Uri(baseUrl),*/
        };
        serializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<List<BeatmapSet>> SearchBeatmapAsync(string query, int amount = 100, int offset = 0, int status = 0, int mode = 0, int minAR = 0, int maxAR = 0, int minOD = 0, int maxOD = 0, int minCS = 0, int maxCS = 0, int minHP = 0, int maxHP = 0, int minDiff = 0, int maxDiff = 0, int minBPM = 0, int maxBPM = 0, int minLength = 0, int maxLength = 0, int genre = 0, int language = 0)
    {
        var beatmapRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{baseUrl}/search?query={query}&amount={amount}"),
            Headers =
            {
                { "Accept", "application/json" }
            },
            
        };

        using var response = await client.SendAsync(beatmapRequest);
        response.EnsureSuccessStatusCode();
        var beatmap = JsonSerializer.Deserialize<List<BeatmapSet>>(response.Content.ReadAsStringAsync().Result ?? throw new InvalidOperationException(), serializerOptions);
        return beatmap;
    }
}