//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core.Contracts;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Shortcodes
{
    [Export(typeof(ITemplateShortCode))]
    public class BuildTime : ITemplateShortCode
    {
        public string Tag => nameof(BuildTime);

        public bool CanCacheResult => false;

        public string Generate(IArguments arguments)
        {
            return DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
        }
    }
}
