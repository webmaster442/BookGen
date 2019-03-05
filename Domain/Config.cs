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
        public string AssetsDir { get; set; }
        public string Template { get; set; }
        public string Index { get; set; }
        public int Version { get; set; }
        public StyleClasses StyleClasses { get; set; }
        public SearchSettings SearchOptions { get; set; }

        public static Config Default
        {
            get
            {
                return new Config
                {
                    OutputDir = "Path to output directory",
                    TOCFile = "Path of table of contents",
                    Index = "Path of startup (index) file",
                    ImageDir = "Path to images directory",
                    HostName = "http://localhost:8080/",
                    AssetsDir = "Path to static assets required by template or null",
                    Template = "Path of template file",
                    StyleClasses = new StyleClasses(),
                    SearchOptions = SearchSettings.Default,
                    Version = 100,
                };
            }
        }
    }
}
