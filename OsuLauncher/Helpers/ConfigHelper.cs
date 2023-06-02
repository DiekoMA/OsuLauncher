namespace OsuLauncher.Helpers;

public static class ConfigHelper
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