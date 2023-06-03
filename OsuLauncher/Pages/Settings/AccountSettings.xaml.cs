namespace OsuLauncher.Pages.Settings;

public partial class AccountSettings
{
    public AccountSettings()
    {
        InitializeComponent();
        InitAuthButton.Click += (sender, args) =>
        {
            try
            {
                Process.Start(new ProcessStartInfo(
                    "https://osu.ppy.sh/oauth/authorize?client_id=21770&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify&state=randomval")
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
                    MessageBox.Show(authCode);
                    listener.Stop();
                    //InitiateTokenSwap(authCode);
                }
            }
            catch (Exception e)
            {
                File.WriteAllText(@"C:\Users\Mayowa\Desktop\errors\log.txt", e.Message + "\n" + e.Source + "\n" + e.StackTrace);
            }
            
        };
    }
    
    private async void InitiateTokenSwap(string code)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");

            var formData = new Dictionary<string, string>
            {
                { "client_id", "21770" },
                { "client_secret", "CDMaHyAmbtex7f5Oxl3iUA7POwPESfCjkoP1fG3D" },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", "http://localhost:7040/callback" }
            };

            request.Content = new FormUrlEncodedContent(formData);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to swap osu! API tokens.");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(json) ?? throw new InvalidOperationException();

            MessageBox.Show(tokenResponse.access_token.ToString());
                
            await File.WriteAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenResponse.access_token.ToString());
            MessageBox.Show("The app will now restart");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}