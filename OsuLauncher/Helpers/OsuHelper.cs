using System;
using System.IO;

namespace OsuLauncher.Helpers;

public static class OsuHelper
{
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
}