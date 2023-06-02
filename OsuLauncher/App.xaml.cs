namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                Application.Current.Shutdown();
                var code = e.Args[0];
                // var tokenRegex = new Regex("(?<=code=)(.*)(?=&state)");
                // if (!tokenRegex.IsMatch(code))
                //     return;
                // var authCode = tokenRegex.Match(code).Value;
                InitiateTokenSwap(code);

            }
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            var log = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
            var configFile = Path.Combine(Directory.GetCurrentDirectory(), "settings.cfg");
            var defaultOsuDir =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "osu!");
            if (Directory.Exists(defaultOsuDir))
            {
                AppUtils.Config.SaveStringItem("preferences", "gamedir",defaultOsuDir); 
            }
            if (!File.Exists(configFile))
            {
                try
                {
                    File.Create(configFile);
                }
                catch (Exception exception)
                {
                    Growl.Error(exception.Message + "Has been logged");
                    Log.Error(exception.Message);
                }
            }
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString());
        }
        
        private async void InitiateTokenSwap(string code)
        {
            try
            {
                using (var client = new HttpClient())
                {
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
                    dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                    MessageBox.Show(tokenResponse.access_token.ToString());
                
                    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenResponse.access_token.ToString());
                    MessageBox.Show("The app will now restart");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        
        }
    }
}