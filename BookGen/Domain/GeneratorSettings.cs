//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using System.Collections.Generic;

namespace BookGen.Domain
{
    public class GeneratorSettings
    {
        public FsPath OutputDirectory { get; set; }
        public FsPath SourceDirectory { get; set; }
        public FsPath ImageDirectory { get; set; }
        public FsPath Toc { get; set; }
        public TOC TocContents { get; set; }
        public Config Configruation { get; set; }
        public Dictionary<string, string> Metatadas { get; set; }
        public Dictionary<string, string> InlineImgs { get; set; }
    }
}
