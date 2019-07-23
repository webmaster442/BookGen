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
        public string Tag => nameof(InlineFile);

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
           var name = arguments.GetArgumentOrThrow("file");

            FsPath file = new FsPath(name);

            return file.ReadFile();
        }
    }
}
