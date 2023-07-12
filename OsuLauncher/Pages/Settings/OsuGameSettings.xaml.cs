using System.Windows.Forms;

namespace OsuLauncher.Pages.Settings;

public partial class OsuGameSettings
{
    private OsuConfigHelper _configHelper;
    public OsuGameSettings()
    {
        InitializeComponent();
        var osuCfg = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"),
                $"osu!.{Environment.UserName}.cfg");
            _configHelper = new OsuConfigHelper(osuCfg);
            if (ConfigHelper.GetStringItem("preferences", "gamedir") != string.Empty)
            {
                try
                {
                    MasterVolumeSlider.ValueChanged += MasterVolumeSliderOnValueChanged;
                    EffectsVolumeSlider.ValueChanged += EffectsVolumeSliderOnValueChanged;
                    MusicVolumeSlider.ValueChanged += MusicVolumeSliderOnValueChanged;
                    foreach (var skin in _configHelper.GetSkins())
                    {
                        SkinCB.Items.Remove(" ");
                        SkinCB.Items.Add(skin.Name);
                    }
                    DisableMouseBtnCB.Checked += (sender, args) => _configHelper.EditValue("MouseDisableButtons", "1");
                    DisableMouseBtnCB.Unchecked += (sender, args) => _configHelper.EditValue("MouseDisableButtons", "0");
                    DisableMouseWheelCB.Checked += (sender, args) => _configHelper.EditValue("MouseDisableWheel", "1");
                    DisableMouseWheelCB.Unchecked += (sender, args) => _configHelper.EditValue("MouseDisableWheel", "0");
                    DiscordRpcCB.Checked += (sender, args) => _configHelper.EditValue("DiscordRichPresence", "1");
                    DiscordRpcCB.Unchecked += (sender, args) => _configHelper.EditValue("DiscordRichPresence", "0");
                    ShowFPSCB.Checked += (sender, args) => _configHelper.EditValue("FpsCounter", "1");
                    ShowFPSCB.Unchecked += (o, eventArgs) => _configHelper.EditValue("FpsCounter", "0");
                    StartFullscreenCB.Checked += (sender, args) => _configHelper.EditValue("Fullscreen", "1");
                    StartFullscreenCB.Unchecked += (sender, args) => _configHelper.EditValue("Fullscreen", "0");
                    SkinCB.SelectionChanged += SkinCBOnSelectionChanged;
                    LeftClickKeybindButton.Click += LeftClickKeybindButtonOnClick;
                    RightClickKeybindButton.Click += RightClickKeybindButtonOnClick;
                    SmokeKeybindButton.Click += SmokeKeybindButtonOnClick;
                    MouseSpeedBox.ValueChanged += (sender, args) =>
                        _configHelper.EditValue("MouseSpeed", args.Info.ToString());
                    CurrentSkinText.Text = _configHelper.ReadString("Skin");
                    MasterVolumeSlider.Value = _configHelper.ReadInt("VolumeUniversal");
                    EffectsVolumeSlider.Value = _configHelper.ReadInt("VolumeEffect");
                    MusicVolumeSlider.Value = _configHelper.ReadInt("VolumeMusic");
                    MouseSpeedBox.Value = _configHelper.ReadDouble("MouseSpeed");
                    LeftClickKeybindButton.Content = _configHelper.ReadString("keyOsuLeft");
                    RightClickKeybindButton.Content = _configHelper.ReadString("keyOsuRight");
                    SmokeKeybindButton.Content = _configHelper.ReadString("keyOsuSmoke");
                    if (_configHelper.ReadInt("MouseDisableButtons") != 0)
                        DisableMouseWheelCB.IsChecked = true;
                    if (_configHelper.ReadInt("MouseDisableWheel") != 0)
                        DisableMouseBtnCB.IsChecked = true;
                    if (_configHelper.ReadInt("DiscordRichPresence") != 0)
                        DiscordRpcCB.IsChecked = true;
                    if (_configHelper.ReadInt("FpsCounter") != 0)
                        ShowFPSCB.IsChecked = true;
                    if (_configHelper.ReadInt("Fullscreen") != 0)
                        StartFullscreenCB.IsChecked = true;
                }
                catch (DirectoryNotFoundException)
                {
                    Growl.Error("Invalid osu! path please check it.");
                }
            }
            else
            {
                Growl.Error("Please remember to set your game path.");
            }
    }

    private void SkinCBOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _configHelper.EditValue("Skin", (string)SkinCB.SelectedItem);
    }

    private void SmokeKeybindButtonOnClick(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyOsuLeft", keySelectDialog.newKey.ToString());
    }

    private void RightClickKeybindButtonOnClick(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        RightClickKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyOsuLeft", keySelectDialog.newKey.ToString());
    }

    private void LeftClickKeybindButtonOnClick(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        LeftClickKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyOsuLeft", keySelectDialog.newKey.ToString());
    }

    private void MusicVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _configHelper.EditValue("VolumeMusic", Convert.ToInt64(e.NewValue).ToString());
    }

    private void EffectsVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _configHelper.EditValue("VolumeEffect", Convert.ToInt64(e.NewValue).ToString());
    }

    private void MasterVolumeSliderOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        _configHelper.EditValue("VolumeUniversal", Convert.ToInt64(e.NewValue).ToString());
    }
}