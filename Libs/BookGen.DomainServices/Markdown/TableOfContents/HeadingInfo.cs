//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
// Based on work of Alexandre Mutel. https://github.com/leisn/MarkdigToc
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices.Markdown.TableOfContents;

internal sealed record class HeadingInfo
{
    public string Content { get; }
    public string Id { get; }
    public int Level { get; }

    public HeadingInfo(int level, string id, string content)
    {
        Content = content;
        Id = id;
        Level = level;
    }
}
