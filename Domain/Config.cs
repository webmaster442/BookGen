//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    public class Config
    {
        public string OutputDir { get; set; }
        public string TOCFile { get; set; }
        public string ImageDir { get; set; }
        public string HostName { get; set; }

        public static Config Default
        {
            get
            {
                return new Config
                {
                    OutputDir = "Path to output directory",
                    TOCFile = "Path of table of contents",
                    ImageDir = "Path to images directory",
                    HostName = "http://localhost:8080/"
                };
            }
        }
    }
}
