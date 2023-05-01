using System;
using System.IO;

namespace OsuLauncher.Helpers;

public static class OsuHelper
{
    public static float ReadFloatSetting(string configPath, string key)
    {
        try
        {
            // Open the osu!.cfg file using a StreamReader
            using (StreamReader reader = new StreamReader(configPath))
            {
                // Read the contents of the osu!.cfg file line by line
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    
                    if (line.StartsWith(key))
                    {
                        int index = line.IndexOf('=');
                        if (index >= 0)
                        {
                            string value = line.Substring(index + 1).Trim();

                            float mouseSens;
                            if (float.TryParse(value, out mouseSens))
                            {
                                return mouseSens;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur while reading the osu!.cfg file
            Console.WriteLine("Error reading osu!.cfg file: " + ex.Message);
        }

        // If the music volume key is not found or parsing fails, return a default value of 0
        return 0f;
    }
    
    public static int ReadIntSetting(string configPath, string key)
    {
        try
        {
            // Open the osu!.cfg file using a StreamReader
            using (StreamReader reader = new StreamReader(configPath))
            {
                // Read the contents of the osu!.cfg file line by line
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // Check if the line contains the music volume key
                    if (line.StartsWith(key))
                    {
                        // Extract the value after the ':' sign
                        int index = line.IndexOf('=');
                        if (index >= 0)
                        {
                            string value = line.Substring(index + 1).Trim();

                            // Parse the value as an integer
                            int musicVolume;
                            if (int.TryParse(value, out musicVolume))
                            {
                                // Return the parsed music volume
                                return musicVolume;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur while reading the osu!.cfg file
            Console.WriteLine("Error reading osu!.cfg file: " + ex.Message);
        }

        // If the music volume key is not found or parsing fails, return a default value of 0
        return 0;
    }

    public static void EditIntSetting(string configPath, double musicVolume, string key)
    {
        try
        {
            // Read all lines from the osu!.cfg file
            string[] lines = File.ReadAllLines(configPath);

            // Loop through the lines and update the music volume line
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(key))
                {
                    lines[i] = key + Math.Round(musicVolume);
                    break;
                }
            }

            // Write the updated lines back to the osu!.cfg file
            File.WriteAllLines(configPath, lines);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur while editing the osu!.cfg file
            Console.WriteLine("Error editing osu!.cfg file: " + ex.Message);
        }
    }
    
    public static bool ReadBoolSetting(string configPath, string key)
    {
        try
        {
            // Open the osu!.cfg file using a StreamReader
            using (StreamReader reader = new StreamReader(configPath))
            {
                // Read the contents of the osu!.cfg file line by line
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // Check if the line contains the specified key
                    if (line.StartsWith(key + "'='"))
                    {
                        // Extract the value after the ':' sign
                        int index = line.IndexOf('=');
                        if (index >= 0)
                        {
                            string value = line.Substring(index + 1).Trim();

                            // Parse the value as a boolean
                            if (value == "1")
                            {
                                // If the value is "1", return true
                                return true;
                            }
                            else
                            {
                                // Otherwise, try to parse the value using the bool.TryParse method
                                bool result;
                                if (bool.TryParse(value, out result))
                                {
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur while reading the osu!.cfg file
            Console.WriteLine("Error reading osu!.cfg file: " + ex.Message);
        }

        // If the key is not found or parsing fails, return a default value of false
        return false;
    }
}