//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Contracts
{
    public interface IContent
    {
        string Content { get; set; }
        string Title { get; set; }
        string TableOfContents { get; set; }
        string Metadata { get; set; }
        string HostUrl { get; }
        string PrecompiledHeader { get; set; }
    }
}
