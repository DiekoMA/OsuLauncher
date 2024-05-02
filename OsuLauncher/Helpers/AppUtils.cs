namespace OsuLauncher.Helpers;

public static class AppUtils
{
    private static DiscordRpcClient rpcClient;
    public static class Config
    {
        private static Configuration config;

        public static void Initialize()
        {
            config = Configuration.LoadFromFile("settings.cfg");
        }

        public static void LoadEnv(string filePath)
        {
            if (File.Exists(filePath))
                return;

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

        public static void SaveStringItem(string section, string key, string value)
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

    public static class RPC
    {
        public static async Task Start(string state)
        {
            rpcClient = new DiscordRpcClient(LauncherSettings.Default.RichPresenceClientId);
            rpcClient.Initialize();
            rpcClient.SetPresence(new RichPresence()
            {
                Details = "",
                State = state,
            });
        }

        public static async Task Clear()
        {
            if (rpcClient.IsInitialized)
                rpcClient.ClearPresence();
        }

        public static async Task Stop()
        {
            /*if (rpcClient.IsInitialized && !rpcClient.IsDisposed)
                rpcClient.Dispose();*/
        }
    }
}