using System.IO;
using System.Linq;

namespace OsuLauncher.Helpers;

public class OsuConfigHelper
{
    private string filePath;

    public OsuConfigHelper(string filePath)
    {
        this.filePath = filePath;
    }

    public int GetIntValue(string key)
    {
        string value = GetValue(key);
        if (int.TryParse(value, out int intValue))
        {
            return intValue;
        }
        else
        {
            throw new InvalidDataException($"Failed to parse int value for key '{key}'");
        }
    }

    public float GetFloatValue(string key)
    {
        string value = GetValue(key);
        if (float.TryParse(value, out float floatValue))
        {
            return floatValue;
        }
        else
        {
            throw new InvalidDataException($"Failed to parse float value for key '{key}'");
        }
    }

    public bool GetBooleanValue(string key)
    {
        string value = GetValue(key);
        if (value == "1" || value == "true")
        {
            return true;
        }
        else if (value == "0" || value == "false")
        {
            return false;
        }
        else
        {
            throw new InvalidDataException($"Failed to parse boolean value for key '{key}'");
        }
    }

    public string GetStringValue(string key)
    {
        return GetValue(key);
    }

    public void SetIntValue(string key, int value)
    {
        SetValue(key, value.ToString());
    }

    public void SetFloatValue(string key, float value)
    {
        SetValue(key, value.ToString());
    }

    public void SetBooleanValue(string key, bool value)
    {
        SetValue(key, value ? "1" : "0");
    }

    public void SetStringValue(string key, string value)
    {
        SetValue(key, value);
    }

    private string GetValue(string key)
    {
        string[] lines = File.ReadAllLines(filePath);
        string line = lines.FirstOrDefault(l => l.StartsWith($"{key} = "));
        if (line != null)
        {
            return line.Substring(key.Length + 1).Trim();
        }
        else
        {
            throw new InvalidDataException($"Key '{key}' not found in osu!.User.cfg file");
        }
    }

    private void SetValue(string key, string value)
    {
        string[] lines = File.ReadAllLines(filePath);
        int index = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith($"{key} = "))
            {
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            lines[index] = $"{key} = {value}";
        }
        else
        {
            lines = lines.Append($"{key} = {value}").ToArray();
        }
        File.WriteAllLines(filePath, lines);
    }
}