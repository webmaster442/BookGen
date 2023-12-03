//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using BookGen.DomainServices;

using BookGen.Resources;

namespace BookGen.ShortCodes;

[Export(typeof(ITemplateShortCode))]
[BuiltInShortCode]
public sealed class JSPageToc : ITemplateShortCode
{
    public string Tag => nameof(JSPageToc);

    public bool CanCacheResult => true;

    public ShortCodeInfo HelpInfo => new()
    {
        Description = "Generate a table of contents from the headings",
        ArgumentInfos = new ArgumentInfo[]
        {
            new()
            {
                Name = "ContentsDiv",
                Description = "Contents div to scan for chapters",
                Optional = false,
            },
            new()
            {
                Name = "TargetDiv",
                Description = "Target div, where toc will be inserted",
                Optional = false,
            }
        }
    };

    public string Generate(IArguments arguments)
    {
        string? contentsDiv = arguments.GetArgumentOrThrow<string>("ContentsDiv");
        string? targetDiv = arguments.GetArgumentOrThrow<string>("TargetDiv");

        var writer = new StringBuilder();

        string? pagetoc = ResourceHandler.GetFile(KnownFile.PageTocJs);

        string? code = pagetoc.Replace("{{contents}}", contentsDiv).Replace("{{target}}", targetDiv);

        return writer.WriteJavaScript(code).ToString();
    }
}
