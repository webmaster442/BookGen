using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;

/// <summary>
/// Represents a color scheme for Windows Terminal
/// </summary>
public record class TerminalScheme
{
    /// <summary>
    /// The name of the scheme
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Black color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("black")]
    public required string Black { get; init; }

    /// <summary>
    /// Red color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("red")]
    public required string Red { get; init; }

    /// <summary>
    /// Green color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("green")]
    public required string Green { get; init; }

    /// <summary>
    /// Yellow color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("yellow")]
    public required string Yellow { get; init; }

    /// <summary>
    /// Blue color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("blue")]
    public required string Blue { get; init; }

    /// <summary>
    /// Purple color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("purple")]
    public required string Purple { get; init; }

    /// <summary>
    /// Cyan color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("cyan")]
    public required string Cyan { get; init; }

    /// <summary>
    /// White color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("white")]
    public required string White { get; init; }

    /// <summary>
    /// Bright black color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightBlack")]
    public required string BrightBlack { get; init; }

    /// <summary>
    /// Bright red color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightRed")]
    public required string BrightRed { get; init; }

    /// <summary>
    /// Bright green color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightGreen")]
    public required string BrightGreen { get; init; }

    /// <summary>
    /// Bright yellow color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightYellow")]
    public required string BrightYellow { get; init; }

    /// <summary>
    /// Bright blue color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightBlue")]
    public required string BrightBlue { get; init; }

    /// <summary>
    /// Bright purple color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightPurple")]
    public required string BrightPurple { get; init; }

    /// <summary>
    /// Bright cyan color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightCyan")]
    public required string BrightCyan { get; init; }

    /// <summary>
    /// Bright white color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("brightWhite")]
    public required string BrightWhite { get; init; }

    /// <summary>
    /// Background color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("background")]
    public required string Background { get; init; }

    /// <summary>
    /// Foreground color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("foreground")]
    public required string Foreground { get; init; }

    /// <summary>
    /// Cursor color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("cursorColor")]
    public string? CursorColor { get; init; }

    /// <summary>
    /// Selection background color. Accepts a string in hex format "#rgb" or "#rrggbb"
    /// </summary>
    [JsonPropertyName("selectionBackground")]
    public string? SelectionBackground { get; init; }
}