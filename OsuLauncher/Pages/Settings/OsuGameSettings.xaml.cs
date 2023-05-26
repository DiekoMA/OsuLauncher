namespace OsuLauncher.Pages.Settings;

public partial class OsuGameSettings
{
    public OsuGameSettings()
    {
        InitializeComponent();
        try
        {
            var osuCfg = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"),
                $"osu!.{Environment.UserName}.cfg");
            var configHelper = new OsuConfigHelper(osuCfg);
            MasterVolumeSlider.ValueChanged += MasterVolumeSliderOnValueChanged;
            EffectsVolumeSlider.ValueChanged += EffectsVolumeSliderOnValueChanged;
            MusicVolumeSlider.ValueChanged += MusicVolumeSliderOnValueChanged;
            MasterVolumeSlider.Value = configHelper.ReadInt("VolumeUniversal");
            EffectsVolumeSlider.Value = configHelper.ReadInt("VolumeEffect");
            MusicVolumeSlider.Value = configHelper.ReadInt("VolumeMusic");
            MouseSensitivitySlider.Value = configHelper.ReadDouble("MouseSpeed");
            LeftClickKeybindButton.Content = configHelper.ReadString("keyOsuLeft");
            RightClickKeybindButton.Content = configHelper.ReadString("keyOsuRight");
            SmokeKeybindButton.Content = configHelper.ReadString("keyOsuSmoke");
            switch (configHelper.ReadInt("MouseDisableButtons"))
            {
                case 0:
                    DisableMouseWheelCB.IsChecked = false;
                    break;
            
                case 1:
                    DisableMouseWheelCB.IsChecked = true;
                    break;
            }
            switch (configHelper.ReadInt("MouseDisableWheel"))
            {
                case 0:
                    DisableMouseBtnCB.IsChecked = false;
                    break;
            
                case 1:
                    DisableMouseBtnCB.IsChecked = true;
                    break;
            }
            switch (configHelper.ReadInt("DiscordRichPresence"))
            {
                case 0:
                    DiscordRpcCB.IsChecked = false;
                    break;
            
                case 1:
                    DiscordRpcCB.IsChecked = true;
                    break;
            }
            switch (configHelper.ReadInt("FpsCounter"))
            {
                case 0:
                    ShowFPSCB.IsChecked = false;
                    break;
            
                case 1:
                    ShowFPSCB.IsChecked = true;
                    break;
            }
        }
        catch (Exception e)
        {
            Growl.Error(e.Message);
            File.WriteAllText(@"C:\Users\ihate\Desktop\errors", e.Message);
        }
    }
    
    private void MusicVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        //OsuHelper.EditIntSetting(osuCfg, Convert.ToDouble(e.NewValue), "VolumeMusic = ");
    }

    private void EffectsVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        //OsuHelper.EditIntSetting(osuCfg, Convert.ToDouble(e.NewValue), "VolumeEffect = ");
    }

    private void MasterVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        //OsuHelper.EditIntSetting(osuCfg, Convert.ToDouble(e.NewValue), "VolumeUniversal = ");
    }
}