//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookGen.Editor.Models
{
    public class EditorSession
    {
        [JsonIgnore]
        public string WorkDirectory { get; set; }

        [JsonIgnore]
        public string DictionaryPath { get; set; }

        public HashSet<string> PreviousWorkdirs { get; set; }

        public EditorSession()
        {
            PreviousWorkdirs = new HashSet<string>();
            WorkDirectory = string.Empty;
        }
    }
}
