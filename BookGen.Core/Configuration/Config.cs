//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Core.Configuration
{
    public class Config
    {
        [Description("Output directory for generated files")]
        public string OutputDir { get; set; }
        [Description("Table of contents file")]
        public string TOCFile { get; set; }
        [Description("Images directory. Will be copied to output directory")]
        public string ImageDir { get; set; }
        [Description("Target host name")]
        public string HostName { get; set; }
        [Description("Additional assets directory. Will be copied to output directory")]
        public string AssetsDir { get; set; }
        [Description("Template file for webpage")]
        public string Template { get; set; }
        [Description("Template file for epub")]
        public string TemplateEpub { get; set; }
        [Description("Index page")]
        public string Index { get; set; }
        [Browsable(false)]
        public int Version { get; set; }
        [Description("Open links that point outside of the host in new tab")]
        public bool LinksOutSideOfHostOpenNewTab { get; set; }
        [Description("Inlime inimage size limit in bytes. -1 disables function")]
        public long InlineImageSizeLimit { get; set; }

        [Browsable(false)]
        public StyleClasses StyleClasses { get; set; }

        [Browsable(false)]
        public SearchSettings SearchOptions { get; set; }

        [Browsable(false)]
        public Metadata Metadata { get; set; }

        [Browsable(false)]
        public Precompile PrecompileHeader { get; set; }

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
                    TemplateEpub = "Path of epub template file",
                    StyleClasses = new StyleClasses(),
                    SearchOptions = SearchSettings.Default,
                    Metadata = Metadata.Default,
                    PrecompileHeader = Precompile.Default,
                    Version = 100,
                    LinksOutSideOfHostOpenNewTab = true,
                    InlineImageSizeLimit = 50 * 1024
                };
            }
        }
    }
}
