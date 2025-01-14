namespace OsuLauncher.Helpers;

public static class AppUtils
{
    private static DiscordRpcClient rpcClient;
    public static class AppConfig
    {
        private static Config config;

        static AppConfig()
        {
            config = Config.Build();
        }

        public static void SetString(string section, string key, string value) => config.SetString(section, key, value); 

        public static void SetBoolean(string section, string key, bool value) => config.SetBoolean(section, key, value);
        

        public static string GetString(string section, string key)
        {
            return config.GetString(section, key)!;
        }

        public static bool GetBoolean(string section, string key)
        {
            return config.GetBoolean(section, key) ?? false;
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

    /*public static class RPC
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
                rpcClient.Dispose();#1#
        }
    }*/
}