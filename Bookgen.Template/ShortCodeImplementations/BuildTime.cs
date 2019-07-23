//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bookgen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class BuildTime : ITemplateShortCode
    {
        public string Tag => nameof(BuildTime);

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            return DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
        }
    }
}
