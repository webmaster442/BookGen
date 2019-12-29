//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Tests.Stubs
{
    internal class ArgumentNameYielderShortCode : ITemplateShortCode
    {
        public string Tag => "yield";

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            return arguments.Keys.First();
        }
    }
}
