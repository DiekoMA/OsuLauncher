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
        osuCfg = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"),
            $"osu!.{Environment.UserName}.cfg");
        InitializeComponent();
        GameDirectoryBox.Text = ConfigHelper.GetStringItem("preferences", "gamedir");
        SongsDirectoryBox.Text = ConfigHelper.GetStringItem("preferences", "songsdir");
        MasterVolumeSlider.ValueChanged += MasterVolumeSliderOnValueChanged;
        EffectsVolumeSlider.ValueChanged += EffectsVolumeSliderOnValueChanged;
        MusicVolumeSlider.ValueChanged += MusicVolumeSliderOnValueChanged;
        InitAuthButton.Click += InitAuthButtonOnClick;
        MasterVolumeSlider.Value = OsuHelper.ReadIntSetting(osuCfg, "VolumeUniversal");
        EffectsVolumeSlider.Value = OsuHelper.ReadIntSetting(osuCfg, "VolumeEffect");
        MusicVolumeSlider.Value = OsuHelper.ReadIntSetting(osuCfg, "VolumeMusic");
        GameDirectoryBox.TextChanged += (sender, args) =>  ConfigHelper.SaveStringItem("preferences", "gamedir",GameDirectoryBox.Text); 
        SongsDirectoryBox.TextChanged += (sender, args) => ConfigHelper.SaveStringItem("preferences", "songsdir",SongsDirectoryBox.Text);
        UpdateCB.Checked += (sender, args) => ConfigHelper.SaveBool("preferences", "checkforupdates", true);
    }

    private void MusicVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        OsuHelper.EditIntSetting(osuCfg, Convert.ToDouble(e.NewValue), "VolumeMusic = ");
    }

    private void EffectsVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        OsuHelper.EditIntSetting(osuCfg, Convert.ToDouble(e.NewValue), "VolumeEffect = ");
    }

    private void MasterVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        OsuHelper.EditIntSetting(osuCfg, Convert.ToDouble(e.NewValue), "VolumeUniversal = ");
    }

    private async void TestButtonOnClick(object sender, RoutedEventArgs e)
    {
        
    }

    private void InitAuthButtonOnClick(object sender, RoutedEventArgs e)
    {
        DialogHelper.ShowDialog(typeof(AuthDialog));
    }

    private void StartAuth()
    {
        
    }
}