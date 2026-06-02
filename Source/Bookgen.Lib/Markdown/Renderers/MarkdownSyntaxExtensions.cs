//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Markdig.Helpers;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers;

internal static class MarkdownSyntaxExtensions
{
    public static string GetCode(this LeafBlock node)
    {
        var code = new StringBuilder();
        StringLine[] lines = node.Lines.Lines;
        int totalLines = lines.Length;
        for (int i = 0; i < totalLines; i++)
        {
            StringLine line = lines[i];
            StringSlice slice = line.Slice;
            if (slice.Text == null)
            {
                continue;
            }

            var lineText = slice.Text.Substring(slice.Start, slice.Length);
            if (i > 0)
            {
                code.AppendLine();
            }

            code.Append(lineText);
        }

        return code.ToString();
    }
}
