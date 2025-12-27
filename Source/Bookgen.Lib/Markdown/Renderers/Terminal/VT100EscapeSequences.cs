// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

/// <summary>
/// Class to represent default VT100 escape sequences.
/// </summary>
public class VT100EscapeSequences
{
    private const char Esc = (char)0x1B;

    private readonly string _endSequence = Esc + "[0m";

    // For code blocks, [500@ make sure that the whole line has background color.
    private const string LongBackgroundCodeBlock = "[500@";

    private readonly PSMarkdownOptionInfo _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="VT100EscapeSequences"/> class.
    /// </summary>
    /// <param name="optionInfo">PSMarkdownOptionInfo object to initialize with.</param>
    public VT100EscapeSequences(PSMarkdownOptionInfo optionInfo)
    {
        _options = optionInfo ?? throw new ArgumentNullException(nameof(optionInfo));
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="headerText">Text of the header to format.</param>
    /// <returns>Formatted Header 1 string.</returns>
    public string FormatHeader1(string headerText)
    {
        return FormatHeader(headerText, _options.Header1);
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="headerText">Text of the header to format.</param>
    /// <returns>Formatted Header 2 string.</returns>
    public string FormatHeader2(string headerText)
    {
        return FormatHeader(headerText, _options.Header2);
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="headerText">Text of the header to format.</param>
    /// <returns>Formatted Header 3 string.</returns>
    public string FormatHeader3(string headerText)
    {
        return FormatHeader(headerText, _options.Header3);
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="headerText">Text of the header to format.</param>
    /// <returns>Formatted Header 4 string.</returns>
    public string FormatHeader4(string headerText)
    {
        return FormatHeader(headerText, _options.Header4);
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="headerText">Text of the header to format.</param>
    /// <returns>Formatted Header 5 string.</returns>
    public string FormatHeader5(string headerText)
    {
        return FormatHeader(headerText, _options.Header5);
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="headerText">Text of the header to format.</param>
    /// <returns>Formatted Header 6 string.</returns>
    public string FormatHeader6(string headerText)
    {
        return FormatHeader(headerText, _options.Header6);
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="codeText">Text of the code block to format.</param>
    /// <param name="isInline">True if it is a inline code block, false otherwise.</param>
    /// <returns>Formatted code block string.</returns>
    public string FormatCode(string codeText, bool isInline)
    {
        bool isVT100Enabled = _options.EnableVT100Encoding;

        if (isInline)
        {
            if (isVT100Enabled)
            {
                return string.Concat(Esc, _options.Code, codeText, _endSequence);
            }
            else
            {
                return codeText;
            }
        }
        else
        {
            if (isVT100Enabled)
            {
                return string.Concat(Esc, _options.Code, codeText, Esc, LongBackgroundCodeBlock, _endSequence);
            }
            else
            {
                return codeText;
            }
        }
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="linkText">Text of the link to format.</param>
    /// <param name="url">URL of the link.</param>
    /// <param name="hideUrl">True url should be hidden, false otherwise. Default is true.</param>
    /// <returns>Formatted link string.</returns>
    public string FormatLink(string linkText, string url, bool hideUrl = true)
    {
        bool isVT100Enabled = _options.EnableVT100Encoding;

        if (hideUrl)
        {
            if (isVT100Enabled)
            {
                return string.Concat(Esc, _options.Link, "\"", linkText, "\"", _endSequence);
            }
            else
            {
                return string.Concat("\"", linkText, "\"");
            }
        }
        else
        {
            if (isVT100Enabled)
            {
                return string.Concat("\"", linkText, "\" (", Esc, _options.Link, url, _endSequence, ")");
            }
            else
            {
                return string.Concat("\"", linkText, "\" (", url, ")");
            }
        }
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="emphasisText">Text to format as emphasis.</param>
    /// <param name="isBold">True if it is to be formatted as bold, false to format it as italics.</param>
    /// <returns>Formatted emphasis string.</returns>
    public string FormatEmphasis(string emphasisText, bool isBold)
    {
        var sequence = isBold ? _options.EmphasisBold : _options.EmphasisItalics;

        if (_options.EnableVT100Encoding)
        {
            return string.Concat(Esc, sequence, emphasisText, _endSequence);
        }
        else
        {
            return emphasisText;
        }
    }

    /// <summary>
    /// Class to represent default VT100 escape sequences.
    /// </summary>
    /// <param name="altText">Text of the image to format.</param>
    /// <returns>Formatted image string.</returns>
    public string FormatImage(string? altText)
    {
        var text = altText;

        if (string.IsNullOrEmpty(altText))
        {
            text = "Image";
        }

        if (_options.EnableVT100Encoding)
        {
            return string.Concat(Esc, _options.Image, "[", text, "]", _endSequence);
        }
        else
        {
            return string.Concat("[", text, "]");
        }
    }

    private string FormatHeader(string headerText, string headerEscapeSequence)
    {
        if (_options.EnableVT100Encoding)
        {
            return string.Concat(Esc, headerEscapeSequence, headerText, _endSequence);
        }
        else
        {
            return headerText;
        }
    }
}
