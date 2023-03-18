//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework;

internal sealed class StaticTemplateContent : IContent
{
    public string Content { get; set; }
    public string Title { get; set; }
    public string TableOfContents { get; set; }
    public string Metadata { get; set; }
    public string HostUrl { get; set; }
    public string PrecompiledHeader { get; set; }

    public StaticTemplateContent()
    {
        Content = string.Empty;
        Title = string.Empty;
        TableOfContents = string.Empty;
        Metadata = string.Empty;
        HostUrl = string.Empty;
        PrecompiledHeader = string.Empty;
    }
}
