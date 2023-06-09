﻿namespace OsuLauncher.Dialogs;

public partial class SettingsDialog
{
    public SettingsDialog()
    {
        InitializeComponent();
        GeneralLbItem.Selected += (sender, args) => SettingsFrame.Content = new GeneralSettings();
        GameLbItem.Selected += (sender, args) => SettingsFrame.Content = new OsuGameSettings();
    }
}