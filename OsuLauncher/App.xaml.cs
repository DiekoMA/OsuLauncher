using Onova;
using Onova.Services;

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
                AppUtils.Config.SaveStringItem("preferences", "gamedir",defaultOsuDir); 
            }
            if (!File.Exists(configFile))
            {
                try
                {
                    File.Create(configFile);
                }
                catch (Exception exception)
                {
                    Growl.Error(exception.Message + "Has been logged");
                    Log.Error(exception.Message);
                }
            }
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString());
        }
    }
}