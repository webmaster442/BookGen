// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

/// <summary>
/// Class to represent color preference options for various Markdown elements.
/// </summary>
public sealed class PSMarkdownOptionInfo
{
    private const char Esc = (char)0x1b;
    private const string EndSequence = "[0m";

    /// <summary>
    /// Gets or sets current VT100 escape sequence for header 1.
    /// </summary>
    public string Header1 { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for header 2.
    /// </summary>
    public string Header2 { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for header 3.
    /// </summary>
    public string Header3 { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for header 4.
    /// </summary>
    public string Header4 { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for header 5.
    /// </summary>
    public string Header5 { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for header 6.
    /// </summary>
    public string Header6 { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for code inline and code blocks.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for links.
    /// </summary>
    public string Link { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for images.
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for bold text.
    /// </summary>
    public string EmphasisBold { get; set; }

    /// <summary>
    /// Gets or sets current VT100 escape sequence for italics text.
    /// </summary>
    public string EmphasisItalics { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether VT100 escape sequences should be added. Default it true.
    /// </summary>
    public bool EnableVT100Encoding { get; set; }

    /// <summary>
    /// Get the property as an rendered escape sequence.
    /// This is used by formatting system for displaying.
    /// </summary>
    /// <param name="propertyName">Name of the property to get as escape sequence.</param>
    /// <returns>Specified property name as escape sequence.</returns>
    public string AsEscapeSequence(MarkdownOptionInfoProperty propertyName)
    {
        return propertyName switch
        {
            MarkdownOptionInfoProperty.Header1 => string.Concat(Esc, Header1, Header1, Esc, EndSequence),
            MarkdownOptionInfoProperty.Header2 => string.Concat(Esc, Header2, Header2, Esc, EndSequence),
            MarkdownOptionInfoProperty.Header3 => string.Concat(Esc, Header3, Header3, Esc, EndSequence),
            MarkdownOptionInfoProperty.Header4 => string.Concat(Esc, Header4, Header4, Esc, EndSequence),
            MarkdownOptionInfoProperty.Header5 => string.Concat(Esc, Header5, Header5, Esc, EndSequence),
            MarkdownOptionInfoProperty.Header6 => string.Concat(Esc, Header6, Header6, Esc, EndSequence),
            MarkdownOptionInfoProperty.Code => string.Concat(Esc, Code, Code, Esc, EndSequence),
            MarkdownOptionInfoProperty.Link => string.Concat(Esc, Link, Link, Esc, EndSequence),
            MarkdownOptionInfoProperty.Image => string.Concat(Esc, Image, Image, Esc, EndSequence),
            MarkdownOptionInfoProperty.EmphasisBold => string.Concat(Esc, EmphasisBold, EmphasisBold, Esc, EndSequence),
            MarkdownOptionInfoProperty.EmphasisItalics => string.Concat(Esc, EmphasisItalics, EmphasisItalics, Esc, EndSequence),
            _ => throw new InvalidOperationException($"Unknown value: {propertyName}"),
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PSMarkdownOptionInfo"/> class and sets dark as the default theme.
    /// </summary>
    public PSMarkdownOptionInfo()
    {
        SetDarkTheme();
        EnableVT100Encoding = true;
    }

    private const string Header1Dark = "[7m";
    private const string Header2Dark = "[4;93m";
    private const string Header3Dark = "[4;94m";
    private const string Header4Dark = "[4;95m";
    private const string Header5Dark = "[4;96m";
    private const string Header6Dark = "[4;97m";
    private const string CodeDark = "[48;2;155;155;155;38;2;30;30;30m";
    private const string CodeMacOS = "[107;95m";
    private const string LinkDark = "[4;38;5;117m";
    private const string ImageDark = "[33m";
    private const string EmphasisBoldDark = "[1m";
    private const string EmphasisItalicsDark = "[36m";

    private const string Header1Light = "[7m";
    private const string Header2Light = "[4;33m";
    private const string Header3Light = "[4;34m";
    private const string Header4Light = "[4;35m";
    private const string Header5Light = "[4;36m";
    private const string Header6Light = "[4;30m";
    private const string CodeLight = "[48;2;155;155;155;38;2;30;30;30m";
    private const string LinkLight = "[4;38;5;117m";
    private const string ImageLight = "[33m";
    private const string EmphasisBoldLight = "[1m";
    private const string EmphasisItalicsLight = "[36m";

    /// <summary>
    /// Set all preference for dark theme.
    /// </summary>
    [MemberNotNull(nameof(Header1),
                   nameof(Header2),
                   nameof(Header3),
                   nameof(Header4),
                   nameof(Header5),
                   nameof(Header6),
                   nameof(Link),
                   nameof(Image),
                   nameof(EmphasisBold),
                   nameof(EmphasisItalics),
                   nameof(Code))]
    public void SetDarkTheme()
    {
        Header1 = Header1Dark;
        Header2 = Header2Dark;
        Header3 = Header3Dark;
        Header4 = Header4Dark;
        Header5 = Header5Dark;
        Header6 = Header6Dark;
        Link = LinkDark;
        Image = ImageDark;
        EmphasisBold = EmphasisBoldDark;
        EmphasisItalics = EmphasisItalicsDark;
        SetCodeColor(isDarkTheme: true);
    }

    /// <summary>
    /// Set all preference for light theme.
    /// </summary>
    [MemberNotNull(nameof(Header1),
               nameof(Header2),
               nameof(Header3),
               nameof(Header4),
               nameof(Header5),
               nameof(Header6),
               nameof(Link),
               nameof(Image),
               nameof(EmphasisBold),
               nameof(EmphasisItalics),
               nameof(Code))]
    public void SetLightTheme()
    {
        Header1 = Header1Light;
        Header2 = Header2Light;
        Header3 = Header3Light;
        Header4 = Header4Light;
        Header5 = Header5Light;
        Header6 = Header6Light;
        Link = LinkLight;
        Image = ImageLight;
        EmphasisBold = EmphasisBoldLight;
        EmphasisItalics = EmphasisItalicsLight;
        SetCodeColor(isDarkTheme: false);
    }

    [MemberNotNull(nameof(Code))]
    private void SetCodeColor(bool isDarkTheme)
    {
        // MacOS terminal app does not support extended colors for VT100, so we special case for it.
        Code = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? CodeMacOS : isDarkTheme ? CodeDark : CodeLight;
    }
}
