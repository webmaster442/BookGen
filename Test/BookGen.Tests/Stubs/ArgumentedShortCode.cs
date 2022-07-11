//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests.Stubs
{
    internal class ArgumentedShortCode : ITemplateShortCode
    {
        public string Tag => "Arguments";

        public bool CanCacheResult => false;

        public string Generate(IArguments arguments)
        {
            return arguments.GetArgumentOrThrow<string>("parameter");
        }
    }
}
