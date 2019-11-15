//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bookgen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class InlineFile : ITemplateShortCode
    {
        private readonly ILog _log;

        public string Tag => nameof(InlineFile);

        [ImportingConstructor]
        public InlineFile(ILog log)
        {
            _log = log;
        }

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
           var name = arguments.GetArgumentOrThrow("file");

            FsPath file = new FsPath(name);

            _log.Detail("Inlineing {0}...", file);

            return file.ReadFile(_log);
        }
    }
}
