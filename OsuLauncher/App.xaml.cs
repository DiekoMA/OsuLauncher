using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HandyControl.Controls;
using OsuLauncher.Helpers;
using Serilog;
using MessageBox = System.Windows.MessageBox;

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
            var configFile = Path.Combine(Directory.GetCurrentDirectory(), "settings.cfg");
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
            MessageBox.Show(e.ToString() + e.ExceptionObject.ToString());
        }
    }
}