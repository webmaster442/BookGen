//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public sealed class Config
    {
        [Doc("Table of contents Markdown file")]
        public string TOCFile
        {
            get;
            set;
        }

        [Doc("Image directory relative to workdir")]
        public string ImageDir
        {
            get;
            set;
        }

        [Doc("Publish host name. Must include protocoll (http or https) and must end with a /")]
        public string HostName
        {
            get;
            set;
        }

        [Doc("Index or first page.")]
        public string Index
        {
            get;
            set;
        }

        [Doc("Config file version. !DO NOT CHANGE!")]
        public int Version
        {
            get;
            set;
        }

        [Doc("Config file version. !DO NOT CHANGE!")]
        public bool LinksOutSideOfHostOpenNewTab
        {
            get;
            set;
        }

        [Doc("Inline images as base64 that are less then this size in bytes. 0 = inlines all files")]
        public long InlineImageSizeLimit
        {
            get;
            set;
        }

        [Doc("Metadata information for output")]
        public Metadata Metadata
        {
            get;
            set;
        }

        [Doc("Web output configuration")]
        public BuildConfig TargetWeb
        {
            get;
            set;
        }

        [Doc("Printable HTML output configuration")]
        public BuildConfig TargetPrint
        {
            get;
            set;
        }

        [Doc("e-pub format output configuration")]
        public BuildConfig TargetEpub
        {
            get;
            set;
        }

        [Doc("Wordpress compatible xml output configuration")]
        public BuildConfig TargetWordpress
        {
            get;
            set;
        }

        [Doc("Translateable strings that can be used in the template", TypeAlias = typeof(Dictionary<string, string>))]
        public Translations Translations
        {
            get;
            set;
        }

        public static Config CreateDefault(int version = 100)
        {
            var config = new Config
            {
                TargetWeb = BuildConfig.CreateDefault("output/web"),
                TargetEpub = BuildConfig.CreateDefault("output/epub"),
                TargetPrint = BuildConfig.CreateDefault("output/print"),
                TargetWordpress = BuildConfig.CreateDefault("output/wordpress"),
                Translations = Translations.CreateDefault(),
                TOCFile = "Path of table of contents",
                Index = "Path of startup (index) file",
                ImageDir = "Path to images directory",
                HostName = "http://localhost:8080/",
                Metadata = Metadata.CreateDefault(),
                Version = version,
                LinksOutSideOfHostOpenNewTab = true,
                InlineImageSizeLimit = 50 * 1024
            };

            config.AddBootStrapClassesForWeb();
            config.AddWordpressSettings();

            return config;
        }
    }
}
