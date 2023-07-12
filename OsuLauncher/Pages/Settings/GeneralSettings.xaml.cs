namespace OsuLauncher.Pages.Settings;

public delegate void UpdateUIEventHandler();

public partial class GeneralSettings
{
    private IConfigurationRoot config;
    private string clientId;
    private string clientSecret;
    private string redirectUrl;
    
    public event UpdateUIEventHandler UpdateUIEvent;

    public GeneralSettings()
    {
        InitializeComponent();
        var assembly = Assembly.GetExecutingAssembly();
        using (var stream = assembly.GetManifestResourceStream("OsuLauncher.appsettings.json"))
        {
            using (var reader = new StreamReader(stream))
            {
                var config = System.Text.Json.JsonSerializer.Deserialize<SecretsConfiguration>(reader.ReadToEnd());
                clientId = config.ClientId;
                clientSecret = config.ClientSecret;
                redirectUrl = config.RedirectUrl;
            }
        }
        GameDirectoryBox.Text = AppUtils.Config.GetStringItem("preferences", "gamedir");
        SongsDirectoryBox.Text = AppUtils.Config.GetStringItem("preferences", "songsdir");
        GameDirectoryBox.TextChanged += (sender, args) =>  AppUtils.Config.SaveStringItem("preferences", "gamedir",GameDirectoryBox.Text); 
        SongsDirectoryBox.TextChanged += (sender, args) => AppUtils.Config.SaveStringItem("preferences", "songsdir",SongsDirectoryBox.Text);
        ThemeCb.Text = AppUtils.Config.GetStringItem("preferences", "theme_base").ToString();
        ThemeCb.SelectionChanged += (sender, args) =>
        {
            switch (ThemeCb.SelectedIndex)
            {
                case 0:
                    ThemeManager.Current.UsingSystemTheme = true;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    AppUtils.Config.SaveStringItem("preferences", "theme_base", "System");
                    AppUtils.Config.SaveStringItem("preferences", "theme_accent", SystemParameters.WindowGlassBrush.ToString()); 
                    break;
                
                case 1:
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    AppUtils.Config.SaveStringItem("preferences", "theme_base", "Dark"); 
                    AppUtils.Config.SaveStringItem("preferences", "theme_accent", SystemParameters.WindowGlassBrush.ToString()); 
                    break;
                
                case 2:
                    var converter = new BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#2E3440");
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    ThemeManager.Current.AccentColor = brush;
                    AppUtils.Config.SaveStringItem("preferences", "theme_base", "Nord"); 
                    AppUtils.Config.SaveStringItem("preferences", "theme_accent", brush.ToString()); 
                    break;
                
                case 3:
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    AppUtils.Config.SaveStringItem("preferences", "theme_base", "Light"); 
                    AppUtils.Config.SaveStringItem("preferences", "theme_accent", SystemParameters.WindowGlassBrush.ToString()); 
                    break;
            }
        };
        UpdateCB.Checked += (sender, args) => AppUtils.Config.SaveBool("preferences", "checkforupdates", true);
        InitAuthButton.Click += (sender, args) =>
        {
            try
            {
                Process.Start(new ProcessStartInfo(
                    $"https://osu.ppy.sh/oauth/authorize?client_id={clientId}&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify+friends.read&state=randomval")
                {
                    UseShellExecute = true
                });
                
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:7040/callback/");
                listener.Start();
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    string responseText = "Authentication complete";
                    byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseText);
                    context.Response.ContentLength64 = responseBytes.Length;
                    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                    context.Response.Close();
                    var code = context.Request.Url?.AbsoluteUri;
                    var tokenRegex = new Regex("(?<=code=)(.*)(?=&state)");
                    if (!tokenRegex.IsMatch(code))
                        return;
                    var authCode = tokenRegex.Match(code).Value;
                    InitiateTokenSwap(authCode);
                    listener.Stop();
                    UpdateUIEvent.Invoke();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            
        };
    }

    private void DialogOptCB_OnChecked(object sender, RoutedEventArgs e)
    {
        AppUtils.Config.SaveBool("User_Preference", "beatmapmirroroptin", true);
    }

    private void DialogOptCB_OnUnchecked(object sender, RoutedEventArgs e)
    {
        AppUtils.Config.SaveBool("User_Preference", "beatmapmirroroptin", false);
    }
    
    private async void InitiateTokenSwap(string code)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://osu.ppy.sh/oauth/token"),
                Headers =
                {
                    { "Accept", "application/json" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", redirectUrl }
                })
            };

            using var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to swap osu! API tokens.");
            }
                
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            
            TokenResponse tokenResponse = System.Text.Json.JsonSerializer.Deserialize<TokenResponse>(json, new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            }) ?? throw new InvalidOperationException();
        
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "refreshtoken.secret"), tokenResponse.RefreshToken);
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenResponse.AccessToken);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
