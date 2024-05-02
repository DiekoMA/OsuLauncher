using Microsoft.Extensions.DependencyInjection;
using OsuLauncher.Services;
using OsuLauncher.ViewModels;

namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainWindowViewModel>()
            });
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<BeatmapViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));
            
            _serviceProvider = services.BuildServiceProvider();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
            /*AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger();
            Log.Information("Application started succesfully");
            switch (AppSettings.Default.ThemeBase)
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
            }*/
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(e.ExceptionObject.ToString());
        }
    }
}