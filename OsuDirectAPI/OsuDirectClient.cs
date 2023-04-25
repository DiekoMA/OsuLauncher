using Newtonsoft.Json;
using OsuDirectAPI.Objects;
using RestSharp;

namespace OsuDirectAPI;

public class OsuDirectClient
{
    private RestClient _restClient;
    public OsuDirectClient()
    {
        var options = new RestClientOptions("https://osu.direct")
        {
            MaxTimeout = -1,
        };
        _restClient = new RestClient(options);
    }

    public string DownloadBeatmapSetById(int id)
    {
        var request = new RestRequest($"/api/d/{id}");
        request.AddHeader("Content-Type", "application/octet-stream");

        RestResponse response = _restClient.Execute(request);
        var beatmapSetDownloadResponse = JsonConvert.DeserializeObject(response.Content ?? throw new InvalidOperationException());
        return (string)beatmapSetDownloadResponse;
    }

    public BeatmapSet SearchBeatmapSet(string beatmapName, int mode ,int amount)
    {
        var request = new RestRequest($"/api/search/");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        
        RestResponse response = _restClient.Execute(request);
        var beatmapSetResponse = JsonConvert.DeserializeObject<BeatmapSet>(response.Content ?? throw new InvalidOperationException());
        return beatmapSetResponse;
    }
    
    public BeatmapSet GetBeatmapSetById(int id)
    {
        var request = new RestRequest($"/api/s/{id}");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        
        RestResponse response = _restClient.Execute(request);
        var beatmapSetResponse = JsonConvert.DeserializeObject<BeatmapSet>(response.Content ?? throw new InvalidOperationException());
        return beatmapSetResponse;
    }
}