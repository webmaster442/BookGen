//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using BookGen.Core;

namespace BookGen.Core.Configuration
{
    public class Precompile
    {
        [Description("List of CSS files to inline in header of template")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," + "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public List<string> CSSFiles { get; set; }
        [Description("List of JS files to inline in header of template")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," + "System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",  typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public List<string> JavascriptFiles { get; set; }

        public static Precompile Default
        {
            get
            {
                return new Precompile
                {
                    CSSFiles = new List<string>(),
                    JavascriptFiles = new List<string>(),
                };
            }
        }

    }
}
