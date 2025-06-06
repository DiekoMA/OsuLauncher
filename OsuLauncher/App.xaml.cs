﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using OsuLauncher.Services;
using OsuLauncher.ViewModels;

namespace OsuLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger();
            Log.Information("Application started succesfully");
            /*switch (AppSettings.Default.ThemeBase)
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