//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Interfaces;

namespace BookGen.Framework.Shortcodes
{
    public class DelegateShortCode : ITemplateShortCode
    {
        private readonly Func<IArguments, string> _generator;

        public bool CanCacheResult => false;

        public DelegateShortCode(string tag, Func<IArguments, string> generator)
        {
            Tag = tag;
            _generator = generator;
        }

        public string Tag { get; }

        public string Generate(IArguments arguments)
        {
            return _generator.Invoke(arguments);
        }
    }
}
