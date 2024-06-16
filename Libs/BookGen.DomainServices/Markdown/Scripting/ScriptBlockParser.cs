//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace BookGen.DomainServices.Markdown.Scripting;

internal sealed class ScriptBlockParser : FencedBlockParserBase<ScriptBlock>
{
    public ScriptBlockParser()
    {
        OpeningCharacters = new[] { '`' };
        InfoPrefix = "script";
        InfoParser = ScriptInfoParser;
    }

    protected override ScriptBlock CreateFencedBlock(BlockProcessor processor)
    {
        var block = new ScriptBlock(this);
        return block;
    }

    private bool ScriptInfoParser(BlockProcessor state,
                                  ref StringSlice line,
                                  IFencedBlock fenced,
                                  char openingCharacter)
    {
        string infoString;
        string? argString = null;

        var c = line.CurrentChar;
        // An info string cannot contain any backsticks
        int firstSpace = -1;
        for (int i = line.Start; i <= line.End; i++)
        {
            c = line.Text[i];
            if (c == '`')
            {
                return false;
            }

            if (firstSpace < 0 && c.IsSpaceOrTab())
            {
                firstSpace = i;
            }
        }

        if (firstSpace > 0)
        {
            infoString = line.Text[line.Start..firstSpace].Trim();

            // Skip any spaces after info string
            firstSpace++;
            while (true)
            {
                c = line[firstSpace];
                if (c.IsSpaceOrTab())
                {
                    firstSpace++;
                }
                else
                {
                    break;
                }
            }

            argString = line.Text.Substring(firstSpace, line.End - firstSpace + 1).Trim();
        }
        else
        {
            infoString = line.ToString().Trim();
        }

        if (infoString != "script")
            return false;

        fenced.Info = HtmlHelper.Unescape(infoString);
        fenced.Arguments = HtmlHelper.Unescape(argString);

        return true;
    }
}