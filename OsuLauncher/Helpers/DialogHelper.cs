using System;
using System.Windows;

namespace OsuLauncher.Helpers;

public class DialogHelper
{
    public static void ShowDialog(Type dialogType)
    {
        Window window = (Window)Activator.CreateInstance(dialogType);
        window.Show();
    }
}