//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace BookGen.DomainServices.Markdown.TableOfContents;

internal sealed partial class TocBlockParser : BlockParser, IAttributesParseable
{
    public TocState Options { get; }
    public TryParseAttributesDelegate? TryParseAttributes { get; set; }

    public TocBlockParser(TocState options)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        OpeningCharacters = new[] { '[' };
    }

    [GeneratedRegex("^(\\[toc\\])|(\\[toc maxlevel=\\\"\\d+\\\"\\])")]
    private static partial Regex TocTagMatcher();

    [GeneratedRegex("\\[toc maxlevel=\\\"(\\d+)\\\"\\]")]
    private static partial Regex MaxLevelMatcher();

    public override BlockState TryOpen(BlockProcessor processor)
    {
        if (processor.IsCodeIndent)
            return BlockState.None;

        StringSlice line = processor.Line;
        int column = processor.Column;
        int sourcePosition = line.Start;

        var matches = TocTagMatcher().Matches(line.ToString());

        if (matches.Count < 1)
            return BlockState.None;

        int tagLength = matches.Select(x => x.Value.Length).Sum();

        var maxLevelMathes = MaxLevelMatcher().Matches(line.ToString());
        if (maxLevelMathes.Count > 0)
        {
            if (int.TryParse(maxLevelMathes[0].Groups.Values.Last().Value, out int maxLevel))
            {
                Options.MaxLevel = maxLevel;
            }
        }

        var c = line.CurrentChar;
        for (int i = 0; i < tagLength; i++)
            c = processor.NextChar();

        StringSlice trivia = StringSlice.Empty;
        if (processor.TrackTrivia && c.IsSpaceOrTab())
        {
            trivia = new StringSlice(processor.Line.Text, processor.Start, processor.Start);
            processor.NextChar();
        }

        var block = new TocBlock(this)
        {
            Column = column,
            Span = { Start = sourcePosition },
            TriviaAfterAtxHeaderChar = trivia,
            TriviaBefore = processor.UseTrivia(sourcePosition - 1),
            LinesBefore = processor.LinesBefore,
            NewLine = processor.Line.NewLine,
        };
        processor.LinesBefore = null;

        processor.NewBlocks.Push(block);
        if (!processor.TrackTrivia)
        {
            processor.GoToColumn(column + tagLength + 1);
        }
        TryParseAttributes?.Invoke(processor, ref processor.Line, block);

        block.Span.End = processor.Line.End;

        if (processor.TrackTrivia)
        {
            var wsa = new StringSlice(processor.Line.Text, processor.Line.End + 1, processor.Line.End);
            block.TriviaAfter = wsa;
            if (wsa.Overlaps(block.TriviaAfterAtxHeaderChar))
                block.TriviaAfter = StringSlice.Empty;
        }

        return BlockState.Break;
    }

    public override bool Close(BlockProcessor processor, Block block)
    {
        if (!processor.TrackTrivia)
        {
            (block as TocBlock)?.Lines.Trim();
        }
        return true;
    }
}
