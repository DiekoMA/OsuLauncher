using API.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace API;

public class OsuClient
{
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
    /// Instantiate without a token to use Non OAUTH Methods.
    /// </summary>
    /// <param name="options"></param>
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