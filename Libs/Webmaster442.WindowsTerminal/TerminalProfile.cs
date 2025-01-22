using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;

/// <summary>
/// A profile is a set of settings that can be applied to a terminal window.
/// </summary>
public record class TerminalProfile
{
    /// <summary>
    /// This is the name of the profile that will be displayed in the dropdown menu.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// This is the executable used in the profile.
    /// </summary>
    [JsonPropertyName("commandline")]
    public required string CommandLine { get; init; }

    /// <summary>
    /// This is the directory the shell starts in when it is loaded.
    /// </summary>
    [JsonPropertyName("startingDirectory")]
    public string StartingDirectory { get; init; }

    /// <summary>
    /// This sets the icon that displays within the tab, dropdown menu, jumplist, and tab switcher.
    /// </summary>
    [JsonPropertyName("icon")]
    public string Icon { get; init; }

    /// <summary>
    /// If set, this will replace the name as the title to pass to the shell on startup. 
    /// Some shells (like bash) may choose to ignore this initial value, while others (Command Prompt, PowerShell) may 
    /// use this value over the lifetime of the application.
    /// </summary>
    [JsonPropertyName("tabTitle")]
    public string TabTitle { get; init; }

    /// <summary>
    /// If set, this profile will automatically open up in an "elevated" window (running as Administrator) by default.
    /// </summary>
    [JsonPropertyName("elevate")]
    public bool Elevate { get; init; }

    /// <summary>
    /// If hidden is set to true, the profile will not appear in the list of profiles. 
    /// This can be used to hide default profiles and dynamically generated profiles
    /// </summary>
    [JsonPropertyName("hidden")]
    public bool Hidden { get; init; }

    /// <summary>
    /// This is the name of the color scheme used in the profile. Color schemes are defined in the schemes object.
    /// </summary>
    [JsonPropertyName("colorScheme")]
    public string ColorScheme { get; init; }

    /// <summary>
    /// When this is set to true, the window will have an acrylic background. When it's set to false, the window will have a plain, untextured background. 
    /// </summary>
    [JsonPropertyName("useAcrylic")]
    public bool UseAcrylic { get; init; }

    /// <summary>
    /// This sets the file location of the image to draw over the window background. The background image can be a .jpg, .png, or .gif file. "desktopWallpaper" will set the background image to the desktop's wallpaper.
    /// </summary>
    [JsonPropertyName("backgroundImage")]
    public string BackgroundImage { get; init; }

    /// <summary>
    /// This sets the transparency of the background image.
    /// </summary>
    [JsonPropertyName("backgroundImageOpacity")]
    public double BackgroundImageOpacity
    {
        get => field;
        init => field = value.Restrict(0, 1.0);
    }

    /// <summary>
    /// This sets how the background image aligns to the boundaries of the window.
    /// </summary>
    [JsonPropertyName("backgroundImageAlignment")]
    public TerminalBackgroundImageAlignment BackgroundImageAlignment { get; init; }

    /// <summary>
    /// This sets how the background image is resized to fill the window.
    /// </summary>
    [JsonPropertyName("backgroundImageStretchMode")]
    public TerminalBackgroundImageStretchMode BackgroundImageStretchMode { get; init; }

    /// <summary>
    /// This sets the transparency of the window for the profile.
    /// This accepts an integer value from 0-100, representing a "percent opaque".
    /// 100 is "fully opaque", 50 is semi-transparent, and 0 is fully transparent.
    /// </summary>
    [JsonPropertyName("opacity")]
    public int Opacity 
    {
        get => field;
        init => field = value.Restrict(0, 100);
    }

    /// <summary>
    /// Creates a new instance of the TerminalProfile class.
    /// </summary>
    public TerminalProfile()
    {
        StartingDirectory = string.Empty;
        Icon = string.Empty;
        TabTitle = string.Empty;
        BackgroundImage = string.Empty;
        ColorScheme = string.Empty;
        BackgroundImageStretchMode = TerminalBackgroundImageStretchMode.UniformToFill;
        BackgroundImageAlignment = TerminalBackgroundImageAlignment.Center;
        BackgroundImageOpacity = 1.0;
        Opacity = 100;
    }
}
