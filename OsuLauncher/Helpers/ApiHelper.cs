namespace OsuLauncher.Helpers;

public sealed class ApiHelper
{
    private static OsuClient _client;
    private static string tokenLocation = string.Empty;
    private static readonly Lazy<ApiHelper> _instance = new Lazy<ApiHelper>(() => new ApiHelper());

    public static ApiHelper Instance => _instance.Value;

    private ApiHelper()
    {
        tokenLocation = Path.Combine(Directory.GetCurrentDirectory(), "token.secret");
        _client = new OsuClient();
    }

    public async Task<OsuClient> RetrieveClient()
    {
        await _client.TryAuthenticateAsync(File.ReadAllText(tokenLocation));
        return _client;
    }
}
    
    