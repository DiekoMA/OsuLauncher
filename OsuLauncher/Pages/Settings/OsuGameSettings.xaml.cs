using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using OsuLauncher.Helpers;

namespace OsuLauncher.Pages.Settings;

public partial class OsuGameSettings : Page
{
    private string osuCfg;
    public OsuGameSettings()
    {
        InitializeComponent();
        osuCfg = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"),
            $"osu!.{Environment.UserName}.cfg");
        MasterVolumeSlider.ValueChanged += MasterVolumeSliderOnValueChanged;
        EffectsVolumeSlider.ValueChanged += EffectsVolumeSliderOnValueChanged;
        MusicVolumeSlider.ValueChanged += MusicVolumeSliderOnValueChanged;
        MasterVolumeSlider.Value = OsuHelper.ReadIntSetting(osuCfg, "VolumeUniversal");
        EffectsVolumeSlider.Value = OsuHelper.ReadIntSetting(osuCfg, "VolumeEffect");
        MusicVolumeSlider.Value = OsuHelper.ReadIntSetting(osuCfg, "VolumeMusic");
        MouseSensitivitySlider.Value = OsuHelper.ReadFloatSetting(osuCfg, "MouseSpeed");
        switch (OsuHelper.ReadBoolSetting(osuCfg, "DiscordRichPresence"))
        {
            case true:
                DiscordRpcCB.IsChecked = true;
                break;
            
            case false:
                DiscordRpcCB.IsChecked = false;
                break;
        }
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
}