namespace OsuLauncher.Pages.Settings;

public partial class GeneralSettings : Page
{
    public GeneralSettings()
    {
        InitializeComponent();
        GameDirectoryBox.Text = AppUtils.Config.GetStringItem("preferences", "gamedir");
        SongsDirectoryBox.Text = AppUtils.Config.GetStringItem("preferences", "songsdir");
        GameDirectoryBox.TextChanged += (sender, args) =>  AppUtils.Config.SaveStringItem("preferences", "gamedir",GameDirectoryBox.Text); 
        SongsDirectoryBox.TextChanged += (sender, args) => AppUtils.Config.SaveStringItem("preferences", "songsdir",SongsDirectoryBox.Text);
        UpdateCB.Checked += (sender, args) => AppUtils.Config.SaveBool("preferences", "checkforupdates", true);
        InitAuthButton.Click += (sender, args) =>
        {
            try
            {
                Process.Start(new ProcessStartInfo(
                    $"https://osu.ppy.sh/oauth/authorize?client_id=21770&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify+friends.read&state=randomval")
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
                    { "client_id", "21770" },
                    { "client_secret", "CDMaHyAmbtex7f5Oxl3iUA7POwPESfCjkoP1fG3D" },
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", "http://localhost:7040/callback" }
                })
            };

            using var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to swap osu! API tokens.");
            }
                
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            TokenResponse tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(json) ?? throw new InvalidOperationException();
        
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "refreshtoken.secret"), tokenResponse.RefreshToken);
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "expiration.secret"), tokenResponse.ExpiresIn.ToString());
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenResponse.AccessToken);
            
            // try
            // {
            //     MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //     mainWindow.RetrieveUserInfo();
            // }
            // catch (Exception e)
            // {
            //     MessageBox.Show(e.Message);
            //     File.WriteAllText(@"C:\Users\Mayowa\Desktop\errors\log.txt", e.Message + "\n" + e.Source + "\n" + e.StackTrace);
            // }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}