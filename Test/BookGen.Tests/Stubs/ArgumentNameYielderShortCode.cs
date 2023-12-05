//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests.Stubs
{
    internal class ArgumentNameYielderShortCode : ITemplateShortCode
    {
        public string Tag => "yield";

        public ShortCodeInfo HelpInfo => ShortCodeInfo.Empty;

        public bool CanCacheResult => false;
        public string Generate(IArguments arguments)
        {
            return arguments.First().Key;
        }
    }
}
