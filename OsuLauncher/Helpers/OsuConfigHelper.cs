namespace OsuLauncher.Helpers;

public class OsuConfigHelper
{
    private string _osuConfigPath = string.Empty;

    public OsuConfigHelper(string osuConfigPath)
    {
        _osuConfigPath = osuConfigPath;
    }

    public DirectoryInfo[] GetSkins()
    {
        //var skinDirectory = new DirectoryInfo(Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"), "Skins"));
        var skinDirectory = new DirectoryInfo(Path.Combine(LauncherSettings.Default.GameDirectory, "skins"));
        return skinDirectory.GetDirectories();
    }
    
    public int ReadInt(string key, int defaultValue = 0)
    {
        string value = ReadValue(key);
        int result;
        if (!int.TryParse(value, out result))
        {
            result = defaultValue;
        }
        return result;
    }
    
    public float ReadFloat(string key, float defaultValue = 0f)
    {
        string value = ReadValue(key);
        float result;
        if (!float.TryParse(value, out result))
        {
            result = defaultValue;
        }
        return result;
    }
    
    public double ReadDouble(string key, float defaultValue = 0f)
    {
        string value = ReadValue(key);
        double result;
        if (!double.TryParse(value, out result))
        {
            result = defaultValue;
        }
        return result;
    }

    public string ReadString(string key, string defaultValue = "")
    {
        string value = ReadValue(key);
        if (value == null || value.Length == 0)
        {
            value = defaultValue;
        }
        return value;
    }

    public bool ReadBool(string key, bool defaultValue = false)
    {
        string value = ReadValue(key);
        bool result;
        if (!bool.TryParse(value, out result))
        {
            result = defaultValue;
        }
        return result;
    }

    public void EditValue(string key, string value)
    {
        // Read all lines of the USER.cfg file
        string[] lines = File.ReadAllLines(_osuConfigPath);

        // Find the line with the key and update its value
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(key))
            {
                lines[i] = $"{key} = {value}";
                break;
            }
        }

        // Write the updated lines back to the USER.cfg file
        File.WriteAllLines(_osuConfigPath, lines);
    }

    private string ReadValue(string key)
    {
        string result = null;
        using (StreamReader reader = new StreamReader(_osuConfigPath))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.StartsWith(key))
                {
                    result = line.Substring(line.IndexOf('=') + 1).Trim();
                    break;
                }
            }
        }
        return result;
    }
}