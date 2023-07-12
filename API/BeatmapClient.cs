using API.Objects;
// using Newtonsoft.Json;
// using RestSharp;

namespace API;

public class BeatmapClient
{
    // private RestClient _restClient;
    // /*private Dictionary<string, string> queries = new Dictionary<string, string>(); */
    // public BeatmapClient()
    // {
    //     var options = new RestClientOptions("https://api.chimu.moe")
    //     {
    //         MaxTimeout = -1,
    //     };
    //     _restClient = new RestClient(options);
    // }
    //
    // public string DownloadBeatmapSetById(int id)
    // {
    //     var request = new RestRequest($"");
    //     request.AddHeader("Content-Type", "application/octet-stream");
    //    
    //
    //     RestResponse response = _restClient.Execute(request);
    //     var beatmapSetDownloadResponse = JsonConvert.DeserializeObject(response.Content ?? throw new InvalidOperationException());
    //     return (string)beatmapSetDownloadResponse;
    // }
    //
    // public BeatmapResponse? SearchBeatmapSet(string query, int amount = 100, int offset = 0, int status = 0, int mode = 0, int minAR = 0, int maxAR = 0, int minOD = 0, int maxOD = 0, int minCS = 0, int maxCS = 0, int minHP = 0, int maxHP = 0, int minDiff = 0, int maxDiff = 0,int minBPM = 0, int maxBPM = 0, int minLength = 0, int maxLength = 0, int genre = 0, int language = 0)
    // {
    //     var request = new RestRequest($"/v1/search");
    //     request.AddHeader("Content-Type", "application/json");
    //     request.AddHeader("Accept", "application/json");
    //     /*queries.Add("", "");
    //     foreach (KeyValuePair<string, string> queryPair in queries)
    //     {
    //         request.AddQueryParameter(queryPair.Key, queryPair.Value);
    //     }*/
    //     request.AddParameter("query", query);
    //     request.AddParameter("amount", amount);
    //     request.AddParameter("offset", offset);
    //     request.AddParameter("status", status);
    //     request.AddParameter("mode", mode);
    //     request.AddParameter("min_ar", minAR);
    //     request.AddParameter("max_ar", maxAR);
    //     request.AddParameter("min_od", minOD);
    //     request.AddParameter("max_od", maxOD);
    //     request.AddParameter("min_cs", minCS);
    //     request.AddParameter("max_cs", maxCS);
    //     request.AddParameter("min_hp", minHP);
    //     request.AddParameter("max_hp", maxHP);
    //     request.AddParameter("min_diff", minDiff);
    //     request.AddParameter("max_diff", maxDiff);
    //     request.AddParameter("min_bpm", minBPM);
    //     request.AddParameter("max_bpm", maxBPM);
    //     request.AddParameter("min_length", minLength);
    //     request.AddParameter("max_length", maxLength);
    //     request.AddParameter("genre", genre);
    //     request.AddParameter("language", language);
    //
    //     RestResponse response = _restClient.Execute(request);
    //     var beatmapSetResponse = JsonConvert.DeserializeObject<BeatmapResponse>(response.Content ?? throw new InvalidOperationException());
    //     return beatmapSetResponse;
    // }
    //
    // public BeatmapSet GetBeatmapSetById(int id)
    // {
    //     var request = new RestRequest($"/api/s/{id}");
    //     request.AddHeader("Content-Type", "application/json");
    //     request.AddHeader("Accept", "application/json");
    //     
    //     RestResponse response = _restClient.Execute(request);
    //     var beatmapSetResponse = JsonConvert.DeserializeObject<BeatmapSet>(response.Content ?? throw new InvalidOperationException());
    //     return beatmapSetResponse;
    // }
}