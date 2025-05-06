namespace OsuLauncher.Models;

public record OsuSettingsPreset
{
    [Category("Naming")]
    public string Title { get; set; } = "New Preset";
    
    
    [Category("Volume")]
    public int MusicVolume { get; set; }
    public int MasterVolume { get; set; }
    public int EffectsVolume { get; set; }
    
    public Slider test { get; set; }
    
    [Category("Keybinds")]
    public Key KeyOsuLeft { get; set; }
    
    //TODO: Add Keybindings 
}