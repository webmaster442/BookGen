//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Rendering.Markdown.TableOfContents;

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
