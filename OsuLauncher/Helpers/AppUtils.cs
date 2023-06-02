namespace OsuLauncher.Helpers;

public static class AppUtils
{
    public static bool CheckForInternetConnection(int timeoutMs = 10000, string url = null)
    {
        try
        {
            url ??= CultureInfo.InstalledUICulture switch
            {
                { Name: var n } when n.StartsWith("fa") => // Iran
                    "http://www.aparat.com",
                { Name: var n } when n.StartsWith("zh") => // China
                    "http://www.baidu.com",
                _ =>
                    "http://www.gstatic.com/generate_204",
            };

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Timeout = timeoutMs;
            using (var response = (HttpWebResponse)request.GetResponse())
                return true;
        }
        catch
        {
            return false;
        }
    }
    
    public static class Config
    {
        private static Configuration config;

        public static void Initialize()
        {
            config = Configuration.LoadFromFile("settings.cfg");
        }
    
        public static void SaveStringItem(string section,string key, string value)
        {
            config[section][key].StringValue = value; 
            config.SaveToFile("settings.cfg");
        }
    
        public static void SaveBool(string section, string key, bool value)
        {
            config[section][key].BoolValue = value; 
            config.SaveToFile("settings.cfg");
        }
    
        public static string GetStringItem(string section, string key)
        {
            config = SharpConfig.Configuration.LoadFromFile("settings.cfg");
            string result = config[section][key].StringValue;
            return result;
        }
    
        public static bool GetBoolItem(string section, string key)
        {
            config = SharpConfig.Configuration.LoadFromFile("settings.cfg");
            bool result = config[section][key].BoolValue;
            return result;
        }
    }

    public static class Dialog
    {
        public static void ShowDialog(Type dialogType)
        {
            Window window = (Window)Activator.CreateInstance(dialogType);
            window.Show();
        }
    }
}