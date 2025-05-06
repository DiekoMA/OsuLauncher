using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using API.Objects;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API;

public class OsuClient
{
    private bool tokenValid;
    private string _token;
    public bool IsAuthenticated { get; private set; }
    private HttpClient _client;
    private readonly string baseUrl = "https://osu.ppy.sh/api";
    private JsonSerializerOptions serializerOptions;

    /// <summary>
    /// Instantiate without a token to use Non OAUTH Methods.
    /// </summary>
    /// <param name="token"></param>
    public OsuClient()
    {
        _client = new HttpClient();
        serializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<bool> TryAuthenticateAsync(string token)
    {
        _token = token;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            _ = await GetAuthenticatedUserAsync();
            IsAuthenticated = true;
        }
        catch (Exception e)
        {
            _client.DefaultRequestHeaders.Authorization = null;
            IsAuthenticated = false;
        }
        return IsAuthenticated;
    }

    /// <summary>
    /// Does not require Authentication
    /// </summary>
    /// <returns></returns>
    public async Task<ObservableCollection<NewsPost>?> GetNewsListingsAsync()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{baseUrl}/v2/news"),
            Headers =
            {
                { "Accept", "application/json" }
            },
        };

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var newsListings = JsonSerializer.Deserialize<NewsResponse>(response.Content.ReadAsStringAsync().Result ?? throw new InvalidOperationException(), new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
        return newsListings.Data;
    }

    /// <summary>
    /// Authentication is required.
    /// </summary>
    /// <returns></returns>
    public async Task<User?> GetAuthenticatedUserAsync()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{baseUrl}/v2/me/osu"),
            Headers =
            {
                { "Accept", "application/json" },
            },
        };

        using var response = await _client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            // Handle the case where the request is not successful
            return null; // Or throw a custom exception
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseContent))
        {
            // Handle the case where response content is empty
            return null; // Or throw a custom exception
        }

        // Deserialize the response content to User object
        try
        {
            var authedUserResponse = JsonSerializer.Deserialize<User>(responseContent, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
            });
            return authedUserResponse;
        }
        catch (JsonException ex)
        {
            // Handle deserialization error
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            return null; // Or throw a custom exception
        }
    }

    /// <summary>
    /// Authentication is required.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<User?> GetUserAsync(int userID)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{baseUrl}/v2/{userID}/osu?key=maxime"),
            Headers =
            {
                { "Accept", "application/json" },
            },
        };

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var userResponse = JsonSerializer.Deserialize<User>(response.Content.ReadAsStringAsync().Result ?? throw new InvalidOperationException(), new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        });
        return userResponse;
    }
}