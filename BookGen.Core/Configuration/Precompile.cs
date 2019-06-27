//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookGen.Core.Configuration
{

    public class Precompile
    {
        public List<string> CSSFiles { get; set; }
        public List<string> JavascriptFiles { get; set; }

        public static Precompile CreateDefault()
        {
            return new Precompile
            {
                CSSFiles = new List<string>(),
                JavascriptFiles = new List<string>(),
            };
        }

    }
}
