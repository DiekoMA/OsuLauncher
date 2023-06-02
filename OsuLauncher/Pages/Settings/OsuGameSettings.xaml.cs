namespace OsuLauncher.Pages.Settings;

public partial class OsuGameSettings
{
    private OsuConfigHelper configHelper;
    public OsuGameSettings()
    {
        InitializeComponent();
        var osuCfg = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"),
                $"osu!.{Environment.UserName}.cfg");
            configHelper = new OsuConfigHelper(osuCfg);
            if (ConfigHelper.GetStringItem("preferences", "gamedir") != string.Empty)
            {
                MasterVolumeSlider.ValueChanged += MasterVolumeSliderOnValueChanged;
                EffectsVolumeSlider.ValueChanged += EffectsVolumeSliderOnValueChanged;
                MusicVolumeSlider.ValueChanged += MusicVolumeSliderOnValueChanged;
                DisableMouseBtnCB.Checked += (sender, args) => configHelper.EditValue("MouseDisableButtons", "1");
                DisableMouseBtnCB.Unchecked += (sender, args) => configHelper.EditValue("MouseDisableButtons", "0");
                DisableMouseWheelCB.Checked += (sender, args) => configHelper.EditValue("MouseDisableWheel", "1");
                DisableMouseWheelCB.Unchecked += (sender, args) => configHelper.EditValue("MouseDisableWheel", "0");
                DiscordRpcCB.Checked += (sender, args) => configHelper.EditValue("DiscordRichPresence", "1");
                DiscordRpcCB.Unchecked += (sender, args) => configHelper.EditValue("DiscordRichPresence", "0");
                ShowFPSCB.Checked += (sender, args) => configHelper.EditValue("FpsCounter", "1");
                ShowFPSCB.Unchecked += (o, eventArgs) => configHelper.EditValue("FpsCounter", "0");
                LeftClickKeybindButton.Click += LeftClickKeybindButtonOnClick;
                RightClickKeybindButton.Click += RightClickKeybindButtonOnClick;
                SmokeKeybindButton.Click += SmokeKeybindButtonOnClick;
                CurrentSkinText.Text = configHelper.ReadString("Skin");
                MasterVolumeSlider.Value = configHelper.ReadInt("VolumeUniversal");
                EffectsVolumeSlider.Value = configHelper.ReadInt("VolumeEffect");
                MusicVolumeSlider.Value = configHelper.ReadInt("VolumeMusic");
                //MouseSensitivitySlider.Value = configHelper.ReadDouble("MouseSpeed");
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
            else
            {
                Growl.Error("Please remember to set your game path");
            }
    }
    private void SmokeKeybindButtonOnClick(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        configHelper.EditValue("keyOsuLeft", keySelectDialog.newKey.ToString());
    }

    private void RightClickKeybindButtonOnClick(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        RightClickKeybindButton.Content = keySelectDialog.newKey;
        configHelper.EditValue("keyOsuLeft", keySelectDialog.newKey.ToString());
    }

    private void LeftClickKeybindButtonOnClick(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        LeftClickKeybindButton.Content = keySelectDialog.newKey;
        configHelper.EditValue("keyOsuLeft", keySelectDialog.newKey.ToString());
    }

    private void MusicVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        configHelper.EditValue("VolumeMusic", Convert.ToInt64(e.NewValue).ToString());
    }

    private void EffectsVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        configHelper.EditValue("VolumeEffect", Convert.ToInt64(e.NewValue).ToString());
    }

    private void MasterVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        configHelper.EditValue("VolumeUniversal", Convert.ToInt64(e.NewValue).ToString());
    }
}