//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Contracts;
using System.Linq;

namespace BookGen.Tests.Stubs
{
    internal class ArgumentNameYielderShortCode : ITemplateShortCode
    {
        public string Tag => "yield";

        public string Generate(IArguments arguments)
        {
            return arguments.First().Key;
        }
    }
}
