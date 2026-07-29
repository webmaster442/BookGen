//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Text;
using System.Web;

namespace BookGen.Lib.Rendering.Markdown.Renderers.SyntaxRenderPlugins;

internal sealed class CsvRenderPlugin : SyntaxRendererPlugin
{
    private const string ComaSeperated = "table-csv";
    private const string TabSeperated = "table-tsv";
    private const string SemicolonSeperated = "table-ssv";

    public override string[] LanguageMonikers { get; } 
        = [ComaSeperated, TabSeperated, SemicolonSeperated];

    public override string Render(string code, string parsedLanguageMoniker)
    {
        var rendered = new StringBuilder(code.Length);
        char delimiter = GetDelimiter(parsedLanguageMoniker);
        using var parser = new CsvParser(delimiter, code);
        rendered.AppendLine("<table>");
        foreach (List<string> record in parser.GetRecords())
        {
            rendered.AppendLine("<tr>");
            foreach (string field in record)
            {
                rendered.AppendLine($"<td>{HttpUtility.HtmlEncode(field)}</td>");
            }
            rendered.AppendLine("</tr>");
        }
        rendered.AppendLine("</table>");
        return rendered.ToString();
    }

    private static char GetDelimiter(string parsedLanguageMoniker)
    {
        return parsedLanguageMoniker switch
        {
            ComaSeperated => ',',
            TabSeperated => '\t',
            SemicolonSeperated => ';',
            _ => throw new UnreachableException($"Unknown language moniker: {parsedLanguageMoniker}"),
        };
    }

    internal sealed class CsvParser : IDisposable
    {
        private readonly StringReader _reader;
        private readonly char _delimiter;

        public CsvParser(char delimiter, string input)
        {
            _reader = new StringReader(input);
            _delimiter = delimiter;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public IEnumerable<List<string>> GetRecords()
        {
            string? line;
            while ((line = _reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    yield return ParseLine(line);
            }
        }

        private List<string> ParseLine(string line)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == _delimiter && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            result.Add(current.ToString());
            return result;
        }
    }
}
