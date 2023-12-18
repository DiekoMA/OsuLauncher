namespace OsuLauncher.Pages.Settings;

public partial class OsuGameSettings
{
    private OsuConfigHelper _configHelper;
    public OsuGameSettings()
    {
        InitializeComponent();
        var osuCfg = Path.Combine(LauncherSettings.Default.GameDirectory,
                $"osu!.{Environment.UserName}.cfg");
        _configHelper = new OsuConfigHelper(osuCfg);
        if (LauncherSettings.Default.GameDirectory != string.Empty)
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
                TaikoDrumCentreLeftKeybindButton.Click += TaikoDrumCentreLeftKeybindButton_Click;
                TaikoDrumCentreRightKeybindButton.Click += TaikoDrumCentreRightKeybindButton_Click;
                TaikoDrumRimLeftKeybindButton.Click += TaikoDrumRimLeftKeybindButton_Click;
                TaikoDrumRimRightKeybindButton.Click += TaikoDrumRimRightKeybindButton_Click;
                CatchMoveLeftKeybindButton.Click += CatchMoveLeftKeybindButton_Click;
                CatchMoveRightKeybindButton.Click += CatchMoveRightKeybindButton_Click;
                DashKeybindButton.Click += DashKeybindButton_Click;
                IncreaseSpeedKeybindButton.Click += IncreaseSpeedKeybindButton_Click;
                DecreaseSpeedKeybindButtonKeybindButton.Click += DecreaseSpeedKeybindButtonKeybindButton_Click;
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
                TaikoDrumCentreLeftKeybindButton.Content = _configHelper.ReadString("keyTaikoInnerLeft");
                TaikoDrumCentreRightKeybindButton.Content = _configHelper.ReadString("keyTaikoInnerRight");
                TaikoDrumRimLeftKeybindButton.Content = _configHelper.ReadString("keyTaikoOuterLeft");
                TaikoDrumRimRightKeybindButton.Content = _configHelper.ReadString("keyTaikoOuterRight");
                CatchMoveLeftKeybindButton.Content = _configHelper.ReadString("keyFruitsLeft");
                CatchMoveRightKeybindButton.Content = _configHelper.ReadString("keyFruitsRight");
                DashKeybindButton.Content = _configHelper.ReadString("keyFruitsDash");
                IncreaseSpeedKeybindButton.Content = _configHelper.ReadString("keyIncreaseSpeed");
                DecreaseSpeedKeybindButtonKeybindButton.Content = _configHelper.ReadString("keyDecreaseSpeed");
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

    private void DecreaseSpeedKeybindButtonKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyDecreaseSpeed", keySelectDialog.newKey.ToString());
    }

    private void IncreaseSpeedKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyIncreaseSpeed", keySelectDialog.newKey.ToString());
    }

    private void DashKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyFruitsDash", keySelectDialog.newKey.ToString());
    }

    private void CatchMoveRightKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyFruitsRight", keySelectDialog.newKey.ToString());
    }

    private void CatchMoveLeftKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        SmokeKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyFruitsLeft", keySelectDialog.newKey.ToString());
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

    private void TaikoDrumRimRightKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        TaikoDrumRimRightKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyTaikoOuterRight", keySelectDialog.newKey.ToString());
    }

    private void TaikoDrumRimLeftKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        TaikoDrumRimLeftKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyTaikoOuterLeft", keySelectDialog.newKey.ToString());
    }

    private void TaikoDrumCentreRightKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        TaikoDrumCentreRightKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyTaikoInnerRight", keySelectDialog.newKey.ToString());
    }

    private void TaikoDrumCentreLeftKeybindButton_Click(object sender, RoutedEventArgs e)
    {
        KeySelectDialog keySelectDialog = new KeySelectDialog();
        keySelectDialog.ShowDialog();
        TaikoDrumCentreLeftKeybindButton.Content = keySelectDialog.newKey;
        _configHelper.EditValue("keyTaikoInnerLeft", keySelectDialog.newKey.ToString());
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