//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using BookGen.Resources;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Shortcodes
{
    [Export(typeof(ITemplateShortCode))]
    public class JSPageToc : ITemplateShortCode
    {
        public string Tag => nameof(JSPageToc);

        public bool CanCacheResult => true;

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
}
