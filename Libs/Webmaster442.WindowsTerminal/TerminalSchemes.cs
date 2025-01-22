namespace Webmaster442.WindowsTerminal;

/// <summary>
/// Built in color schemes for Windows Terminal
/// </summary>
public static class TerminalSchemes
{
    /// <summary>
    /// Campbell scheme
    /// </summary>
    public const string Campbell = "Campbell";
    /// <summary>
    /// Campbell Powershell scheme
    /// </summary>
    public const string CampbellPowershell = "Campbell Powershell";
    /// <summary>
    /// One Half Dark scheme
    /// </summary>
    public const string OneHalfDark = "One Half Dark";
    /// <summary>
    /// One Half Light scheme
    /// </summary>
    public const string OneHalfLight = "One Half Light";
    /// <summary>
    /// Vintage scheme
    /// </summary>
    public const string Vintage = "Vintage";
    /// <summary>
    /// Tango Dark scheme
    /// </summary>
    public const string TangoDark = "Tango Dark";
    /// <summary>
    /// Tango Light scheme
    /// </summary>
    public const string TangoLight = "Tango Light";

    /// <summary>
    /// https://windowsterminalthemes.dev/?theme=purplepeter
    /// </summary>
    public static readonly TerminalScheme PurplepeterShecme = new()
    {
        Name = "purplepeter",
        Black = "#961947",
        Red = "#ff796d",
        Green = "#99b481",
        Yellow = "#efdfac",
        Blue = "#66d9ef",
        Purple = "#e78fcd",
        Cyan = "#ba8cff",
        White = "#ffba81",
        BrightBlack = "#d62365",
        BrightRed = "#f99f92",
        BrightGreen = "#b4be8f",
        BrightYellow = "#f2e9bf",
        BrightBlue = "#79daed",
        BrightPurple = "#ba91d4",
        BrightCyan = "#a0a0d6",
        BrightWhite = "#b9aed3",
        Background = "#2a1a4a",
        Foreground = "#ece7fa",
        CursorColor = "#c7c7c7",
        SelectionBackground = "#8689c2",
    };

    /// <summary>
    /// https://windowsterminalthemes.dev/?theme=Github
    /// </summary>
    public static readonly TerminalScheme GithubShecme = new()
    {
        Name = "Github",
        Black = "#3e3e3e",
        Red = "#970b16",
        Green = "#07962a",
        Yellow = "#f8eec7",
        Blue = "#003e8a",
        Purple = "#e94691",
        Cyan = "#89d1ec",
        White = "#ffffff",
        BrightBlack = "#666666",
        BrightRed = "#de0000",
        BrightGreen = "#87d5a2",
        BrightYellow = "#f1d007",
        BrightBlue = "#2e6cba",
        BrightPurple = "#ffa29f",
        BrightCyan = "#1cfafe",
        BrightWhite = "#ffffff",
        Background = "#f4f4f4",
        Foreground = "#3e3e3e",
        SelectionBackground = "#a9c1e2",
        CursorColor = "#3f3f3f"
    };
}