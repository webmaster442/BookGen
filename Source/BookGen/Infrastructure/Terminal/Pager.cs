//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Text.RegularExpressions;

using Spectre.Console;

namespace BookGen.Infrastructure.Terminal;

/// <summary>
/// Allows to display text in a paginated way
/// </summary>
public partial class Pager : WigetBase
{
    private readonly List<Page> _pages;
    private readonly PagerOptions _options;

    /// <summary>
    /// Creates a new pager. The pager will read the text from the reader and paginate it
    /// </summary>
    /// <param name="reader">A Text reader that supplies the text</param>
    /// /// <param name="options">Pager options</param>
    public Pager(TextReader reader, PagerOptions? options = default)
    {
        if (options == default)
            options = PagerOptions.Default;

        _options = options;
        _pages = new();
        AddPages(reader);
    }

    /// <summary>
    /// Creates a new pager. The pager will paginate the text
    /// </summary>
    /// <param name="text">Text to paginate</param>
    /// <param name="options">Pager options</param>
    public Pager(string text, PagerOptions? options = default)
    {
        if (options == default)
            options = PagerOptions.Default;

        _options = options;
        _pages = new();
        using var reader = new StringReader(text);
        AddPages(reader);
    }

    /// <summary>
    /// Hides the pager
    /// </summary>
    public override void OnHide()
    {
        //Empty logic
    }

    /// <summary>
    /// Shows the pager
    /// </summary>
    public override void OnShow()
    {
        int currentPage = 0;
        while (IsShowing)
        {
            Console.Clear();
            foreach (var line in _pages[currentPage])
            {
                if (line == null) continue;
                AnsiConsole.WriteLine(_options.LineFormatter(line));
            }
            var navbar = $"{currentPage + 1} of {_pages.Count} | esc/q: quit | up: previous | down: next";
            AnsiConsole.WriteLine("═".PadLeft(_options.PageWidth, '═'));
            AnsiConsole.Write(navbar);

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    Hide();
                    return;
                case ConsoleKey.UpArrow:
                case ConsoleKey.LeftArrow:
                case ConsoleKey.PageUp:
                    currentPage = Math.Max(0, currentPage - 1);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.PageDown:
                    currentPage = Math.Min(_pages.Count - 1, currentPage + 1);
                    break;
            }
        }
    }

    private const string AnsiReset = "\x1b[0m";

    private void AddPages(TextReader reader)
    {
        Page current = new(_options.PageHeight);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            foreach (var toAdd in SplitToVisibleWidth(line, _options.PageWidth))
            {
                if (!current.TryAdd(toAdd))
                {
                    _pages.Add(current);
                    current = new Page(_options.PageHeight);
                    current.TryAdd(toAdd);
                }
            }
        }
        _pages.Add(current);
        ApplyFormattingAcrossPages();
    }

    /// <summary>
    /// Splits a line into chunks that fit into the given visible width.
    /// ANSI escape sequences are kept intact (never split) and do not count
    /// towards the visible width, so a chunk/page break can never break a
    /// formatting or reset sequence.
    /// </summary>
    private static IEnumerable<string> SplitToVisibleWidth(string line, int width)
    {
        if (line.Length == 0)
        {
            yield return string.Empty;
            yield break;
        }

        var builder = new System.Text.StringBuilder();
        int visible = 0;
        int index = 0;

        while (index < line.Length)
        {
            Match match = AnsiEscapeRegex().Match(line, index);
            if (match.Success && match.Index == index)
            {
                builder.Append(match.Value);
                index += match.Length;
                continue;
            }

            builder.Append(line[index]);
            index++;
            visible++;

            if (visible == width)
            {
                yield return builder.ToString();
                builder.Clear();
                visible = 0;
            }
        }

        if (builder.Length > 0)
            yield return builder.ToString();
    }

    private void ApplyFormattingAcrossPages()
    {
        for (int i = 0; i < _pages.Count - 1; i++)
        {
            string activeFormatting = GetActiveFormatting(_pages[i]);
            if (activeFormatting.Length > 0)
            {
                _pages[i].AppendToLastLine(AnsiReset);
                _pages[i + 1].PrependToFirstLine(activeFormatting);
            }
        }
    }

    private static string GetActiveFormatting(Page page)
    {
        var activeCodes = new List<string>();

        foreach (var line in page)
        {
            if (line == null) continue;
            foreach (Match match in AnsiEscapeRegex().Matches(line))
            {
                // Extract the parameters between the '[' and the trailing 'm'
                var parameters = match.Value[2..^1];
                // An empty parameter list (\x1b[m) is equivalent to a reset
                var codes = parameters.Length == 0
                    ? ["0"]
                    : parameters.Split(';');

                foreach (var code in codes)
                {
                    // '0' (or an empty code) resets all previously active formatting
                    if (code.Length == 0 || code == "0")
                        activeCodes.Clear();
                    else
                        activeCodes.Add(code);
                }
            }
        }

        return activeCodes.Count == 0
            ? string.Empty
            : $"\x1b[{string.Join(';', activeCodes)}m";
    }


    private class Page : IEnumerable<string>
    {
        private readonly string[] _lines;
        private int _index;

        public Page(int count)
        {
            _lines = new string[count];
        }

        public IEnumerator<string> GetEnumerator()
            => ((IEnumerable<string>)_lines).GetEnumerator();

        public bool TryAdd(string line)
        {
            if (_index < _lines.Length)
            {
                _lines[_index++] = line;
                return true;
            }
            return false;
        }

        public void AppendToLastLine(string text)
        {
            if (_index > 0)
            {
                _lines[_index - 1] += text;
            }
        }

        public void PrependToFirstLine(string text)
        {
            if (_index > 0)
            {
                _lines[0] = text + _lines[0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => _lines.GetEnumerator();
    }

    [GeneratedRegex(@"\x1b\[[0-9;]*m", RegexOptions.Compiled, 2000)]
    private static partial Regex AnsiEscapeRegex();
}
