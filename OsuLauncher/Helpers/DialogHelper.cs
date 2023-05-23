namespace OsuLauncher.Helpers;

public class DialogHelper
{
    public static void ShowDialog(Type dialogType)
    {
        HandyControl.Controls.Window window = (HandyControl.Controls.Window)Activator.CreateInstance(dialogType);
        window.Show();
    }
}