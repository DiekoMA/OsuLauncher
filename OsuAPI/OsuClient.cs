using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OsuAPI.Objects;
using Refit;
using RestSharp;

namespace OsuAPI;

public class OsuClient
{
    private string clientID;
    private string clientSecret;
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
    
    /// <summary>
    /// Instantiate without ClientConfigurationOptions to use Non OAUTH Methods.
    /// </summary>
    /// <param name="options"></param>
    public OsuClient(ClientConfigurationOptions options = null)
    {
        //_token = token;
        clientID = options.ClientID;
        clientSecret = options.ClientSecret;
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

    /// <summary>
    /// Authentication is required.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<User> GetUserAsync(int userId)
    {
        return await _client.GetUserAsync(_token, userId);
    }

    /// <summary>
    /// Authentication is required.
    /// </summary>
    /// <returns></returns>
    public async Task<User> GetAuthenticatedUserAsync()
    {
        return await _client.GetAuthenticatedUserAsync(_token);
    }
    
    /// <summary>
    /// Authentication is required.
    /// </summary>
    /// <param name="locale">The language code</param>
    /// <param name="path">The wiki path</param>
    /// <returns></returns>
    public async Task<Wiki> GetWikiAsync(string locale, string path)
    {
        return await _client.GetWikiAsync(_token, locale, path);
        
    }
    
}