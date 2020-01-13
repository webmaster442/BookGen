//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Contracts;

namespace BookGen.Tests.Stubs
{
    class ArgumentedShortCode : ITemplateShortCode
    {
        public string Tag => "Arguments";

        public string Generate(IArguments arguments)
        {
            return arguments.GetArgumentOrThrow<string>("parameter");
        }
    }
}
