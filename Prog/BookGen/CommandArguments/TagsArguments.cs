using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.CommandArguments
{
    public sealed class TagsArguments : BookGenArgumentBase
    {
        [Switch("a", "auto", false)]
        public bool AutoGenerateTags { get; set; }

        [Switch("k", "keywordcount")]
        public int AutoKeyWordCount { get; set; }

        public TagsArguments()
        {
            AutoGenerateTags = false;
            AutoKeyWordCount = 10;
        }
    }
}
