using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using HandyControl.Tools;
using OsuLauncher.Dialogs;
using OsuLauncher.Helpers;
using ConfigHelper = OsuLauncher.Helpers.ConfigHelper;
using MessageBox = System.Windows.MessageBox;

namespace OsuLauncher.Pages;

public partial class SettingsPage : Page
{
    private string osuCfg;
    public SettingsPage()
    {
        InitializeComponent();
    }
}