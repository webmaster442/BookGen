//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;

namespace BookGen.Framework.Shortcodes
{
    public class DelegateShortCode : ITemplateShortCode
    {
        private readonly Func<IReadOnlyDictionary<string, string>, string> _generator;

        public DelegateShortCode(string tag, Func<IReadOnlyDictionary<string, string>, string> generator)
        {
            Tag = tag;
            _generator = generator;
        }

        public string Tag { get;  }

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            return _generator.Invoke(arguments);
        }
    }
}
