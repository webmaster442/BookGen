//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices.Markdown.TableOfContents;

internal sealed class TocState
{
    internal HeadingInfos Headings { get; }

    public TocState()
    {
        Headings = new HeadingInfos() { IsLocator = true, Level = -1 };
    }

    internal void AddHeading(HeadingInfo info)
        => Headings.Append(HeadingInfos.FromHeading(info));
}
