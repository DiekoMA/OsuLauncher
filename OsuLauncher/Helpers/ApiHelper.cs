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
        if (File.Exists(tokenLocation))
        {
            byte[] readEncryptedToken = File.ReadAllBytes("token.secret");
            byte[] decryptedToken = ProtectedData.Unprotect(readEncryptedToken, null, DataProtectionScope.CurrentUser);
            var decryptedAccesstoken = JsonSerializer.Deserialize<TokenResponse>(decryptedToken);

            await _client.TryAuthenticateAsync(decryptedAccesstoken.AccessToken);
            return _client;
        }

        return _client;
    }

    public string GetTokenLocation() => tokenLocation;
}

