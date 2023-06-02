namespace OsuLauncher.Pages.Settings;

public partial class AccountSettings : Page
{
    public AccountSettings()
    {
        InitializeComponent();
        InitAuthButton.Click += (sender, args) =>
        {
            Process.Start(new ProcessStartInfo("https://osu.ppy.sh/oauth/authorize?client_id=21770&redirect_uri=http%3A%2F%2Flocalhost%3A7040%2Fcallback&response_type=code&scope=public+identify&state=randomval%20http://localhost:7040/callback")
            {
                UseShellExecute = true
            });
            
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:7040/callback/");
            listener.Start();
            Console.WriteLine("Listening for requests on http://localhost:8080/");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                string responseText = "Authentication complete";
                byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = responseBytes.Length;
                context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                context.Response.Close();
                var code = context.Request.Url.AbsoluteUri;
                var tokenRegex = new Regex("(?<=code=)(.*)(?=&state)");
                if (!tokenRegex.IsMatch(code))
                    return;
                var authCode = tokenRegex.Match(code).Value;
                Process.Start(new ProcessStartInfo($"OsuLauncher://{authCode}")
                {
                    UseShellExecute = true
                });
            }
        }; //DialogHelper.ShowDialog(typeof(AuthDialog));
    }
}