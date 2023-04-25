using Newtonsoft.Json;
using OsuAPI.Objects;
using RestSharp;

namespace OsuAPI;

public class OsuClient
{
    private RestClient _restClient;
    private string _token;
    
    public OsuClient(string token)
    {
        _token = token;
        var options = new RestClientOptions("https://osu.ppy.sh")
        {
            MaxTimeout = -1,
        };
        _restClient = new RestClient(options);
    }

    public bool IsAuthenticated()
    {
        if (GetAuthenticatedUser() != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public User GetAuthenticatedUser()
    {
        var request = new RestRequest($"/api/v2/me/osu");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {_token}");
        
        RestResponse response = _restClient.Execute(request);
        var authenticatedUserResponse = JsonConvert.DeserializeObject<User>(response.Content ?? throw new InvalidOperationException());
        return authenticatedUserResponse;
    }
    
    public User GetUser(int userID)
    {
        var request = new RestRequest($"/api/v2/users/{userID}/osu?key=maxime");
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {_token}");

        RestResponse response = _restClient.Execute(request);
        var userResponse = JsonConvert.DeserializeObject<User>(response.Content ?? throw new InvalidOperationException());
        return userResponse;
    }
}