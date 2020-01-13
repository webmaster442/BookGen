//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Template;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Shortcodes
{
    [Export(typeof(ITemplateShortCode))]
    public class JSPageToc : ITemplateShortCode
    {
        public string Tag => nameof(JSPageToc);

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            var contentsDiv = arguments.GetArgumentOrThrow("ContentsDiv");
            var targetDiv = arguments.GetArgumentOrThrow("TargetDiv");

            FluentHtmlWriter writer = new FluentHtmlWriter();

            var code = ResourceLocator.GetResourceFile<BuiltInTemplates>("/Scripts/PageToc.js").Replace("{{contents}}", contentsDiv).Replace("{{target}}", targetDiv);

            return writer.WriteJavaScript(code).ToString();
        }
    }
}
