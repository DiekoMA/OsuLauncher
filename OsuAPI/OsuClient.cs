using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OsuAPI.Objects;
using Refit;
using RestSharp;

namespace OsuAPI;

public class OsuClient
{
    private RestClient _restClient;
    private string _token;
    private readonly IOsuClient _client;
    public static RefitSettings Settings = new RefitSettings()
    {
        ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        })
    };
    
    public OsuClient(string token = "")
    {
        _token = token;
        _client = RestService.For<IOsuClient>("https://osu.ppy.sh/api/v2", Settings);
    }

    public bool IsAuthenticated()
    {
        if (GetAuthenticatedUserAsync() != null)
            return true;
        return false;
    }
    
    /// <summary>
    /// Does not require Authentication
    /// </summary>
    /// <returns></returns>
    public async Task<NewsResponse> GetNewsListingsAsync()
    {
        return await _client.GetNewsListingsAsync();
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _client.GetUserAsync(_token, userId);
    }

    public async Task<User> GetAuthenticatedUserAsync()
    {
        return await _client.GetAuthenticatedUserAsync(_token);
    }
    
    public async Task<Wiki> GetWikiAsync(string locale, string path)
    {
        return await _client.GetWikiAsync(_token, locale, path);
    }
}