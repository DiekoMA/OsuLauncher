namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            var log = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
            var configFile = Path.Combine(Directory.GetCurrentDirectory(), "settings.cfg");
            var defaultOsuDir =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "osu!");
            if (Directory.Exists(defaultOsuDir))
            {
                AppUtils.Config.SaveStringItem("preferences", "gamedir", defaultOsuDir);
            }
            if (!File.Exists(configFile))
            {
                try
                {
                    File.Create(configFile);
                    if (Directory.Exists(defaultOsuDir))
                    {
                        AppUtils.Config.SaveStringItem("preferences", "gamedir", defaultOsuDir);
                    }
                }
                catch (Exception exception)
                {
                    Growl.Error(exception.Message + "Has been logged");
                    Log.Error(exception.Message);
                }
            }

            switch (AppUtils.Config.GetStringItem("preferences", "theme_base"))
            {
                case "System":
                    ThemeManager.Current.UsingSystemTheme = true;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    break;

                case "Dark":
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    break;

                case "Nord":
                    var converter = new BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#2E3440");
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
                    ThemeManager.Current.AccentColor = brush;
                    break;

                case "Light":
                    ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
                    ThemeManager.Current.AccentColor = SystemParameters.WindowGlassBrush;
                    break;
            }
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString());
        }
    }
}