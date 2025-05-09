//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Markdown.TableOfContents;

internal sealed class TocState
{
    internal HeadingInfos Headings { get; }

    internal int MaxLevel { get; set; }

    public TocState()
    {
        Headings = new HeadingInfos() { IsLocator = true, Level = -1 };
        MaxLevel = int.MaxValue;
    }

    internal void AddHeading(HeadingInfo info)
    {
        Headings.Append(HeadingInfos.FromHeading(info));
    }
}
