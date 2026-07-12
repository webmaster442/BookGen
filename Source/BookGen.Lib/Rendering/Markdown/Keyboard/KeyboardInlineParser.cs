//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Helpers;
using Markdig.Parsers;

namespace BookGen.Lib.Rendering.Markdown.Keyboard;

internal sealed class KeyboardInlineParser : InlineParser
{
    public KeyboardInlineParser()
    {
        OpeningCharacters = ['<'];
    }

    public override bool Match(InlineProcessor processor, ref StringSlice slice)
    {
        bool matchFound;
        char next;

        matchFound = false;

        next = slice.PeekCharExtra(1);

        if (next == '<')
        {
            char current;
            int start;
            int end;

            slice.NextChar();
            slice.NextChar(); // skip the opening pair

            current = slice.CurrentChar;
            start = slice.Start;
            end = start;

            while (current != '\0' && current != '>')
            {
                end++;
                current = slice.NextChar();
            }

            if (end > start && current == '>' && slice.PeekCharExtra(1) == '>')
            {
                int inlineStart;

                end--;
                inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

                processor.Inline = new KeyboardInline
                {
                    Span =
                    {
                        Start = inlineStart,
                        End = inlineStart + (end - start)
                    },
                    Line = line,
                    Column = column,
                    Text = new StringSlice(slice.Text, start, end)
                };

                slice.NextChar();
                slice.NextChar(); // skip the closing characters

                matchFound = true;
            }
        }

        return matchFound;
    }
}
