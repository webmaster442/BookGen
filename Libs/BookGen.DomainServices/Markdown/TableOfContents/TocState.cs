//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices.Markdown.TableOfContents;

internal sealed class TocState
{
    private readonly object _lock;
    private readonly HeadingInfos _headings;

    internal HeadingInfos Headings
    {
        get
        {
            lock (_lock)
            {
                return _headings;
            }
        }
    }

    internal int MaxLevel { get; set; }

    public TocState()
    {
        _lock = new object();
        _headings = new HeadingInfos() { IsLocator = true, Level = -1 };
        MaxLevel = int.MaxValue;
    }

    internal void AddHeading(HeadingInfo info)
    {
        lock (_lock)
        {
            Headings.Append(HeadingInfos.FromHeading(info));
        }
    }
}
