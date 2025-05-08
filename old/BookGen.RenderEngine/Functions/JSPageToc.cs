//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using BookGen.DomainServices;
using BookGen.RenderEngine.Internals;
using BookGen.Resources;

namespace BookGen.RenderEngine.Functions;

internal sealed class JSPageToc : Function
{
    public override string Execute(FunctionArguments arguments)
    {
        string? contentsDiv = arguments.GetArgumentOrThrow<string>("ContentsDiv");
        string? targetDiv = arguments.GetArgumentOrThrow<string>("TargetDiv");

        var writer = new StringBuilder();

        string? pagetoc = ResourceHandler.GetFile(KnownFile.PageTocJs);

        string? code = pagetoc.Replace("{{contents}}", contentsDiv).Replace("{{target}}", targetDiv);

        return writer.WriteJavaScript(code).ToString();
    }

    protected override FunctionInfo GetInformation()
    {
        return new FunctionInfo
        {
            Name = "JSPageToc",
            Description = "Generate a table of contents from the headings",
            ArgumentInfos = new Internals.ArgumentInfo[]
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
    }
}