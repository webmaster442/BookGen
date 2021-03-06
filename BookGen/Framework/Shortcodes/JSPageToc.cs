﻿//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Resources;
using System.ComponentModel.Composition;
using System.Text;

namespace BookGen.Framework.Shortcodes
{
    [Export(typeof(ITemplateShortCode))]
    public class JSPageToc : ITemplateShortCode
    {
        public string Tag => nameof(JSPageToc);

        public bool CanCacheResult => true;

        public string Generate(IArguments arguments)
        {
            var contentsDiv = arguments.GetArgumentOrThrow<string>("ContentsDiv");
            var targetDiv = arguments.GetArgumentOrThrow<string>("TargetDiv");

            StringBuilder writer = new StringBuilder();

            var pagetoc = ResourceHandler.GetFile(KnownFile.PageTocJs);

            var code = pagetoc.Replace("{{contents}}", contentsDiv).Replace("{{target}}", targetDiv);

            return writer.WriteJavaScript(code).ToString();
        }
    }
}
