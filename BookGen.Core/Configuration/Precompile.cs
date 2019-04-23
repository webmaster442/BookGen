//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookGen.Core.Configuration
{

    public class Precompile: ConfigurationBase
    {
        public List<string> CSSFiles { get; set; }
        public List<string> JavascriptFiles { get; set; }

        [JsonIgnore]
        public string CSSFilesString
        {
            get { return string.Join("\r\n", CSSFiles); }
            set
            {
                CSSFiles = new List<string>(value.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
                base.OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string JavascriptFilesString
        {
            get { return string.Join("\r\n", JavascriptFiles); }
            set
            {
                JavascriptFiles = new List<string>(value.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries));
                base.OnPropertyChanged();
            }
        }

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
