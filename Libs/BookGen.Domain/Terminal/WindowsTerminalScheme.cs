//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.Terminal;

using System.Text.Json.Serialization;

public class WindowsTerminalScheme
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("black")]
    public string Black { get; set; }

    [JsonPropertyName("red")]
    public string Red { get; set; }

    [JsonPropertyName("green")]
    public string Green { get; set; }

    [JsonPropertyName("yellow")]
    public string Yellow { get; set; }

    [JsonPropertyName("blue")]
    public string Blue { get; set; }

    [JsonPropertyName("purple")]
    public string Purple { get; set; }

    [JsonPropertyName("cyan")]
    public string Cyan { get; set; }

    [JsonPropertyName("white")]
    public string White { get; set; }

    [JsonPropertyName("brightBlack")]
    public string BrightBlack { get; set; }

    [JsonPropertyName("brightRed")]
    public string BrightRed { get; set; }

    [JsonPropertyName("brightGreen")]
    public string BrightGreen { get; set; }

    [JsonPropertyName("brightYellow")]
    public string BrightYellow { get; set; }

    [JsonPropertyName("brightBlue")]
    public string BrightBlue { get; set; }

    [JsonPropertyName("brightPurple")]
    public string BrightPurple { get; set; }

    [JsonPropertyName("brightCyan")]
    public string BrightCyan { get; set; }

    [JsonPropertyName("brightWhite")]
    public string BrightWhite { get; set; }

    [JsonPropertyName("background")]
    public string Background { get; set; }

    [JsonPropertyName("foreground")]
    public string Foreground { get; set; }

    [JsonPropertyName("cursorColor")]
    public string CursorColor { get; set; }

    [JsonPropertyName("selectionBackground")]
    public string SelectionBackground { get; set; }

    public WindowsTerminalScheme()
    {
        Name = DefaultShemeName;
        Black = "#0a0520";
        Red = "#ff796d";
        Green = "#99b481";
        Yellow = "#efdfac";
        Blue = "#66d9ef";
        Purple = "#e78fcd";
        Cyan = "#ba8cff";
        White = "#ffba81";
        BrightBlack = "#100b23";
        BrightRed = "#f99f92";
        BrightGreen = "#b4be8f";
        BrightYellow = "#f2e9bf";
        BrightBlue = "#79daed";
        BrightPurple = "#ba91d4";
        BrightCyan = "#a0a0d6";
        BrightWhite = "#b9aed3";
        Background = "#2a1a4a";
        Foreground = "#ece7fa";
        CursorColor = "#c7c7c7";
        SelectionBackground = "#8689c2";
    }

    public const string DefaultShemeName = "purplepeter";
}