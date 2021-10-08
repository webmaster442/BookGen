//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Core.Contracts;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Shortcodes
{
    [Export(typeof(ITemplateShortCode))]
    public class InlineFile : ITemplateShortCode
    {
        private readonly ILog _log;

        public string Tag => nameof(InlineFile);

        public bool CanCacheResult => true;

        [ImportingConstructor]
        public InlineFile(ILog log)
        {
            _log = log;
        }

        public string Generate(IArguments arguments)
        {
            var name = arguments.GetArgumentOrThrow<string>("file");

            FsPath file = new FsPath(name);

            _log.Detail("Inlineing {0}...", file);

            return file.ReadFile(_log);
        }
    }
}
